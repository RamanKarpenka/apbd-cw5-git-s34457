using System.Collections.Generic;
using LegacyRenewalApp.Strategies;
namespace LegacyRenewalApp.Domain;

public class DiscountService
{
    private readonly IEnumerable<IDiscountStrategy> _strategies;

    public DiscountService(IEnumerable<IDiscountStrategy> strategies)
    {
        _strategies = strategies;
    }

    public decimal Calculate(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, ref string notes)
    {
        decimal total = 0;

        foreach (var strategy in _strategies)
        {
            total += strategy.Apply(customer, plan, seatCount, baseAmount, ref notes);
        }

        return total;
    }
}