namespace CongestionTaxCalculator.Domain.Tests.Entities;

using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Enums;
using Xunit;

public class VehicleTests
{
    [Theory]
    [InlineData(VehicleType.Car)]
    [InlineData(VehicleType.Motorbike)]
    [InlineData(VehicleType.Tractor)]
    public void Constructor_ShouldSetVehicleTypeCorrectly(VehicleType expectedType)
    {
        // Act
        var vehicle = new Vehicle(expectedType);

        // Assert
        Assert.Equal(expectedType, vehicle.Type);
    }
}
