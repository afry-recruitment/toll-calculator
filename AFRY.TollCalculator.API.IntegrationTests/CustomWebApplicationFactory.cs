using AFRY.TollCalculator.API.Domain.Entities;
using AFRY.TollCalculator.API.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFRY.TollCalculator.API.IntegrationTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<TollCalculatorDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<TollCalculatorDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TollCalculatorDbContext>();
                db.Database.EnsureCreated();

                SeedDatabase(db);
            }
        });
    }

    private void SeedDatabase(TollCalculatorDbContext context)
    {
        context.TollFreeDates.RemoveRange(context.TollFreeDates);
        context.TollFeePeriods.RemoveRange(context.TollFeePeriods);

        context.SaveChanges();

        context.TollFreeDates.AddRange(
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

        context.TollFeePeriods.AddRange(
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

        context.SaveChanges();
    }
}
