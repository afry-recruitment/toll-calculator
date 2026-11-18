using System;
using System.Collections.Generic;
using TollFeeCalculator;

public static class Program
{
    public static void Main()
    {
        var calculator = new TollCalculator();
        var database = new FakeTollDatabase();
        var car = new Car("ABC123");
        var motorbike = new Motorbike("MBK001");

        // Wednesday: 06:20 and 06:45 share an hour (13 SEK), 07:30 starts a new
        // hour at rush-hour rate (18 SEK), 08:10 joins that hour, and 16:30 is another rush hour (18 SEK).
        DateTime[] weekdayPassages =
        {
            new DateTime(2026, 11, 18, 6, 20, 0),
            new DateTime(2026, 11, 18, 6, 45, 0),
            new DateTime(2026, 11, 18, 7, 30, 0),
            new DateTime(2026, 11, 18, 8, 10, 0),
            new DateTime(2026, 11, 18, 16, 30, 0),
        };
        Save(database, calculator, car, weekdayPassages, "Car, weekday");

        // Saturday: weekends are fee-free.
        DateTime[] weekendPassages =
        {
            new DateTime(2026, 11, 21, 7, 30, 0),
            new DateTime(2026, 11, 21, 16, 30, 0),
        };
        Save(database, calculator, car, weekendPassages, "Car, Saturday");

        // Motorbikes are toll-free.
        Save(database, calculator, motorbike, weekdayPassages, "Motorbike, weekday");
    }

    private static void Save(FakeTollDatabase database, TollCalculator calculator, Vehicle vehicle, DateTime[] passages, string scenario)
    {
        int totalFee = calculator.GetTollFee(vehicle, passages);
        database.Write(vehicle.LicensePlate, passages[0].Date, totalFee);
        Console.WriteLine($"{scenario}: {totalFee} SEK");
    }
}

// Simulates a database by keeping records in memory.
public class FakeTollDatabase
{
    private readonly List<TollRecord> records = new List<TollRecord>();

    public void Write(string licensePlate, DateTime date, int totalFee)
    {
        var record = new TollRecord(licensePlate, date, totalFee);
        records.Add(record);
        Console.WriteLine($"[DB] Wrote {record.LicensePlate}, {record.Date:yyyy-MM-dd}, {record.TotalFee} SEK");
    }

    private sealed record TollRecord(string LicensePlate, DateTime Date, int TotalFee);
}
