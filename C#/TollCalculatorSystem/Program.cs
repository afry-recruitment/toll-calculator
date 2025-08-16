using TollCalculatorSystem.Models;
using TollCalculatorSystem.Services;

var calculator = new TollCalculator();
var car = new Car();

var passes = new[]
{
    new DateTime(2025, 8, 16, 06, 15, 00),
    new DateTime(2025, 8, 16, 06, 45, 00),
    new DateTime(2025, 8, 16, 07, 30, 00)
};

var fee = calculator.GetTollFee(car, passes);

Console.WriteLine($"Calculated fee: {fee}");