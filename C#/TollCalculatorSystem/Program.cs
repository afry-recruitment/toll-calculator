using TollCalculatorSystem.Models;
using TollCalculatorSystem.Services;

var calculator = new TollCalculator();

var passes = new[]
{
    new DateTime(2025, 8, 15, 15, 0, 0),
    new DateTime(2025, 8, 15, 15, 0, 00),
    new DateTime(2025, 8, 15, 07, 14, 00)
};

var fee = calculator.CalculateTollFee(VehicleType.Car, passes);

Console.WriteLine($"Calculated fee: {fee}");

