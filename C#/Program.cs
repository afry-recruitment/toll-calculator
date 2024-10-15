using TollFeeCalculator.Models;
using TollFeeCalculator.Services;
using TollFeeCalculator.Services.Interfaces;

namespace TollFeeCalculator;

public class Program
{
    //Gif from hackers (1995) Just for the Bonus point :)
    private static ITollCalculator _tollCalculator;
    private static Dictionary<string, Vehicle> _registeredVehicles;
    private static ITollFreeService _tollFreeService;

    public static void Main(string regNr)
    {
        // Example: the regNr for the passing vehicle
        regNr = "XYZ123";
        _registeredVehicles = new Dictionary<string, Vehicle>();
        _tollCalculator = new TollCalculator();

        // Load toll-free vehicles and dates
        ITollFreeDataService tollFreeDataService = new TollFreeDataService();
        List<string> tollFreeVehicles = tollFreeDataService.LoadTollFreeVehicles("tollFreeVehicles.json");
        List<DateTime> tollFreeDates = tollFreeDataService.LoadTollFreeDates("tollFreeDates.json");

        _tollFreeService = new TollFreeService(tollFreeVehicles, tollFreeDates);

        // Example: Registering a vehicle passage
        RegisterVehiclePassage(regNr, DateTime.Now);
    }

    public static void RegisterVehiclePassage(string regNr, DateTime passageTime)
    {
        if (_tollFreeService.IsTollFreeDate(passageTime))
        {
            Console.WriteLine($"Date {passageTime.ToShortDateString()} is a toll-free date for vehicle {regNr}.");
            return;
        }

        if (_tollFreeService.IsTollFreeVehicle(regNr))
        {
            Console.WriteLine($"Vehicle {regNr} is toll-free.");
            return;
        }

        if (_registeredVehicles.TryGetValue(regNr, out Vehicle vehicle))
        {
            if (!vehicle.IsDailyCapped)
            {
                vehicle.RecordPassage(passageTime);
                _tollCalculator.UpdateDailyToll(vehicle);
            }
        }
        else
        {
            vehicle = new Vehicle(regNr);
            _registeredVehicles[regNr] = vehicle;
            vehicle.RecordPassage(passageTime);
            _tollCalculator.UpdateDailyToll(vehicle);
        }
    }
}
