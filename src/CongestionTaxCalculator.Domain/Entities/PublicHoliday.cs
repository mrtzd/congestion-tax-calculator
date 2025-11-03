namespace CongestionTaxCalculator.Domain.Entities;

public class PublicHoliday
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int CityId { get; set; }
    public City City { get; set; } = null!;
}
