namespace CongestionTaxCalculator.Domain.Policies;

public class DateTollPolicy
{
    public bool IsTollFree(
        DateOnly date,
        HashSet<DayOfWeek> tollFreeWeekdays,
        HashSet<int> tollFreeMonths,
        HashSet<DateOnly> publicHolidays,
        HashSet<DateOnly> daysBeforePublicHolidays)
    {
        if (tollFreeWeekdays.Contains(date.DayOfWeek))
            return true;

        if (tollFreeMonths.Contains(date.Month))
            return true;

        if (publicHolidays.Contains(date))
            return true;

        if (daysBeforePublicHolidays.Contains(date))
            return true;

        return false;
    }
}
