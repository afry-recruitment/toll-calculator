using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace TollFeeCalculator
{
    class Program
    {
        private static TollCalculator tollCalculator = new TollCalculator();

        static void Main(string[] args)
        {
            bool appRunning = true;
            while (appRunning)
            {
                Console.WriteLine("\nToll Fee Calculator Main Menu:");
                Console.WriteLine("1. Calculate Toll");
                Console.WriteLine("2. Exit");
                Console.Write("Select an option: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int userChoice) && userChoice >= 1 && userChoice <= 2)
                {
                    switch (userChoice)
                    {
                        case 1:
                            CalculateToll();
                            break;
                        case 2:
                            appRunning = ConfirmExit();
                            break;
                        default:
                            Console.WriteLine("Please select a valid option (1-2).");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 2.");
                }
            }
        }

        private static bool ConfirmExit()
        {
            Console.Write("Are you sure you want to exit? (Y/N): ");
            string exitConfirmation = Console.ReadLine();
            return exitConfirmation.Equals("N", StringComparison.OrdinalIgnoreCase);
        }

        private static void CalculateToll()
        {
            Console.WriteLine("Enter vehicle type (C for Car, MC for Motorbike):");
            ShowVehicleOptions();
            string vehicleTypeInput = Console.ReadLine();

            // registration number
            string regNumber = "ABC123";

            Vehicle vehicle;
            switch (vehicleTypeInput.ToUpper())
            {
                case "C":
                    DrawCarArt();
                    vehicle = new Vehicle(regNumber, VehicleType.Car);
                    break;
                case "MC":
                    DrawMotorbikeArt();
                    vehicle = new Vehicle(regNumber, VehicleType.Motorbike);
                    break;
                default:
                    Console.WriteLine("Invalid vehicle type entered.");
                    return;
            }

            Console.WriteLine("Enter the number of dates for toll calculation:");
            if (int.TryParse(Console.ReadLine(), out int numberOfDates) && numberOfDates > 0)
            {
                DateTime[] dates = new DateTime[numberOfDates];
                for (int i = 0; i < numberOfDates; i++)
                {
                    Console.WriteLine($"Enter date and time for toll passing {i + 1} (format: yyyy-MM-dd HH:mm):");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                    {
                        dates[i] = date;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter the date in the correct format.");
                        return;
                    }
                }

                int fee = tollCalculator.GetTollFee(vehicle, dates);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Toll fee for vehicle {vehicle.Type} is {fee} SEK");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Invalid number of dates entered.");
            }
        }

        private static void ShowVehicleOptions()
        {
            Console.WriteLine("C - Car");
            Console.WriteLine("MC - Motorbike");
        }

        private static void DrawCarArt()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("   ______");
            Console.WriteLine("  /|_||_\\`.__");
            Console.WriteLine(" (   _    _ _\\");
            Console.WriteLine("=`-(_)--(_)-'");
            Console.ResetColor();
        }

        private static void DrawMotorbikeArt()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("      ___o");
            Console.WriteLine("    _`\\ <,_");
            Console.WriteLine("___(*)/ (*)");
            Console.ResetColor();
        }
    }
}
