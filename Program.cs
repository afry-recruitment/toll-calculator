using TrafficTollCalculator.Interfaces;
using TrafficTollCalculator.Models;
using TrafficTollCalculator.Services;
using System;
using System.Collections.Generic;

public class Program
{

    static void Main(string[] args)
    {
        var tollCalculatorService = new TollFeesService();

        var vehicles = new List<Vehicle>
    {
    new Vehicle { VehicleType = "Car" },
    new Vehicle { VehicleType = "Motorbike" },
    new Vehicle { VehicleType = "Truck" }
};


    var timestampsPerVehicle = new List<List<DateTime>>
    {
        new List<DateTime>
        {
            new DateTime(2023, 1, 1, 7, 30, 0),
            new DateTime(2023, 3, 28, 15, 15, 0),
            new DateTime(2023, 9, 5, 6, 15, 0),
            new DateTime(2023, 9, 4, 6, 29, 0), // 8
            new DateTime(2023, 9, 4, 6, 30, 0), // 13
            new DateTime(2023, 9, 4, 7, 15, 1), // 18
            new DateTime(2023, 9, 4, 8, 16, 0), // 13
            new DateTime(2023, 9, 5, 8, 30, 0), // 0
            new DateTime(2023, 9, 4, 15, 00, 0), // 18
            new DateTime(2023, 9, 4, 17, 00, 0), // 13
            new DateTime(2023, 9, 4, 18, 0, 0), // 8
            new DateTime(2023, 9, 4, 18, 30, 0), // 0
            new DateTime(2023, 9, 4, 19, 0, 0), // 0
            new DateTime(2023, 9, 4, 2, 0, 0), // 0



        },
        new List<DateTime>
        {
            new DateTime(2023, 9, 5, 12, 0, 0),
            new DateTime(2023, 9, 6, 12, 0, 0),
            new DateTime(2023, 9, 10, 12, 0, 0),
            new DateTime(2023, 9, 4, 12, 0, 0),
            new DateTime(2023, 9, 4, 12, 0, 0),
            new DateTime(2023, 9, 4, 12, 0, 0),
            new DateTime(2023, 9, 4, 12, 0, 0),
            new DateTime(2023, 9, 4, 12, 0, 0),
        },
        new List<DateTime>
        {
            new DateTime(2023, 9, 4, 6, 0, 0), // 8
        }
    };
        var totalFee = 0;

        for (int i = 0; i < Math.Min(vehicles.Count, timestampsPerVehicle.Count); i++)
        {
            var FeeForOneVehicle = tollCalculatorService.GetTollFee(new List<List<DateTime>> { timestampsPerVehicle[i] }, new List<Vehicle> { vehicles[i] });
            Console.WriteLine($"Total toll fee for Vehicle register {i + 1}: {FeeForOneVehicle} SEK");

            totalFee += FeeForOneVehicle;
        }
        Console.WriteLine($"Total toll fee for all Vehicles: {totalFee} SEK");

    }
}