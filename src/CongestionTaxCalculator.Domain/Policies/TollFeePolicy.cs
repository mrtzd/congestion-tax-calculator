using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Domain.Policies;

public class TollFeePolicy
{
    public int GetFeeForTime(TimeOnly time, IEnumerable<TollFeeRule> rules)
    {
        var rule = rules.FirstOrDefault(r => time >= r.StartTime && time <= r.EndTime);
        return rule?.Amount ?? 0;
    }
}
