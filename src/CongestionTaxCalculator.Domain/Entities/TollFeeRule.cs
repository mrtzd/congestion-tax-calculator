namespace CongestionTaxCalculator.Domain.Entities;

public class TollFeeRule
{
    public int Id { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int Amount { get; set; }
    public int CityId { get; set; }
    public City City { get; set; } = null!;
}
