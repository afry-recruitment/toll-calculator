using System;
using System.Collections.Generic;
using TollFeeCalculator;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Toll Fee Calculator!");

        try
        {
            // Prompt user for vehicle type
            Console.WriteLine("Enter vehicle type (Car, Motorbike, Tractor, Emergency, Diplomat, Foreign, Military, Truck):");
            string vehicleTypeInput = Console.ReadLine()?.Trim();

            // Get the appropriate vehicle object
            Vehicle vehicle = GetVehicle(vehicleTypeInput);
            if (vehicle == null)
            {
                Console.WriteLine("Invalid vehicle type entered. Exiting...");
                return;
            }

            // Prompt user for toll pass dates
            Console.WriteLine("Enter toll pass dates and times in the format 'yyyy-MM-dd HH:mm' (comma-separated):");
            string dateInput = Console.ReadLine();
            DateTime[] dates = ParseDates(dateInput);
            if (dates.Length == 0)
            {
                Console.WriteLine("No valid dates provided. Exiting...");
                return;
            }

            // Calculate the toll fee
            var tollCalculator = new TollCalculator();
            int totalFee = tollCalculator.GetTotalTollFee(vehicle, dates);

            // Display the result
            Console.WriteLine($"Total toll fee for a {vehicle.GetVehicleType()}: {totalFee} SEK");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Get the appropriate Vehicle object based on the user's input.
    /// </summary>
    static Vehicle GetVehicle(string vehicleTypeInput)
    {
        return vehicleTypeInput switch
        {
            "Car" => new Car(),
            "Motorbike" => new Motorbike(),
            "Tractor" => new Tractor(),
            "Emergency" => new Emergency(),
            "Diplomat" => new Diplomat(),
            "Foreign" => new Foreign(),
            "Military" => new Military(),
            "Truck" => new Truck(),
            _ => null // Return null for invalid input
        };
    }

    /// <summary>
    /// Parse a comma-separated string of dates into an array of DateTime objects.
    /// </summary>
    static DateTime[] ParseDates(string dateInput)
    {
        var dates = new List<DateTime>();
        if (!string.IsNullOrWhiteSpace(dateInput))
        {
            string[] dateStrings = dateInput.Split(',');
            foreach (string dateString in dateStrings)
            {
                if (DateTime.TryParse(dateString.Trim(), out DateTime parsedDate))
                {
                    dates.Add(parsedDate);
                }
                else
                {
                    Console.WriteLine($"Invalid date format: {dateString}");
                }
            }
        }
        return dates.ToArray();
    }
}
