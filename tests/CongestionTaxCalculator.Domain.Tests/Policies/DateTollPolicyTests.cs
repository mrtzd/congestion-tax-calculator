using CongestionTaxCalculator.Domain.Policies;

namespace CongestionTaxCalculator.Domain.Tests.Policies;

public class DateTollPolicyTests
{
    private readonly DateTollPolicy _policy = new();

    [Fact]
    public void IsTollFree_ShouldReturnTrue_ForDayInTollFreeWeekdaySet()
    {
        // Arrange
        var date = new DateOnly(2013, 1, 5); // A Saturday
        var freeWeekdays = new HashSet<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday };

        // Act & Assert
        Assert.True(_policy.IsTollFree(date, freeWeekdays, [], [], []));
    }

    [Fact]
    public void IsTollFree_ShouldReturnTrue_ForDayBeforePublicHoliday()
    {
        // Arrange
        var date = new DateOnly(2013, 12, 31); // New Year's Eve
        var daysBeforeHolidays = new HashSet<DateOnly> { new(2013, 12, 31) };

        // Act & Assert
        Assert.True(_policy.IsTollFree(date, [], [], [], daysBeforeHolidays));
    }

    [Fact]
    public void IsTollFree_ShouldReturnFalse_WhenDateIsNotExempt()
    {
        // Arrange
        var date = new DateOnly(2013, 10, 10); // A regular weekday

        // Act & Assert
        Assert.False(_policy.IsTollFree(date, [], [], [], []));
    }
}
