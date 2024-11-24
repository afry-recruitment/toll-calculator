using System.Globalization;
using TollFeeCalculator.Vehicles;

string dateTimePattern = "yyyyMMdd-HHmm";

Console.WriteLine("Welcome to the Toll Calculator!");

var tollCalculator = new TollCalculator();


Vehicle? vehicle = null;
while (vehicle is null)
{
    Console.WriteLine("Enter the vehicle type:");

    var vehicleType = Console.ReadLine();

    vehicle = vehicleType switch
    {
        "Car" => new Car(),
        "Motorbike" => new Motorbike(),
        "Tractor" => new Tractor(),
        "Emergency" => new Emergency(),
        "Diplomat" => new Diplomat(),
        "Foreign" => new Foreign(),
        "Military" => new Military(),
        _ => null
    };
    if (vehicle is null)
    {
        Console.WriteLine("Invalid vehicle type. Please try again.");
    }
}
 

var dates = new List<DateTime>();
while (true)
{
    Console.WriteLine($"Enter the date and time of the pass ({dateTimePattern}) or hit return to finish:");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        break;
    }

    if (!DateTime.TryParseExact(input, dateTimePattern, null, DateTimeStyles.None, out var date)) 
    {
        Console.WriteLine("Invalid date and time. Please try again.");
        continue;
    }

    if (!dates.All(d => d.Year == date.Year && d.Month == date.Month && d.Day == date.Day))
    {
        Console.WriteLine("All passes must be in the same day");
        continue;
    }

    dates.Add(date);

}

var total = tollCalculator.GetTollFee(new Car(), [.. dates]);
Console.WriteLine($"The total toll fee is {total} kr.");