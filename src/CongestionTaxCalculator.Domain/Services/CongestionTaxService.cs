using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Policies;

namespace CongestionTaxCalculator.Domain.Services;

public class CongestionTaxService(
    TollExemptionPolicy exemptionPolicy,
    DateTollPolicy datePolicy,
    TollFeePolicy feePolicy) : ICongestionTaxService
{
    public int CalculateTax(Vehicle vehicle, IReadOnlyList<DateTime> dates, City city)
    {
        var exemptVehicleTypes = city.VehicleExemptions.Select(ve => ve.VehicleType).ToHashSet();
        if (exemptionPolicy.IsTollFree(vehicle, exemptVehicleTypes) || !dates.Any())
        {
            return 0;
        }

        var groupedByDay = dates.OrderBy(d => d).GroupBy(d => d.Date);
        int totalTax = 0;

        var tollFreeWeekdays = city.TollFreeWeekdays.Select(w => w.DayOfWeek).ToHashSet();
        var tollFreeMonths = city.TollFreeMonths.Select(m => m.Month).ToHashSet();
        var publicHolidays = city.PublicHolidays.Select(h => h.Date).ToHashSet();

        var daysBeforePublicHolidays = new HashSet<DateOnly>();
        if (city.IsDayBeforePublicHolidayTollFree)
        {
            daysBeforePublicHolidays = publicHolidays.Select(h => h.AddDays(-1)).ToHashSet();
        }

        foreach (var dayGroup in groupedByDay)
        {
            if (datePolicy.IsTollFree(DateOnly.FromDateTime(dayGroup.Key), tollFreeWeekdays, tollFreeMonths, publicHolidays, daysBeforePublicHolidays))
                continue;

            totalTax += CalculateFeeForDay(dayGroup.ToList(), city.TollFeeRules, city.MaxDailyFee, city.SingleChargeRuleMinutes);
        }

        return totalTax;
    }

    private int CalculateFeeForDay(List<DateTime> dates, IEnumerable<TollFeeRule> feeRules, int maxDailyFee, int windowMinutes)
    {
        int dailyFee = 0;
        DateTime windowStart = dates[0];
        int windowHighestFee = feePolicy.GetFeeForTime(TimeOnly.FromDateTime(windowStart), feeRules);

        foreach (var date in dates.Skip(1))
        {
            int currentFee = feePolicy.GetFeeForTime(TimeOnly.FromDateTime(date), feeRules);
            TimeSpan diff = date - windowStart;

            if (diff.TotalMinutes <= windowMinutes)
            {
                if (currentFee > windowHighestFee)
                    windowHighestFee = currentFee;
            }
            else
            {
                dailyFee += windowHighestFee;
                windowStart = date;
                windowHighestFee = currentFee;
            }
        }
        dailyFee += windowHighestFee;
        return Math.Min(dailyFee, maxDailyFee);
    }
}
