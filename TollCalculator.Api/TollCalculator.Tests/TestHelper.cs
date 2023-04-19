using Application.Interfaces.GetTollFee;
using Microsoft.EntityFrameworkCore;
using TollCalculator.Domain.Entities;
using TollCalculator.Infrastructure.Context;
using TollCalculator.Infrastructure.Repositories;

namespace Library.UnitTests
{
    public class TestHelper
    {
        private readonly InMemoryDbContext dbContext;
        public TestHelper()
        {
            var builder = new DbContextOptionsBuilder<InMemoryDbContext>();
            builder.UseInMemoryDatabase(databaseName: "TestingDb");
            var dbContextOptions = builder.Options;

            dbContext = new InMemoryDbContext(dbContextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            AddTestData(dbContext);
        }

        public IGetTollFeeRepository TestingInMemoryRepo()
        {
            return new GetTollFeeRepository(dbContext);
        }

        public IEnumerable<TollFreeVehiclesEntity> GetMockTollFreeVehicles()
        {
            return new List<TollFreeVehiclesEntity>()
            {
                { new TollFreeVehiclesEntity(){ Id = 1, Vehicle = "Military" }}
            };
        }
        public void DeleteTollFreeDays()
        {
            dbContext.TollFreeDays.RemoveRange(dbContext.TollFreeDays);

            dbContext.SaveChangesAsync();
        }
        public List<TollRatesEntity> GetTollRates()
        {
           return dbContext.TollRates.ToList();
        }
        public static void AddTestData(InMemoryDbContext dbContext)
        {
            dbContext.Add(new TollFreeVehiclesEntity { Id = 1, Vehicle = "Military" });

            dbContext.AddRange(
                new TollRatesEntity { Id = 1, StartTime = 21600, EndTime = 23399, Fee = 8 },
                new TollRatesEntity { Id = 2, StartTime = 23400, EndTime = 25199, Fee = 13 },
                new TollRatesEntity { Id = 3, StartTime = 25200, EndTime = 28799, Fee = 18 },
                new TollRatesEntity { Id = 4, StartTime = 28800, EndTime = 30599, Fee = 13 },
                new TollRatesEntity { Id = 5, StartTime = 30600, EndTime = 53999, Fee = 8 },
                new TollRatesEntity { Id = 6, StartTime = 54000, EndTime = 55799, Fee = 13 },
                new TollRatesEntity { Id = 7, StartTime = 55800, EndTime = 61199, Fee = 18 },
                new TollRatesEntity { Id = 8, StartTime = 61200, EndTime = 64799, Fee = 13 },
                new TollRatesEntity { Id = 9, StartTime = 64800, EndTime = 66599, Fee = 8 });

            dbContext.AddRange(
                new TollFreeDaysEntity { Id = 1, Date = "2013/01/01" },
                new TollFreeDaysEntity { Id = 2, Date = DateTime.Now.ToString("yyyy/MM/dd") });

            dbContext.SaveChangesAsync();
        }
        public static int ConvertTimeStringToInt(string timeString)
        {
            String[] timeParts = timeString.Split(":");

            int hrs = Int32.Parse(timeParts[0]);
            int min = Int32.Parse(timeParts[1]);
            int sec = Int32.Parse(timeParts[2]);

            int secondsFromMidnight = (hrs * 3600) + (min * 60) + sec;

            return secondsFromMidnight;
        }
    }
}