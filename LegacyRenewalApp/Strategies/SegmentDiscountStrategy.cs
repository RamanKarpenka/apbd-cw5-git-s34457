namespace LegacyRenewalApp.Strategies;

public class SegmentDiscountStrategy : IDiscountStrategy
{
    public decimal Apply(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, ref string notes)
    {
        decimal discount = 0;

        if (customer.Segment == "Silver")
        {
            discount = baseAmount * 0.05m;
            notes += "silver discount; ";
        }
        else if (customer.Segment == "Gold")
        {
            discount = baseAmount * 0.10m;
            notes += "gold discount; ";
        }
        else if (customer.Segment == "Platinum")
        {
            discount = baseAmount * 0.15m;
            notes += "platinum discount; ";
        }
        else if (customer.Segment == "Education" && plan.IsEducationEligible)
        {
            discount = baseAmount * 0.20m;
            notes += "education discount; ";
        }

        return discount;
    }
}