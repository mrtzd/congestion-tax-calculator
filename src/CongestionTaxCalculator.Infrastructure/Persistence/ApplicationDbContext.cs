using CongestionTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<City> Cities { get; set; }
    public DbSet<TollFeeRule> TollFeeRules { get; set; }
    public DbSet<TollFreeMonth> TollFreeMonths { get; set; }
    public DbSet<TollFreeWeekday> TollFreeWeekdays { get; set; }
    public DbSet<PublicHoliday> PublicHolidays { get; set; }
    public DbSet<VehicleExemption> VehicleExemptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>(builder =>
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(c => c.Name).IsUnique();

            builder.HasMany(c => c.TollFeeRules).WithOne(r => r.City).HasForeignKey(r => r.CityId);
            builder.HasMany(c => c.TollFreeWeekdays).WithOne(w => w.City).HasForeignKey(w => w.CityId);
            builder.HasMany(c => c.TollFreeMonths).WithOne(m => m.City).HasForeignKey(m => m.CityId);
            builder.HasMany(c => c.PublicHolidays).WithOne(h => h.City).HasForeignKey(h => h.CityId);
            builder.HasMany(c => c.VehicleExemptions).WithOne(e => e.City).HasForeignKey(e => e.CityId);
        });
    }
}
