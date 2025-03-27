using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TollFeeCalculator.Core;
using TollFeeCalculator.Pricing;
using TollFeeCalculator.Services;
using TollFeeCalculator.Vehicles;


namespace TollFeeCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Dependency Injection
            var serviceProvider = ConfigureServices();

            // Resolve Dependencies
            var tollCalculatorService = serviceProvider.GetRequiredService<TollCalculatorService>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();


            IVehicle vehicle = GetVehicleFromUser(serviceProvider, logger);
            if (vehicle == null)
            {
                logger.LogError("Failed to get valid vehicle type");
                return;
            }

            // Get Current Date and Time
            DateTime[] dates = GetCurrentDateTime();

            // Calculate Toll Fee
            try
            {
                decimal totalFee = tollCalculatorService.GetTollFee(vehicle, dates);
                Console.WriteLine($"Total avgift: {totalFee}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while calculating the toll fee.");
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Configure logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            // Register your services
            services.AddTransient<ITollCalculator, TimeBasedTollCalculator>();
            services.AddTransient<ITollExemptionRulesService, TollExemptionRulesService>();
            services.AddTransient<TollCalculatorService>();

            // Register a factory for creating vehicles
            services.AddTransient<Func<VehicleType, IVehicle>>(serviceProvider => vehicleType =>
            {
                switch (vehicleType)
                {
                    case VehicleType.Car:
                        return new Car();
                    case VehicleType.Motorbike:
                        return new Motorbike();
                    case VehicleType.Tractor:
                        return new Tractor();
                    case VehicleType.Emergency:
                        return new Emergency();
                    case VehicleType.Diplomat:
                        return new Diplomat();
                    case VehicleType.Foreign:
                        return new Foreign();
                    case VehicleType.Military:
                        return new Military();
                    default:
                        throw new ArgumentException($"Invalid vehicle type: {vehicleType}");
                }
            });

            return services.BuildServiceProvider();
        }

        private static IVehicle GetVehicleFromUser(IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            Console.WriteLine("Available Vehicle Types:");
            foreach (VehicleType vehicleType in Enum.GetValues(typeof(VehicleType)))
            {
                Console.WriteLine($"- {(int)vehicleType}: {vehicleType}");
            }

            Console.Write("Enter the number corresponding to the vehicle type: ");
            if (int.TryParse(Console.ReadLine(), out int vehicleTypeNumber) && Enum.IsDefined(typeof(VehicleType), vehicleTypeNumber))
            {
                VehicleType selectedVehicleType = (VehicleType)vehicleTypeNumber;
                var vehicleFactory = serviceProvider.GetService<Func<VehicleType, IVehicle>>();
                if (vehicleFactory != null)
                {
                    return vehicleFactory(selectedVehicleType);
                }
                else
                {
                    logger.LogError("Could not resolve vehicle factory.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Invalid vehicle type selected.");
                return null;
            }
        }

        private static DateTime[] GetCurrentDateTime()
        {
            return [DateTime.Now];
        }
    }
}