namespace LegacyRenewalApp.Strategies;

public class LoyaltyDiscountStrategy : IDiscountStrategy
{
    public decimal Apply(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, ref string notes)
    {
        decimal discount = 0;

        if (customer.YearsWithCompany >= 5)
        {
            discount = baseAmount * 0.07m;
            notes += "long-term loyalty discount; ";
        }
        else if (customer.YearsWithCompany >= 2)
        {
            discount = baseAmount * 0.03m;
            notes += "basic loyalty discount; ";
        }

        return discount;
    }
}