using AFRY.TollCalculator.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AFRY.TollCalculator.API.Persistence;

public class TollCalculatorDbContext(DbContextOptions<TollCalculatorDbContext> option) :DbContext(option)
{
    public DbSet<TollFeePeriod> TollFeePeriods => Set<TollFeePeriod>();

    public DbSet<TollFreeDate> TollFreeDates => Set<TollFreeDate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TollFeePeriod>()
        .Property(p => p.Fee)
        .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<TollFeePeriod>().HasData(
            new TollFeePeriod { Id = 1, StartTime = new TimeSpan(6, 0, 0), EndTime = new TimeSpan(6, 29, 59), Fee = 8 },
            new TollFeePeriod { Id = 2, StartTime = new TimeSpan(6, 30, 0), EndTime = new TimeSpan(6, 59, 59), Fee = 13 },
            new TollFeePeriod { Id = 3, StartTime = new TimeSpan(7, 0, 0), EndTime = new TimeSpan(7, 59, 59), Fee = 18 },
            new TollFeePeriod { Id = 4, StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(8, 29, 59), Fee = 13 },
            new TollFeePeriod { Id = 5, StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(14, 59, 59), Fee = 8 },
            new TollFeePeriod { Id = 6, StartTime = new TimeSpan(15, 0, 0), EndTime = new TimeSpan(15, 29, 59), Fee = 13 },
            new TollFeePeriod { Id = 7, StartTime = new TimeSpan(15, 30, 0), EndTime = new TimeSpan(16, 59, 59), Fee = 18 },
            new TollFeePeriod { Id = 8, StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(17, 59, 59), Fee = 13 },
            new TollFeePeriod { Id = 9, StartTime = new TimeSpan(18, 0, 0), EndTime = new TimeSpan(18, 29, 59), Fee = 8 }
        );

        modelBuilder.Entity<TollFreeDate>().HasData(
            new TollFreeDate { Id = 1, Date = new DateTime(2013, 1, 1), Description = "New Year's Day" },
            new TollFreeDate { Id = 2, Date = new DateTime(2013, 3, 28), Description = "Good Friday" },
            new TollFreeDate { Id = 3, Date = new DateTime(2013, 3, 29), Description = "Easter Monday" },
            new TollFreeDate { Id = 4, Date = new DateTime(2013, 4, 1), Description = "Holiday" },
            new TollFreeDate { Id = 5, Date = new DateTime(2013, 4, 30), Description = "Holiday" },
            new TollFreeDate { Id = 6, Date = new DateTime(2013, 5, 1), Description = "Labor Day" },
            new TollFreeDate { Id = 7, Date = new DateTime(2013, 5, 8), Description = "Holiday" },
            new TollFreeDate { Id = 8, Date = new DateTime(2013, 5, 9), Description = "Holiday" },
            new TollFreeDate { Id = 9, Date = new DateTime(2013, 6, 5), Description = "Holiday" },
            new TollFreeDate { Id = 10, Date = new DateTime(2013, 6, 6), Description = "Holiday" },
            new TollFreeDate { Id = 11, Date = new DateTime(2013, 6, 21), Description = "Holiday" },
            new TollFreeDate { Id = 12, Date = new DateTime(2013, 11, 1), Description = "Holiday" },
            new TollFreeDate { Id = 13, Date = new DateTime(2013, 12, 24), Description = "Christmas Eve" },
            new TollFreeDate { Id = 14, Date = new DateTime(2013, 12, 25), Description = "Christmas Day" },
            new TollFreeDate { Id = 15, Date = new DateTime(2013, 12, 26), Description = "Boxing Day" },
            new TollFreeDate { Id = 16, Date = new DateTime(2013, 12, 31), Description = "New Year's Eve" }
        );
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var currentTime = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = currentTime;
                    entry.Entity.CreatedBy = "User";
                    entry.Entity.ModifiedAt = currentTime;
                    entry.Entity.ModifiedBy = "User";
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = currentTime;
                    entry.Entity.ModifiedBy = "User";
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

}
