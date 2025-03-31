using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoadToll.Application.Calculator;
using RoadToll.Application.DailyFee;
using RoadToll.Application.DateChecker;
using RoadToll.Application.Models.RoadToll;

namespace RoadToll.Test.Core
{
    public class Startup
    {
        public static IConfigurationRoot LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            var configuration = LoadConfiguration();

            services.AddSingleton<IConfiguration>(configuration);
            services.Configure<RoadTollConfiguration>(configuration.GetSection("RoadTollConfiguration"));
            services.Configure<FeeFreeDates>(configuration.GetSection("FreeDates"));

            services.AddTransient<ITollCalculator, TollCalculator>();
            services.AddTransient<IDailyFeeScheduleCalculator, DailyFeeScheduleCalculator>();
            services.AddTransient<ITollFreeDateChecker, TollFreeDateChecker>();

            return services.BuildServiceProvider();
        }
    }
}