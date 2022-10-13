using System;
using TollFeeCalculator.Models;
using TollFeeCalculator.Utilities;

namespace TollFeeCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            /* USED FOR TESTING */
            // Create all intervals.
            FeeTimeInterval.CreateFeeTimeIntervals();

            // Add toll-free dates for 2022.
            TollFreeDate.SetTollFreeDates2022();

            // Array of passages on 2022-01-04.
            DateTime[] date = { new DateTime(2022, 1, 4, 6, 29, 05), new DateTime(2022, 1, 4, 9, 29, 05), new DateTime(2022, 1, 4, 11, 29, 05), new DateTime(2022, 1, 4, 13, 29, 05), new DateTime(2022, 1, 4, 13, 37, 43), new DateTime(2022, 1, 4, 14, 29, 04), new DateTime(2022, 1, 4, 14, 29, 06), new DateTime(2022, 1, 4, 17, 26, 04), new DateTime(2022, 1, 4, 18, 29, 04), new DateTime(2022, 1, 4, 19, 35, 04) };

            // Array of passages on multiple dates.
            DateTime[] multipleDates = { new DateTime(2022, 1, 4, 6, 29, 05), new DateTime(2022, 1, 4, 9, 29, 05), new DateTime(2022, 1, 4, 11, 29, 05), new DateTime(2022, 1, 4, 13, 29, 05), new DateTime(2022, 1, 4, 13, 37, 43), new DateTime(2022, 1, 4, 14, 29, 04), new DateTime(2022, 1, 4, 14, 29, 06), new DateTime(2022, 1, 4, 17, 26, 04), new DateTime(2022, 1, 4, 18, 29, 04), new DateTime(2022, 1, 4, 19, 35, 04), new DateTime(2022, 1, 7, 18, 29, 04), new DateTime(2022, 1, 18, 19, 35, 04), new DateTime(2022, 2, 18, 9, 15, 44) };

            // Create a owner.
            Owner owner = new(OwnerType.Government);

            // Create a vehicle of type "Personal".
            Vehicle personalVehicle = new(VehicleType.Personal, owner);

            // Create a vehicle of type "Foreign".
            Vehicle foreignVehicle = new(VehicleType.Foreign, owner);

            // Convert all passages for personal vehicle from DateTime to TollStationPassages.
            TollPassageConverter.ConvertOldPassages(personalVehicle, dates);

            // Convert all passages for foreign vehicle from DateTime to TollStationPassages.
            TollPassageConverter.ConvertOldPassages(foreignVehicle, dates);

            // Get all passages for all vehicles for specific owner.
            owner.GetPassagesForAllOwnedCars();

            // Get all passages for personal vehicle during a specific month.
            personal.GetTollStationPassagesForMonth(2022, 1);

            // Get all passages for personal vehicle during a specific date.
            personal.GetTollStationPassagesForDate(new DateTime(2022, 1, 4));

            // Calculate fee for specific date. (Original methods)
            var TollCalculator = new TollCalculator();
            TollCalculator.GetTollFee(personalVehicle, date);

            // Get all passages for vehicle. (For use in methods)
            TollStationPassage[] allPersonal = TollStationPassage.AllTollStationPassages.Where(x => x.Vehicle == personal).ToArray();
            TollStationPassage[] allForeign = TollStationPassage.AllTollStationPassages.Where(x => x.Vehicle == foreign).ToArray();

            // Calculate total fee for a specific vehicle. (All TollStationPassages in array)
            Console.WriteLine(TollFeeCalculation.CalculateTotalTollFee(allPersonal));
            Console.WriteLine(TollFeeCalculation.CalculateTotalTollFee(allForeign));

            // Calculate total fee for a specific vehicle. (All dates)
            Console.WriteLine(TollFeeCalculation.CalculateTotalTollFee(personalVehicle));
            Console.WriteLine(TollFeeCalculation.CalculateTotalTollFee(foreignVehicle));

            // Calculate total fee for a specific vehicle. (Specific year)
            Console.WriteLine(TollFeeCalculation.CalculateTotalTollFee(personalVehicle, 2022));
            Console.WriteLine(TollFeeCalculation.CalculateTotalTollFee(foreignVehicle, 2022));

            // Calculate total fee for a specific vehicle. (Specific year and month)
            Console.WriteLine(TollFeeCalculation.CalculateTotalTollFee(personalVehicle, 2022, 1));
            Console.WriteLine(TollFeeCalculation.CalculateTotalTollFee(foreignVehicle, 2022, 1));

            // Calculate toll fee for an array of passages.
            Console.WriteLine(TollFeeCalculation.CalculateTollFeeForDate(allPersonal));
            Console.WriteLine(TollFeeCalculation.CalculateTollFeeForDate(allForeign));

            // Calculate toll fee for a specific vehicle. (TollStationPassages in array)
            Console.WriteLine(TollFeeCalculation.CalculateTollFeeForDate(personal, allPersonal));
            Console.WriteLine(TollFeeCalculation.CalculateTollFeeForDate(foreign, allForeign));

            // Calculate toll fee for a specific vehicle. (passages in array and specific date)
            Console.WriteLine(TollFeeCalculation.CalculateTollFeeForDate(personal, new DateTime(2022, 1, 4)));
            Console.WriteLine(TollFeeCalculation.CalculateTollFeeForDate(foreign, new DateTime(2022, 1, 4)));

            // Calculate toll fee for a specific vehicle. (passages in array)
            Console.WriteLine(TollFeeCalculation.CalculateTollFeeForDate(personal, dates));
            Console.WriteLine(TollFeeCalculation.CalculateTollFeeForDate(foreign, dates));

            // Get all TollStationPassages.
            Console.WriteLine(personal.GetAllTollStationPassages());

            // Get all TollStationPassages for specific period.
            Console.WriteLine(personal.GetTollStationPassagesForPeriod(new DateTime(2020, 1, 5), new DateTime(2022, 1, 16)));
        }
    }
}
