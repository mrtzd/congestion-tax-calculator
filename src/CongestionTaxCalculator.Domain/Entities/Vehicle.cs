
using CongestionTaxCalculator.Domain.Enums;

namespace CongestionTaxCalculator.Domain.Entities;

public class Vehicle(VehicleType type)
{
    public VehicleType Type { get; } = type;
}
