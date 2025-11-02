using CongestionTaxCalculator.Domain.Policies;

namespace CongestionTaxCalculator.Domain.Tests.Policies;

public class DateTollPolicyTests
{
    private readonly DateTollPolicy _policy = new(2013);

    [Theory]
    [InlineData(2013, 2, 16)] // Saturday
    [InlineData(2013, 2, 17)] // Sunday
    public void IsTollFree_ShouldReturnTrue_ForWeekends(int year, int month, int day)
    {
        Assert.True(_policy.IsTollFree(new DateTime(year, month, day)));
    }

    [Fact]
    public void IsTollFree_ShouldReturnTrue_ForMonthOfJuly()
    {
        var date = new DateTime(2013, 7, 10);

        Assert.True(_policy.IsTollFree(date));
    }

    [Theory]
    [InlineData(1, 1)]   // New Year's Day
    [InlineData(4, 1)]   // Easter Monday
    [InlineData(5, 1)]   // Labour Day
    [InlineData(12, 25)] // Christmas Day
    public void IsTollFree_ShouldReturnTrue_ForPublicHolidays(int month, int day)
    {
        Assert.True(_policy.IsTollFree(new DateTime(2013, month, day)));
    }

    [Theory]
    [InlineData(3, 28)]  // Day before Good Friday
    [InlineData(4, 30)]  // Day before Labour Day
    [InlineData(12, 24)] // Christmas Eve
    [InlineData(12, 31)] // New Year's Eve
    public void IsTollFree_ShouldReturnTrue_ForDaysBeforePublicHolidays(int month, int day)
    {
        Assert.True(_policy.IsTollFree(new DateTime(2013, month, day)));
    }

    [Fact]
    public void IsTollFree_ShouldReturnFalse_ForRegularWeekday()
    {
        var date = new DateTime(2013, 2, 11); // A Monday

        Assert.False(_policy.IsTollFree(date));
    }
}
