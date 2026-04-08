namespace LegacyRenewalApp.Strategies;

public class SeatDiscountStrategy : IDiscountStrategy
{
    public decimal Apply(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, ref string notes)
    {
        decimal discount = 0;

        if (seatCount >= 50)
        {
            discount = baseAmount * 0.12m;
            notes += "large team discount; ";
        }
        else if (seatCount >= 20)
        {
            discount = baseAmount * 0.08m;
            notes += "medium team discount; ";
        }
        else if (seatCount >= 10)
        {
            discount = baseAmount * 0.04m;
            notes += "small team discount; ";
        }

        return discount;
    }
}