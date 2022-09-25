using TollCalculator.Models;
using TollCalculator.Controller;
internal class Program
{
    private static void Main(string[] args)
    {
        var date = new DateTime(2022, 09, 23, 06, 25, 20);
        var date1 = new DateTime(2022, 09, 23, 06, 35, 20);
        var dates = new List<DateTime> { DateTime.Now, date, date1 };
        var vehicle = new Car();

        var result = new FeeCalculator().GetTollFee(vehicle, dates);

        Console.WriteLine(result);
    }

}