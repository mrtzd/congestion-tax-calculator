
namespace CongestionTaxCalculator.Domain.Policies;

public class DateTollPolicy(int year)
{
    private readonly HashSet<DateTime> _publicHolidays = GetPublicHolidays(year);

    public bool IsTollFree(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        if (date.Month == 7) // July is toll-free
        {
            return true;
        }

        // Check for public holidays and the day before
        if (_publicHolidays.Contains(date.Date) || _publicHolidays.Contains(date.Date.AddDays(1)))
        {
            return true;
        }

        return false;
    }

    private static HashSet<DateTime> GetPublicHolidays(int year)
    {
        if (year == 2013)
        {
            return
            [
                new DateTime(2013, 1, 1),   // New Year's Day
                new DateTime(2013, 1, 6),   // Epiphany
                new DateTime(2013, 3, 29),  // Good Friday
                new DateTime(2013, 4, 1),   // Easter Monday
                new DateTime(2013, 5, 1),   // Labour Day
                new DateTime(2013, 5, 9),   // Ascension Day
                new DateTime(2013, 6, 6),   // National Day of Sweden
                new DateTime(2013, 6, 22),  // Midsummer Day
                new DateTime(2013, 11, 2),  // All Saints' Day
                new DateTime(2013, 12, 25), // Christmas Day
                new DateTime(2013, 12, 26), // Boxing Day

                new DateTime(2013, 12, 31), // New Year's Eve
            ];
        }
        return [];
    }
}
