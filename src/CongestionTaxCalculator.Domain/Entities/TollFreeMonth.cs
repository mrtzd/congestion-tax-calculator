namespace CongestionTaxCalculator.Domain.Entities;

public class TollFreeMonth
{
    public int Id { get; set; }
    public int Month { get; set; }
    public int CityId { get; set; }
    public City City { get; set; } = null!;
}
