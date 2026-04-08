using System;
namespace LegacyRenewalApp.Domain;

public class SupportFeeCalculator
{
    public decimal Calculate(string planCode, bool includeSupport, ref string notes)
    {
        if (!includeSupport) return 0;

        notes += "premium support included; ";

        return planCode switch
        {
            "START" => 250m,
            "PRO" => 400m,
            "ENTERPRISE" => 700m,
            _ => 0m
        };
    }
}