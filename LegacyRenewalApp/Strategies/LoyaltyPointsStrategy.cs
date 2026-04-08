namespace LegacyRenewalApp.Strategies;

public class LoyaltyPointsStrategy : IDiscountStrategy
{
    private readonly bool _usePoints;

    public LoyaltyPointsStrategy(bool usePoints)
    {
        _usePoints = usePoints;
    }

    public decimal Apply(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, ref string notes)
    {
        if (!_usePoints || customer.LoyaltyPoints <= 0)
            return 0;

        int points = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;
        notes += $"loyalty points used: {points}; ";
        return points;
    }
}