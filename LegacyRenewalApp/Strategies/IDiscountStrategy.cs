namespace LegacyRenewalApp.Strategies;

public interface IDiscountStrategy
{
    decimal Apply(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, ref string notes);
}