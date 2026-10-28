using TollCalculator.Interfaces;
using TollCalculator.Models;
using TollCalculator.Services;

namespace TollCalculator;

internal class Program
{
    private static readonly TollFeeService Calculator = new();

    static void Main(string[] args)
    {
        Console.WriteLine("Gothenburg Congestion Tax");
        Console.WriteLine("=========================");
        Console.WriteLine();

        // Example journeys covering the main toll rules.
        PrintScenario("Normal journey", new Car(), new[]
        {
            new DateTime(2024, 1, 8, 6, 10, 0),
            new DateTime(2024, 1, 8, 6, 40, 0),
            new DateTime(2024, 1, 8, 7, 15, 0),
            new DateTime(2024, 1, 8, 9, 0, 0),
            new DateTime(2024, 1, 8, 15, 30, 0)
        });

        PrintScenario("Passages within one hour", new Car(), new[]
        {
            new DateTime(2024, 1, 8, 6, 10, 0),
            new DateTime(2024, 1, 8, 6, 40, 0)
        });

        PrintScenario("Daily fee limit", new Car(), new[]
        {
            new DateTime(2024, 1, 8, 7, 0, 0),
            new DateTime(2024, 1, 8, 9, 0, 0),
            new DateTime(2024, 1, 8, 11, 0, 0),
            new DateTime(2024, 1, 8, 13, 0, 0),
            new DateTime(2024, 1, 8, 15, 30, 0),
            new DateTime(2024, 1, 8, 17, 30, 0)
        });

        PrintScenario("Toll-free vehicle", new Motorbike(), new[]
        {
            new DateTime(2024, 1, 8, 7, 0, 0)
        });

        PrintScenario("Weekend", new Car(), new[]
        {
            new DateTime(2024, 1, 6, 7, 0, 0)
        });

        PrintScenario("Holiday", new Car(), new[]
        {
            new DateTime(2013, 7, 15, 7, 0, 0)
        });
    }

    private static void PrintScenario(
        string title,
        Vehicle vehicle,
        DateTime[] passages)
    {
        Console.WriteLine(title);
        Console.WriteLine(new string('-', title.Length));

        // Sort passages for display; the calculator also handles unsorted input.
        foreach (DateTime passage in passages.OrderBy(date => date))
        {
            int fee = Calculator.GetTollFee(passage, vehicle);
            Console.WriteLine($"{passage:HH:mm} - {fee} SEK");
        }

        int totalFee = Calculator.GetTollFee(vehicle, passages);
        Console.WriteLine($"Total: {totalFee} SEK");
        Console.WriteLine();
    }
}
