using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        /// <summary>
        /// Calculate the total toll fees for a timeperiod
        /// </summary>
        /// <param name="vehicle">the vehicle</param>
        /// <param name="passes">date and time of all passes</param>
        /// <returns>total toll fees</returns>
        ///

        private int MaxDailyTollFee = 60;
        public int CalculateTollFee(IVehicle vehicle, DateTime[] passes)
        {
            if (passes == null || passes.Length == 0)
            {
                return 0;
            }

            DateTime firstPassThisHour = passes[0];
            DateTime currentDay = passes[0].Date;
            int totalFee = 0;
            int dayFee = 0;
            int activeFee = 0;

            foreach (DateTime pass in passes)
            {
                int nextFee = GetTollFee(pass, vehicle);

                if (pass.Date != currentDay)
                {
                    totalFee += CalculateDayFee(dayFee, activeFee);
                    dayFee = 0;
                    activeFee = 0;
                    currentDay = pass.Date;
                    firstPassThisHour = pass;
                }

                if ((pass - firstPassThisHour).TotalMinutes > MaxDailyTollFee)
                {
                    dayFee += activeFee;
                    activeFee = nextFee;
                    firstPassThisHour = pass;
                }
                else
                {
                    activeFee = Math.Max(activeFee, nextFee);
                }
            }
            totalFee += CalculateDayFee(dayFee, activeFee);
            return totalFee;
        }
        private int CalculateDayFee(int dayFee, int activeFee)
        {
            return Math.Min(dayFee + activeFee, MaxDailyTollFee);
        }

        private bool TollFreeVehicle(IVehicle vehicle)
        {
            return vehicle.IsTollFreeVehicle;
        }

        private int GetTollFee(DateTime date, IVehicle vehicle)
        {
            TollFreeDate _tollFreeDate = new TollFreeDate(date);
            if (_tollFreeDate.IsTollFree || TollFreeVehicle(vehicle)) return 0;

            TimeSpan time = date.TimeOfDay;

            foreach (TimeInterval timeInterval in _timeIntervals)
            {
                if (timeInterval.IsWithinInterval(time))
                    return timeInterval.Fee;
            }
            return 0;
        }

        private readonly List<TimeInterval> _timeIntervals =
        [
            new TimeInterval(new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 59), 8),
            new TimeInterval(new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 59), 13),
            new TimeInterval(new TimeSpan(7, 0, 0), new TimeSpan(7, 59, 59), 18),
            new TimeInterval(new TimeSpan(8, 0, 0), new TimeSpan(8, 29, 59), 13),
            new TimeInterval(new TimeSpan(8, 30, 0), new TimeSpan(14, 59, 59), 8),
            new TimeInterval(new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 59), 13),
            new TimeInterval(new TimeSpan(15, 30, 0), new TimeSpan(16, 59, 59), 18),
            new TimeInterval(new TimeSpan(17, 0, 0), new TimeSpan(17, 59, 59), 13),
            new TimeInterval(new TimeSpan(18, 0, 0), new TimeSpan(18, 29, 59), 8),
        ];
    }
}