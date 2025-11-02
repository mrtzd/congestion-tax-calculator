using CongestionTaxCalculator.Domain.Policies;

namespace CongestionTaxCalculator.Domain.Tests.Policies;

public class TollFeePolicyTests
{
    private readonly TollFeePolicy _policy = new();

    [Theory]
    [InlineData("06:15:00", 8)]   // In the first bracket
    [InlineData("06:45:00", 13)]  // In the second bracket
    [InlineData("07:30:00", 18)]  // In the third bracket
    [InlineData("08:15:00", 13)]  // In the fourth bracket
    [InlineData("12:00:00", 8)]   // In the long day bracket
    [InlineData("15:10:00", 13)]  // Afternoon bracket
    [InlineData("16:00:00", 18)]  // Afternoon rush hour
    [InlineData("17:30:00", 13)]  // Evening rush hour
    [InlineData("18:15:00", 8)]   // Last paid bracket
    public void GetFeeForTime_ShouldReturnCorrectFee_ForPaidHours(string time, int expectedFee)
    {
        var passageTime = TimeOnly.Parse(time);

        var fee = _policy.GetFeeForTime(passageTime);

        Assert.Equal(expectedFee, fee);
    }

    [Theory]
    [InlineData("05:59:59")] // Before morning rush
    [InlineData("18:30:00")] // After evening rush
    [InlineData("22:00:00")] // Late night
    public void GetFeeForTime_ShouldReturnZero_ForFreeHours(string time)
    {
        var passageTime = TimeOnly.Parse(time);

        var fee = _policy.GetFeeForTime(passageTime);

        Assert.Equal(0, fee);
    }
}
