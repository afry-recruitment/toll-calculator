using PublicHoliday;
using System;
using System.Globalization;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Services
{
    public class TollCalculator
    {
        private const int MaxTollFee = 60;

        private readonly string[] TollFreeTypes = new[]
        {
            "Motorbike",
            "Tractor",
            "Emergency",
            "Diplomat",
            "Foreign",
            "Military"
        };

        private readonly List<TollInterval> TollIntervals = new()
        {
            new TollInterval("06:00", "06:29", 8),
            new TollInterval("06:30", "06:59", 13),
            new TollInterval("07:00", "07:59", 18),
            new TollInterval("08:00", "08:29", 13),
            new TollInterval("08:30", "14:59", 8),
            new TollInterval("15:00", "15:29", 13),
            new TollInterval("15:30", "16:59", 18),
            new TollInterval("17:00", "17:59", 13),
            new TollInterval("18:00", "18:29", 8),
        };


        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */
        public int GetTotalTollFee(Vehicle vehicle, DateTime[] dates)
        {
            if (dates == null || dates.Length == 0)
                return 0;

            if (dates.Length == 1)
                return GetTollFee(dates[0], vehicle);

            return CalculateTotalFee(vehicle, dates);
        }

        private int CalculateTotalFee(Vehicle vehicle, DateTime[] dates)
        {
            DateTime[] sortedDates = dates.OrderBy(d => d).ToArray();
            DateTime intervalStart = sortedDates[0];
            int totalFee = 0;
            int currentFee = GetTollFee(intervalStart, vehicle);

            foreach (DateTime date in sortedDates.Skip(1))
            {
                int fee = GetTollFee(date, vehicle);

                if (IsWithinSameHour(intervalStart, date))
                {
                    currentFee = Math.Max(currentFee, fee);
                }
                else
                {
                    totalFee += currentFee;
                    intervalStart = date;
                    currentFee = fee;
                }
            }

            totalFee += currentFee;

            return FinalizeTotalFee(totalFee);
        }

        private bool IsWithinSameHour(DateTime intervalStart, DateTime date)
        {
            TimeSpan diff = date - intervalStart;

            return diff.TotalMinutes <= 60;
        }

        private int FinalizeTotalFee(int fee)
        {
            return Math.Min(fee, MaxTollFee);
        }

        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
                return false;

            string vehicleType = vehicle.GetVehicleType();

            return TollFreeTypes.Contains(vehicleType);
        }

        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
                return 0;

            return CalculateTollFee(date);
        }

        private int CalculateTollFee(DateTime date)
        {
            var time = date.TimeOfDay;

            foreach (var interval in TollIntervals)
            {
                if (IsWithinInterval(time, interval.Start, interval.Stop))
                    return interval.Cost;
            }

            return 0;
        }

        private Boolean IsTollFreeDate(DateTime date)
        {
            return new SwedenPublicHoliday().IsPublicHoliday(date);
        }

        private bool IsWithinInterval(TimeSpan time, TimeSpan start, TimeSpan stop)
        {
            return time >= start && time <= stop;
        }
    }
}
