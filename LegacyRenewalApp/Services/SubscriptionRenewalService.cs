using System;
using System.Collections.Generic;

using LegacyRenewalApp;
using LegacyRenewalApp.Domain;
using LegacyRenewalApp.Infrastructure;
using LegacyRenewalApp.Strategies;

public class SubscriptionRenewalService
{
    private readonly CustomerRepository _customerRepository;
    private readonly SubscriptionPlanRepository _planRepository;
    private readonly IBillingGateway _billingGateway;

    public SubscriptionRenewalService()
        : this(
            new CustomerRepository(),
            new SubscriptionPlanRepository(),
            new BillingGatewayWrapper())
    {
    }

    public SubscriptionRenewalService(
        CustomerRepository customerRepository,
        SubscriptionPlanRepository planRepository,
        IBillingGateway billingGateway)
    {
        _customerRepository = customerRepository;
        _planRepository = planRepository;
        _billingGateway = billingGateway;
    }

    public RenewalInvoice CreateRenewalInvoice(
        int customerId,
        string planCode,
        int seatCount,
        string paymentMethod,
        bool includePremiumSupport,
        bool useLoyaltyPoints)
    {
        Validate(customerId, planCode, seatCount, paymentMethod);

        var customer = _customerRepository.GetById(customerId);
        var plan = _planRepository.GetByCode(planCode);

        if (!customer.IsActive)
            throw new InvalidOperationException("Inactive customers cannot renew subscriptions");

        decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;

        string notes = "";

        var discountService = new DiscountService(new List<IDiscountStrategy>
        {
            new SegmentDiscountStrategy(),
            new LoyaltyDiscountStrategy(),
            new SeatDiscountStrategy(),
            new LoyaltyPointsStrategy(useLoyaltyPoints)
        });

        decimal discount = discountService.Calculate(customer, plan, seatCount, baseAmount, ref notes);

        decimal subtotal = baseAmount - discount;

        if (subtotal < 300m)
        {
            subtotal = 300m;
            notes += "minimum discounted subtotal applied; ";
        }

        var supportCalc = new SupportFeeCalculator();
        decimal supportFee = supportCalc.Calculate(plan.Code, includePremiumSupport, ref notes);

        var paymentCalc = new PaymentFeeCalculator();
        decimal paymentFee = paymentCalc.Calculate(paymentMethod, subtotal + supportFee, ref notes);

        var taxCalc = new TaxCalculator();
        decimal taxRate = taxCalc.GetRate(customer.Country);

        decimal taxBase = subtotal + supportFee + paymentFee;
        decimal tax = taxBase * taxRate;

        decimal final = taxBase + tax;

        if (final < 500m)
        {
            final = 500m;
            notes += "minimum invoice amount applied; ";
        }

        var invoice = new RenewalInvoice
        {
            InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customerId}-{plan.Code}",
            CustomerName = customer.FullName,
            PlanCode = plan.Code,
            PaymentMethod = paymentMethod.ToUpperInvariant(),
            SeatCount = seatCount,
            BaseAmount = Math.Round(baseAmount, 2),
            DiscountAmount = Math.Round(discount, 2),
            SupportFee = Math.Round(supportFee, 2),
            PaymentFee = Math.Round(paymentFee, 2),
            TaxAmount = Math.Round(tax, 2),
            FinalAmount = Math.Round(final, 2),
            Notes = notes.Trim(),
            GeneratedAt = DateTime.UtcNow
        };

        _billingGateway.SaveInvoice(invoice);

        if (!string.IsNullOrWhiteSpace(customer.Email))
        {
            _billingGateway.SendEmail(
                customer.Email,
                "Subscription renewal invoice",
                $"Hello {customer.FullName}, final amount: {invoice.FinalAmount:F2}");
        }

        return invoice;
    }

    private void Validate(int customerId, string planCode, int seatCount, string paymentMethod)
    {
        if (customerId <= 0)
            throw new ArgumentException("Customer id must be positive");

        if (string.IsNullOrWhiteSpace(planCode))
            throw new ArgumentException("Plan code is required");

        if (seatCount <= 0)
            throw new ArgumentException("Seat count must be positive");

        if (string.IsNullOrWhiteSpace(paymentMethod))
            throw new ArgumentException("Payment method is required");
    }
}