using System;
namespace LegacyRenewalApp.Domain;

public class PaymentFeeCalculator
{
    public decimal Calculate(string method, decimal amount, ref string notes)
    {
        method = method.ToUpperInvariant();

        if (method == "CARD")
        {
            notes += "card payment fee; ";
            return amount * 0.02m;
        }
        if (method == "BANK_TRANSFER")
        {
            notes += "bank transfer fee; ";
            return amount * 0.01m;
        }
        if (method == "PAYPAL")
        {
            notes += "paypal fee; ";
            return amount * 0.035m;
        }
        if (method == "INVOICE")
        {
            notes += "invoice payment; ";
            return 0m;
        }

        throw new ArgumentException("Unsupported payment method");
    }
}