using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Enums;

namespace CongestionTaxCalculator.Domain.Tests.Helpers;

public static class TestDataFactory
{
    // Helper method to create a standard Gothenburg City for testing
    public static City CreateGothenburgTestCity()
    {
        var city = new City
        {
            Name = "Gothenburg",
            MaxDailyFee = 60,
            SingleChargeRuleMinutes = 60,
            IsDayBeforePublicHolidayTollFree = true,
            TollFreeMonths = { new TollFreeMonth { Month = 7 } },
            TollFreeWeekdays =
            {
                new TollFreeWeekday { DayOfWeek = DayOfWeek.Saturday },
                new TollFreeWeekday { DayOfWeek = DayOfWeek.Sunday }
            },
            VehicleExemptions =
            {
                new VehicleExemption { VehicleType = VehicleType.Motorbike },
                new VehicleExemption { VehicleType = VehicleType.Bus },
                new VehicleExemption { VehicleType = VehicleType.Emergency },
                new VehicleExemption { VehicleType = VehicleType.Diplomat },
                new VehicleExemption { VehicleType = VehicleType.Military },
                new VehicleExemption { VehicleType = VehicleType.Foreign },
            },
            PublicHolidays =
            {
                new PublicHoliday { Date = new DateOnly(2013, 1, 1) },
                new PublicHoliday { Date = new DateOnly(2013, 1, 6) },
                new PublicHoliday { Date = new DateOnly(2013, 3, 29) },
                new PublicHoliday { Date = new DateOnly(2013, 3, 31) },
                new PublicHoliday { Date = new DateOnly(2013, 4, 1) },
                new PublicHoliday { Date = new DateOnly(2013, 5, 1) },
                new PublicHoliday { Date = new DateOnly(2013, 5, 9) },
                new PublicHoliday { Date = new DateOnly(2013, 5, 19) },
                new PublicHoliday { Date = new DateOnly(2013, 6, 6) },
                new PublicHoliday { Date = new DateOnly(2013, 6, 22) },
                new PublicHoliday { Date = new DateOnly(2013, 11, 2) },
                new PublicHoliday { Date = new DateOnly(2013, 12, 25) },
                new PublicHoliday { Date = new DateOnly(2013, 12, 26) }
            },
            TollFeeRules =
            {
                new TollFeeRule { StartTime = new TimeOnly(6, 0),  EndTime = new TimeOnly(6, 29, 59), Amount = 8 },
                new TollFeeRule { StartTime = new TimeOnly(6, 30), EndTime = new TimeOnly(6, 59, 59), Amount = 13 },
                new TollFeeRule { StartTime = new TimeOnly(7, 0),  EndTime = new TimeOnly(7, 59, 59), Amount = 18 },
                new TollFeeRule { StartTime = new TimeOnly(8, 0),  EndTime = new TimeOnly(8, 29, 59), Amount = 13 },
                new TollFeeRule { StartTime = new TimeOnly(8, 30), EndTime = new TimeOnly(14, 59, 59), Amount = 8 },
                new TollFeeRule { StartTime = new TimeOnly(15, 0), EndTime = new TimeOnly(15, 29, 59), Amount = 13 },
                new TollFeeRule { StartTime = new TimeOnly(15, 30),EndTime = new TimeOnly(16, 59, 59), Amount = 18 },
                new TollFeeRule { StartTime = new TimeOnly(17, 0), EndTime = new TimeOnly(17, 59, 59), Amount = 13 },
                new TollFeeRule { StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(18, 29, 59), Amount = 8 },
            }
        };
        return city;
    }
}
