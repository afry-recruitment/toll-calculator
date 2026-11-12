using TollCalculator.Models;
using TollCalculator.Services.Policies;

var calculator = new TollCalculator.Services.TollCalculator(
    new DefaultTollFeeSchedule(),
    new DefaultTollFreeVehiclePolicy(),
    new HolidayPolicy2013());

IVehicle vehicle = new Car();

var passes = new[]
{
    new DateTime(2013, 1, 2, 6, 10, 0),
    new DateTime(2013, 1, 2, 6, 40, 0),
    new DateTime(2013, 1, 2, 7, 20, 0),
    new DateTime(2013, 1, 2, 15, 45, 0),
    new DateTime(2013, 1, 2, 17, 10, 0)
};

var totalFee = calculator.GetTollFee(vehicle, passes);

Console.WriteLine("Toll Calculator Demo");
Console.WriteLine($"Vehicle: {vehicle.VehicleType}");
Console.WriteLine($"Passes: {passes.Length}");
Console.WriteLine("Cost per passing:");

foreach (var pass in passes.OrderBy(p => p))
{
    var passFee = calculator.GetSinglePassFee(pass, vehicle);
    Console.WriteLine($"- {pass:yyyy-MM-dd HH:mm}: {passFee} SEK");
}

Console.WriteLine($"Total fee for the day: {totalFee} SEK");
