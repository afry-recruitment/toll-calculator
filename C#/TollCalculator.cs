using System;
using System.Globalization;
using TollFeeCalculator;
using Nager.Date;


namespace TollFeeCalculator
{
    public class TollCalculator
    {

        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */

        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            // Handle edge cases
            if (dates == null || dates.Length == 0)
                return 0;

            // Sort dates to ensure chronological order
            Array.Sort(dates);

            int totalFee = 0;
            DateTime windowStart = dates[0];  // Start of current 60-minute window
            int windowMaxFee = GetTollFee(dates[0], vehicle);  // Highest fee in current window

            for (int i = 1; i < dates.Length; i++)
            {
                DateTime currentPass = dates[i];
                int currentFee = GetTollFee(currentPass, vehicle);

                // Calculate time difference from the start of the current window
                TimeSpan timeDiff = currentPass - windowStart;
                long minutes = (long)timeDiff.TotalMinutes;

                if (minutes <= 60)
                {
                    // Still within the same 60-minute window
                    // Keep track of the highest fee in this window
                    if (currentFee > windowMaxFee)
                    {
                        windowMaxFee = currentFee;
                    }
                }
                else
                {
                    // More than 60 minutes have passed - close current window and start new one
                    
                    // Add the highest fee from the completed window to total
                    totalFee += windowMaxFee;

                    // Start a new window at this pass
                    windowStart = currentPass;
                    windowMaxFee = currentFee;
                }
            }

            // Add the fee from the last window (which hasn't been added yet)
            totalFee += windowMaxFee;

            // Cap the daily maximum at 60 SEK
            if (totalFee > 60)
                totalFee = 60;

            return totalFee;
        }

        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null) return false;
            VehicleType vehicleType = vehicle.GetVehicleType();
            return vehicleType == VehicleType.Motorbike ||
                   vehicleType == VehicleType.Tractor ||
                   vehicleType == VehicleType.Emergency ||
                   vehicleType == VehicleType.Diplomat ||
                   vehicleType == VehicleType.Foreign ||
                   vehicleType == VehicleType.Military;
        }

        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18; // Morning rush hour peak
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 8 && minute >= 30 && minute <= 59) return 8;
            else if (hour >= 9 && hour <= 14) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if ((hour == 15 && minute >= 30) || (hour == 16 && minute <= 59)) return 18; // Afternoon rush hour peak
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        private Boolean IsTollFreeDate(DateTime date)
        {
            // Weekends are toll-free
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) 
                return true;

            // July is toll-free
            if (date.Month == 7) 
                return true;

            // Check if it's a Swedish public holiday
            if (DateSystem.IsPublicHoliday(date, CountryCode.SE))
                return true;

            return false;
        }

    }
}