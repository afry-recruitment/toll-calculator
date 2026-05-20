using System;
using TollCalculator;
using TollCalculator.Interfaces;

Console.WriteLine("Toll Calculator Test");

//startar instansen
var calc = new TollCalculator.TollCalculator();

//ej tullfri
var time1 = new DateTime(2026, 1, 2, 10, 30, 0);
int fee1 = calc.GetTollFee(time1, null);  
Console.WriteLine($"Non-exempt at {time1:yyyy-MM-dd HH:mm} -> fee = {fee1}");

//ej tullfri max-pris
var dates = new[]
{
    new DateTime(2026, 9, 10, 6, 30, 0),
    new DateTime(2026, 9, 10, 7, 30, 0),
    new DateTime(2026, 9, 10, 8, 30, 0),
    new DateTime(2026, 9, 10, 9, 30, 0),
    new DateTime(2026, 9, 10, 10, 30, 0),
    new DateTime(2026, 9, 10, 11, 30, 0),
    new DateTime(2026, 9, 10, 12, 30, 0),
    new DateTime(2026, 9, 10, 13, 30, 0),
    new DateTime(2026, 9, 10, 14, 30, 0),
    new DateTime(2026, 9, 10, 15, 30, 0),
    new DateTime(2026, 9, 10, 16, 30, 0),
    new DateTime(2026, 9, 10, 17, 30, 0),
    new DateTime(2026, 9, 10, 18, 30, 0),
    new DateTime(2026, 9, 10, 19, 30, 0),
    new DateTime(2026, 9, 10, 20, 30, 0),
    new DateTime(2026, 9, 10, 21, 30, 0)
};

int totalForDay = calc.GetTollFee(null, dates);
Console.WriteLine($"Total fee for the day with multiple entries -> total fee = {totalForDay}");

//tullfri
var stub = new StubVehicle("Motorbike");
var time2 = new DateTime(2013, 6, 14, 08, 00, 0);
int fee2 = calc.GetTollFee(time2, stub);
Console.WriteLine($"Exempt at {time2:yyyy-MM-dd HH:mm} -> fee = {fee2}");

// Wait for user input so the console stays open
Console.WriteLine("Press Enter to exit...");
Console.ReadLine();

class StubVehicle : TollCalculator.Interfaces.IVehicle
{
    private readonly string _type;
    public StubVehicle(string type) => _type = type;
    public string GetVehicleType() => _type;
}