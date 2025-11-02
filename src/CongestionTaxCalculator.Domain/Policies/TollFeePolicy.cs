using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.Policies;

public class TollFeePolicy
{
    private readonly List<TollFeeRule> _rules;

    public TollFeePolicy()
    {
        // This configuration is specific to Gothenburg.
        _rules =
        [
            new(new TimeOnly(6, 0), new TimeOnly(6, 29, 59), 8),
            new(new TimeOnly(6, 30), new TimeOnly(6, 59, 59), 13),
            new(new TimeOnly(7, 0), new TimeOnly(7, 59, 59), 18),
            new(new TimeOnly(8, 0), new TimeOnly(8, 29, 59), 13),
            new(new TimeOnly(8, 30), new TimeOnly(14, 59, 59), 8),
            new(new TimeOnly(15, 0), new TimeOnly(15, 29, 59), 13),
            new(new TimeOnly(15, 30), new TimeOnly(16, 59, 59), 18),
            new(new TimeOnly(17, 0), new TimeOnly(17, 59, 59), 13),
            new(new TimeOnly(18, 0), new TimeOnly(18, 29, 59), 8)
        ];
    }

    public int GetFeeForTime(TimeOnly time)
    {
        var rule = _rules.FirstOrDefault(r => time >= r.StartTime && time <= r.EndTime);
        return rule?.Amount ?? 0;
    }
}