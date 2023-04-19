using Microsoft.EntityFrameworkCore;
using TollCalculator.Domain.Entities;

namespace TollCalculator.Infrastructure.Context;

public class InMemoryDbContext : DbContext
{
    public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options)
     : base(options) { }

    public DbSet<TollRatesEntity> TollRates { get; set; }
    public DbSet<TollFreeVehiclesEntity> TollFreeVehicles { get; set; }
    public DbSet<TollFreeDaysEntity> TollFreeDays { get; set; }
    public DbSet<VehicleInformationEntity> VehicleInformation { get; set; }

}
