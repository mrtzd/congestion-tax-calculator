using CongestionTaxCalculator.Domain.Enums;

namespace CongestionTaxCalculator.Domain.Entities;

public class VehicleExemption
{
    public int Id { get; set; }
    public VehicleType VehicleType { get; set; }
    public int CityId { get; set; }
    public City City { get; set; } = null!;
}
