using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Serialization;
using TollCalculator.Domain.Entities;

namespace TollCalculator.Infrastructure.Context;

public class AddData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using(var context = new InMemoryDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<InMemoryDbContext>>()))
        {
            context.TollFreeVehicles.AddRange(
                new TollFreeVehiclesEntity { Id = 1, Vehicle = "Motorbike" },
                new TollFreeVehiclesEntity { Id = 2, Vehicle = "Tractor" },
                new TollFreeVehiclesEntity { Id = 3, Vehicle = "Emergency" },
                new TollFreeVehiclesEntity { Id = 4, Vehicle = "Diplomat" },
                new TollFreeVehiclesEntity { Id = 5, Vehicle = "Foreign" },
                new TollFreeVehiclesEntity { Id = 6, Vehicle = "Military" });

            context.TollRates.AddRange(
                new TollRatesEntity { Id = 1, StartTime = 21600, EndTime = 23399, Fee = 8 },
                new TollRatesEntity { Id = 2, StartTime = 23400, EndTime = 25199, Fee = 13 },
                new TollRatesEntity { Id = 3, StartTime = 25200, EndTime = 28799, Fee = 18 },
                new TollRatesEntity { Id = 4, StartTime = 28800, EndTime = 30599, Fee = 13 },
                new TollRatesEntity { Id = 5, StartTime = 30600, EndTime = 53999, Fee = 8 },
                new TollRatesEntity { Id = 6, StartTime = 54000, EndTime = 55799, Fee = 13 },
                new TollRatesEntity { Id = 7, StartTime = 55800, EndTime = 61199, Fee = 18 },
                new TollRatesEntity { Id = 8, StartTime = 61200, EndTime = 64799, Fee = 13 },
                new TollRatesEntity { Id = 9, StartTime = 64800, EndTime = 66599, Fee = 8 });


            context.TollFreeDays.AddRange(
                new TollFreeDaysEntity { Id = 1, Date = "2013/01/01" },
                new TollFreeDaysEntity { Id = 2, Date = "2013/03/28" },
                new TollFreeDaysEntity { Id = 3, Date = "2013/03/29" },
                new TollFreeDaysEntity { Id = 4, Date = "2013/04/01" },
                new TollFreeDaysEntity { Id = 5, Date = "2013/04/30" },
                new TollFreeDaysEntity { Id = 6, Date = "2013/05/01" },
                new TollFreeDaysEntity { Id = 7, Date = "2013/05/08" },
                new TollFreeDaysEntity { Id = 8, Date = "2013/05/09" },
                new TollFreeDaysEntity { Id = 9, Date = "2013/06/05" },
                new TollFreeDaysEntity { Id = 10, Date = "2013/06/06" },
                new TollFreeDaysEntity { Id = 11, Date = "2013/06/21" },
                new TollFreeDaysEntity { Id = 12, Date = "2013/07/05" },
                new TollFreeDaysEntity { Id = 13, Date = "2013/11/01" },
                new TollFreeDaysEntity { Id = 14, Date = "2013/12/24" },
                new TollFreeDaysEntity { Id = 15, Date = "2013/12/25" },
                new TollFreeDaysEntity { Id = 16, Date = "2013/12/26" },
                new TollFreeDaysEntity { Id = 17, Date = "2013/12/31" });

            context.SaveChanges();
        }
    }
}
