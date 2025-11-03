using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Enums;
using CongestionTaxCalculator.Domain.Policies;
using CongestionTaxCalculator.Domain.Services;
using CongestionTaxCalculator.Domain.Tests.Helpers;

namespace CongestionTaxCalculator.Domain.Tests.Services;

public class CongestionTaxServiceTests
{
    private readonly ICongestionTaxService _service;

    public CongestionTaxServiceTests()
    {
        _service = new CongestionTaxService(
            new TollExemptionPolicy(),
            new DateTollPolicy(),
            new TollFeePolicy()
        );
    }

    [Fact]
    public void CalculateTax_ShouldReturnZero_ForTollFreeVehicle()
    {
        // Arrange
        var vehicle = new Vehicle(VehicleType.Motorbike);
        var city = TestDataFactory.CreateGothenburgTestCity();
        var dates = new[] { DateTime.Parse("2013-02-08 07:00:00") };

        // Act
        var tax = _service.CalculateTax(vehicle, dates, city);

        // Assert
        Assert.Equal(0, tax);
    }

    [Fact]
    public void CalculateTax_ShouldReturnZero_ForTollFreeDate()
    {
        // Arrange
        var vehicle = new Vehicle(VehicleType.Car);
        var city = TestDataFactory.CreateGothenburgTestCity();
        var dates = new[] { DateTime.Parse("2013-04-01 10:00:00") }; // Easter Monday

        // Act
        var tax = _service.CalculateTax(vehicle, dates, city);

        // Assert
        Assert.Equal(0, tax);
    }

    [Fact]
    public void CalculateTax_ShouldApplySingleChargeRule_AndUseHighestFeeInWindow()
    {
        // Arrange
        var vehicle = new Vehicle(VehicleType.Car);
        var city = TestDataFactory.CreateGothenburgTestCity();
        // Times: 06:23 (8 SEK), 06:55 (13 SEK) -> Within 60 mins, highest is 13
        var dates = new[]
        {
            DateTime.Parse("2013-02-07 06:23:27"),
            DateTime.Parse("2013-02-07 06:55:00")
        };

        // Act
        var tax = _service.CalculateTax(vehicle, dates, city);

        // Assert
        Assert.Equal(13, tax);
    }

    [Fact]
    public void CalculateTax_ShouldSumFees_ForPassagesMoreThan60MinsApart()
    {
        // Arrange
        var vehicle = new Vehicle(VehicleType.Car);
        var city = TestDataFactory.CreateGothenburgTestCity();
        // Times: 06:23 (8 SEK), 15:27 (13 SEK) -> More than 60 mins apart
        var dates = new[]
        {
            DateTime.Parse("2013-02-07 06:23:27"),
            DateTime.Parse("2013-02-07 15:27:00")
        };

        // Act
        var tax = _service.CalculateTax(vehicle, dates, city);

        // Assert
        Assert.Equal(21, tax); // 8 + 13
    }

    [Fact]
    public void CalculateTax_ShouldHandleMultiplePassages_AndCapAt60SEK()
    {
        // Arrange
        var vehicle = new Vehicle(VehicleType.Car);
        var city = TestDataFactory.CreateGothenburgTestCity();
        var dateStrings = new[]
        {
            "2013-02-08 06:20:27", // Window 1: 06:20 (8)
            "2013-02-08 06:27:00", // Window 1: 06:27 (8) -> Highest in window is 8
            
            "2013-02-08 14:35:00", // Window 2: 14:35 (8) -> Highest in window is 8

            "2013-02-08 15:29:00", // Window 3: 15:29 (13)
            "2013-02-08 15:47:00", // Window 3: 15:47 (18)
            "2013-02-08 16:01:00", // Window 3: 16:01 (18)
            "2013-02-08 16:48:00", // Window 3: 16:48 (18) -> Highest in window is 18
            
            "2013-02-08 17:49:00", // Window 4: 17:49 (13)
            "2013-02-08 18:29:00", // Window 4: 18:29 (8) -> Highest in window is 13

            "2013-02-08 18:35:00"  // Window 5: 18:35 (0) -> Highest in window is 0
        };
        var dates = dateStrings.Select(DateTime.Parse).ToArray();
        // Expected sum: 8 + 8 + 18 + 13 + 0 = 47. Let's add more to test cap.

        var moreDates = new List<DateTime>(dates)
        {
            DateTime.Parse("2013-02-08 07:30:00"), // Add a pass for 18 SEK
            DateTime.Parse("2013-02-08 08:15:00")  // Add a pass for 13 SEK
        };
        // New sum would be 8 + 8 + 18 + 13 + 18 + 13 = 78, which should be capped.

        // Act
        var tax = _service.CalculateTax(vehicle, moreDates, city);

        // Assert
        Assert.Equal(60, tax);
    }

    [Fact]
    public void CalculateTax_ShouldHandleMultipleDays_CapEachDayAndSkipTollFreeDays()
    {
        // Arrange
        var vehicle = new Vehicle(VehicleType.Car);
        var city = TestDataFactory.CreateGothenburgTestCity();
        var dates = new[]
        {
        // Day 1: taxable day (Feb 7, 2013)
        DateTime.Parse("2013-02-07 06:23:00"), // 8
        DateTime.Parse("2013-02-07 07:10:00"), // 18
        DateTime.Parse("2013-02-07 08:25:00"), // 13
        DateTime.Parse("2013-02-07 15:45:00"), // 18
        DateTime.Parse("2013-02-07 16:30:00"), // 18
        DateTime.Parse("2013-02-07 17:30:00"), // 13
        DateTime.Parse("2013-02-07 18:15:00"), // 8
        // Day 2: toll-free day (April 1, 2013 - Easter Monday)
        DateTime.Parse("2013-04-01 07:30:00"), // Toll-free, should be skipped
        // Day 3: taxable day (Feb 8, 2013)
        DateTime.Parse("2013-02-08 06:20:00"), // 8
        DateTime.Parse("2013-02-08 06:50:00"), // 13
        DateTime.Parse("2013-02-08 07:30:00"), // 18
        DateTime.Parse("2013-02-08 15:45:00"), // 18
        DateTime.Parse("2013-02-08 16:10:00"), // 18
        DateTime.Parse("2013-02-08 17:50:00")  // 13
    };

        // Act
        var tax = _service.CalculateTax(vehicle, dates, city);

        // Assert
        // Day 1 and Day 3 both easily exceed 60 SEK total, so they should each be capped.
        // Day 2 (Easter Monday) is toll-free.
        // => 60 (Feb 7) + 0 (Apr 1) + 60 (Feb 8) = 120 SEK total.
        Assert.Equal(120, tax);
    }

}
