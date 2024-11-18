using toll_calculator_jakob.Enums;
using toll_calculator_jakob.Models;
using toll_calculator_jakob.Services;

var calculator = new TollCalculatorService();
var car = new Vehicle(VehicleType.Car);

List<DateTime> dates =
        [
            new DateTime(2024, 11, 14, 6, 15, 0), // 8
            new DateTime(2024, 11, 14, 7, 10, 0), // 18
            new DateTime(2024, 11, 14, 7, 14, 0), // 18
            // 18

            new DateTime(2024, 11, 14, 7, 15, 0), // 18
            // 18

            new DateTime(2024, 11, 14, 15, 9, 0), // 13
            new DateTime(2024, 11, 14, 15, 10, 0), // 13
            // 13 

            new DateTime(2024, 11, 14, 18, 10, 0), // 8
            // 8
        ];

int total = calculator.GetTollFeeTotal(car, dates);
// 57

Console.WriteLine(total.ToString());    
Console.ReadLine();