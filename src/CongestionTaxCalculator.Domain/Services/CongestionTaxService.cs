// src/CongestionTaxCalculator.Domain/Services/CongestionTaxService.cs

using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Policies;

namespace CongestionTaxCalculator.Domain.Services;

public class CongestionTaxService(
    TollExemptionPolicy exemptionPolicy,
    DateTollPolicy datePolicy,
    TollFeePolicy feePolicy)
{
    private const int MaxDailyFee = 60;

    public int CalculateTax(Vehicle vehicle, IReadOnlyList<DateTime> dates)
    {
        if (exemptionPolicy.IsTollFree(vehicle) || !dates.Any())
        {
            return 0;
        }

        var sortedDates = dates.OrderBy(d => d).ToList();
        var groupedByDay = sortedDates.GroupBy(d => d.Date);

        int totalTax = 0;

        foreach (var dayGroup in groupedByDay)
        {
            var currentDate = dayGroup.Key;

            // Skip toll-free days
            if (datePolicy.IsTollFree(currentDate))
                continue;

            var dayPassages = dayGroup.OrderBy(d => d).ToList();

            int dailyFee = 0;
            DateTime windowStart = dayPassages.First();
            int windowHighestFee = feePolicy.GetFeeForTime(TimeOnly.FromDateTime(windowStart));

            foreach (var date in dayPassages.Skip(1))
            {
                int currentFee = feePolicy.GetFeeForTime(TimeOnly.FromDateTime(date));
                TimeSpan diff = date - windowStart;

                if (diff.TotalMinutes <= 60)
                {
                    // Still within 60-minute window → keep max fee
                    if (currentFee > windowHighestFee)
                        windowHighestFee = currentFee;
                }
                else
                {
                    // Window ended → add highest fee and start new window
                    dailyFee += windowHighestFee;
                    windowStart = date;
                    windowHighestFee = currentFee;
                }
            }

            // Add fee for the last window
            dailyFee += windowHighestFee;

            // Cap daily fee
            totalTax += Math.Min(dailyFee, MaxDailyFee);
        }

        return totalTax;
    }
}
