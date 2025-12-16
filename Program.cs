
using tollFeeCalculator.Models;
using tollFeeCalculator.Services;

class Program
{
    static void Main(string[] args)
    {
        TollCalculator tollCalculator = new TollCalculator();

        Car car = new Car();
        Motorbike motorbike = new Motorbike();

        var datesZeroSum = new[]
        {
            new DateTime(2025, 12, 13, 06, 00, 00),
            new DateTime(2025, 12, 14, 07, 00, 00),
            new DateTime(2025, 12, 16, 05, 00, 00),
        };

        var datesSeveralEntriesOneHour = new[]
        {
            new DateTime(2025, 12, 16, 06, 00, 00),
            new DateTime(2025, 12, 16, 06, 30, 00),
        };

        var datesMaxFee = new[]
        {
            new DateTime(2025, 12, 16, 06, 00, 00),
            new DateTime(2025, 12, 16, 07, 00, 00),
            new DateTime(2025, 12, 16, 08, 00, 00),
            new DateTime(2025, 12, 16, 09, 00, 00),
            new DateTime(2025, 12, 16, 10, 00, 00),
            new DateTime(2025, 12, 16, 11, 00, 00),
            new DateTime(2025, 12, 16, 12, 00, 00),

        };

        int zeroSumFee = tollCalculator.GetTollFee(car, datesZeroSum);
        int higherHourFee = tollCalculator.GetTollFee(car, datesSeveralEntriesOneHour);
        int maxFee = tollCalculator.GetTollFee(car, datesMaxFee);
        int noFeeMotorbike = tollCalculator.GetTollFee(motorbike, datesSeveralEntriesOneHour);

        Console.WriteLine($"Fee should be 0: {zeroSumFee}");
        Console.WriteLine($"Fee should be 13: {higherHourFee}");
        Console.WriteLine($"Fee should be 60: {maxFee}");
        Console.WriteLine($"Fee should be 0: {noFeeMotorbike}");
    }
}
