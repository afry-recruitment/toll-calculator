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
            DateTime intervalStart = dates[0];
            int totalFee = 0;
            foreach (DateTime date in dates)
            {
                int nextFee = GetTollFee(date, vehicle);
                int tempFee = GetTollFee(intervalStart, vehicle);

                long diffInMillies = date.Millisecond - intervalStart.Millisecond;
                long minutes = diffInMillies / 1000 / 60;

                if (minutes <= 60)
                {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    totalFee += nextFee;
                }
            }
            if (totalFee > 60) totalFee = 60;
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