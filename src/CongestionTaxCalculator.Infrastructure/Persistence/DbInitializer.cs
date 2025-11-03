using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CongestionTaxCalculator.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        if (context.Cities.Any())
        {
            return;   // DB has been seeded
        }

        var gothenburg = new City
        {
            Name = "Gothenburg",
            MaxDailyFee = 60,
            SingleChargeRuleMinutes = 60,
            IsDayBeforePublicHolidayTollFree = true,
            TollFreeMonths = { new TollFreeMonth { Month = 7 } }, // July
            TollFreeWeekdays =
            {
                new TollFreeWeekday { DayOfWeek = DayOfWeek.Saturday },
                new TollFreeWeekday { DayOfWeek = DayOfWeek.Sunday }
            },
            VehicleExemptions =
            {
                new VehicleExemption { VehicleType = VehicleType.Motorbike },
                new VehicleExemption { VehicleType = VehicleType.Emergency },
                new VehicleExemption { VehicleType = VehicleType.Diplomat },
                new VehicleExemption { VehicleType = VehicleType.Foreign },
                new VehicleExemption { VehicleType = VehicleType.Military },
                new VehicleExemption { VehicleType = VehicleType.Bus },
            },
            PublicHolidays = // Based on the official 2013 list
            {
                new PublicHoliday { Date = new DateOnly(2013, 1, 1) },   // New Year's Day
                new PublicHoliday { Date = new DateOnly(2013, 1, 6) },   // Epiphany
                new PublicHoliday { Date = new DateOnly(2013, 3, 29) },  // Good Friday
                new PublicHoliday { Date = new DateOnly(2013, 4, 1) },   // Easter Monday
                new PublicHoliday { Date = new DateOnly(2013, 5, 1) },   // Labour Day
                new PublicHoliday { Date = new DateOnly(2013, 5, 9) },   // Ascension Day
                new PublicHoliday { Date = new DateOnly(2013, 6, 6) },   // National Day
                new PublicHoliday { Date = new DateOnly(2013, 6, 22) },  // Midsummer Day
                new PublicHoliday { Date = new DateOnly(2013, 11, 2) },  // All Saints' Day
                new PublicHoliday { Date = new DateOnly(2013, 12, 25) }, // Christmas Day
                new PublicHoliday { Date = new DateOnly(2013, 12, 26) }, // Boxing Day
            },
            TollFeeRules =
            {
                new TollFeeRule { StartTime = new TimeOnly(6, 0), EndTime = new TimeOnly(6, 29, 59), Amount = 8 },
                new TollFeeRule { StartTime = new TimeOnly(6, 30), EndTime = new TimeOnly(6, 59, 59), Amount = 13 },
                new TollFeeRule { StartTime = new TimeOnly(7, 0), EndTime = new TimeOnly(7, 59, 59), Amount = 18 },
                new TollFeeRule { StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 29, 59), Amount = 13 },
                new TollFeeRule { StartTime = new TimeOnly(8, 30), EndTime = new TimeOnly(14, 59, 59), Amount = 8 },
                new TollFeeRule { StartTime = new TimeOnly(15, 0), EndTime = new TimeOnly(15, 29, 59), Amount = 13 },
                new TollFeeRule { StartTime = new TimeOnly(15, 30), EndTime = new TimeOnly(16, 59, 59), Amount = 18 },
                new TollFeeRule { StartTime = new TimeOnly(17, 0), EndTime = new TimeOnly(17, 59, 59), Amount = 13 },
                new TollFeeRule { StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(18, 29, 59), Amount = 8 }
            }
        };

        context.Cities.Add(gothenburg);
        context.SaveChanges();
    }
}
