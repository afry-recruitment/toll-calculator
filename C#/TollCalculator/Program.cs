namespace TollFeeCalculator;
class Program
{
    static void Main(string[] args)
    {
        var tollCalculator = new TollCalculator();
        var car = new Car();
        var carToll = TollCalculationHelper.TollChargesPerPass(DateTime.Now);
        Console.WriteLine($"Toll Charges - {carToll}");

        var dates = new DateTime[] { new DateTime(2022, 09, 22, 08, 29, 29), new DateTime(2022, 09, 22, 09, 30, 29), new DateTime(2022, 09, 22, 10, 31, 29),
        new DateTime(2022, 09, 22, 11, 40, 29), new DateTime(2022, 09, 22, 15, 29, 29), new DateTime(2022, 09, 22, 16, 28, 29), new DateTime(2022, 09, 22, 17, 30, 29), new DateTime(2022, 09, 22, 07, 01, 29)};
        var carTotalToll = tollCalculator.GetTollFee(car, dates);
        Console.WriteLine($"Car Toll - {carTotalToll}");
    }
}