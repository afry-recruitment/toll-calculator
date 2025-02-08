using TollFeeCalculator.Application.Interfaces;
using TollFeeCalculator.Application.Services;
using TollFeeCalculator.Domain.Entities;
using TollFeeCalculator.Domain.Enums;
using TollFeeCalculator.Infrastructure.Interfaces;
using TollFeeCalculator.Infrastructure.Repositories;

ITollRepository tollRepository = new TollRepository();
ITollFeeService tollFeeService = new TollFeeService(tollRepository);
var car = new Vehicle(VehicleType.Car);
var motorbike = new Vehicle(VehicleType.Motorbike);
var diplomat = new Vehicle(VehicleType.Diplomat);
DateTime[] passageTimes = {
            new(2024, 11, 14, 6, 15, 0), // 8
            new(2024, 11, 14, 7, 10, 0), // 18
            new(2024, 11, 14, 7, 14, 0), // 18 => total 59min
            // 18

            new(2024, 11, 14, 7, 15, 0), // 18
            // 18

            new(2024, 11, 14, 15, 9, 0), // 13
            new(2024, 11, 14, 15, 10, 0), // 13
            // 13 

            new(2024, 11, 14, 18, 10, 0), // 8
            // 8
            };

//result should be 18+18+13+8 = 57
int totalFee = tollFeeService.CalculateTollFee(car, passageTimes);
int totalFee2 = tollFeeService.CalculateTollFee(motorbike, passageTimes);
int totalFee3 = tollFeeService.CalculateTollFee(diplomat, passageTimes);
Console.WriteLine($"Total toll fee for the day: {totalFee} SEK");
Console.WriteLine($"Total toll fee for the day: {totalFee2} SEK");
Console.WriteLine($"Total toll fee for the day: {totalFee3} SEK");