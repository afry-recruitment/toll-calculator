using TollCalculator.Models;
using TollCalculator.Controller;
internal class Program
{
    private static void Main(string[] args)
    {
        var dates = new List<DateTime> { DateTime.Now, new DateTime(2022, 09, 23, 06, 25, 20), new DateTime(2022, 09, 23, 06, 35, 20) };
        var vehicle = new Car();

        var result = new FeeCalculator().GetTollFee(vehicle, dates);

        Console.WriteLine(result);
    }

}