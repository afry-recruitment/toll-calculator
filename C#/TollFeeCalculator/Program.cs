using TollFeeCalculator.Models.Contracts;
using TollFeeCalculator.Models.Vehicles;
using TollFeeCalculator.Servcies;

namespace TollFeeCalculator;

class Program
{
    static void Main(string[] args)
    {
        IVehicle vehicle = args.Length > 0 && args[0].ToLower() == "motorbike"
            ? new Motorbike()
            : new Car();
        List<DateTime> tollDates = args[1].Split(',')
            .Select(date => DateTime.Parse(date.Trim()))
            .ToList();
        int totalFee = TollCalculator.GetTollFee(vehicle, [.. tollDates]);
        Console.WriteLine($"Total toll fee for the day: {totalFee}");
    }
}
