using TollFeeCalculator;
using DateTimeExtensions;
using DateTimeExtensions.WorkingDays;

namespace vs_cs
{
    public static class TollCalculator
    {
        /// <summary>
        /// Culture of toll location.
        /// </summary>
        private static readonly string CULTURE = "sv-SE";

        /// <summary>
        /// Max total fee that can be issued in a day.
        /// </summary>
        private static readonly int MAX_FEE_IN_A_DAY = 60;

        /// <summary>
        /// Time that has to pass since last fee before issuing a new.
        /// </summary>
        private static readonly int MINUTES_BEFORE_NEW_FEE = 60;

        /// <summary>
        /// Calculate the total toll fee for a vehicle passing a toll at multiple occasions in a day.
        /// </summary>
        /// <param name="vehicle">Vehicle passing the tolls.</param>
        /// <param name="dates">Date and time of toll passes in a day.</param>
        /// <returns>Total toll fee.</returns>
        public static int GetTotalTollFee(IVehicle vehicle, DateTime[] dates)
        {
            var currentFees = new List<(DateTime dateTime, int amount)>();

            foreach (DateTime date in dates)
            {
                var nextFee = GetTollFee(date, vehicle);

                if (currentFees.Count > 0)
                {
                    var (dateTime, amount) = currentFees.Last();
                    double minutesSinceLastFee = date.Subtract(currentFees.Last().dateTime).TotalMinutes;

                    if (minutesSinceLastFee <= MINUTES_BEFORE_NEW_FEE)
                    {
                        if (nextFee >= amount)
                        {
                            // Replace the lesser fee amount, but keep the time of previous.
                            currentFees.RemoveAt(currentFees.Count - 1);
                            currentFees.Add((dateTime, nextFee));
                        }
                    }
                    else
                    {
                        currentFees.Add((date, nextFee));
                    }
                }
                else
                {
                    currentFees.Add((date, nextFee));
                }
            }

            var totalFee = currentFees.Sum(f => f.amount);

            return totalFee > MAX_FEE_IN_A_DAY ? MAX_FEE_IN_A_DAY : totalFee;
        }

        /// <summary>
        /// Calculate toll fee for vehicle passing a single toll.
        /// </summary>
        /// <param name="dateTime">Date and time when passing toll.</param>
        /// <param name="vehicle">Vehicle passing toll.</param>
        /// <returns>Toll fee.</returns>
        public static int GetTollFee(DateTime dateTime, IVehicle vehicle)
        {
            if (IsTollFreeDate(dateTime) || IsTollFreeVehicle(vehicle)) return 0;

            int hour = dateTime.Hour;
            int minute = dateTime.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        /// <summary>
        /// Check whether the vehice is toll free.
        /// </summary>
        /// <param name="vehicle">Vehicle to check whether toll free.</param>
        /// <returns>True if the vehicle is not tollable, otherwise false.</returns>
        private static bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle is null) return false;

            return !vehicle.IsTollable;
        }

        /// <summary>
        /// Check whether the date is on a holiday or weekend in culture. 
        /// </summary>
        /// <param name="date">Date to check.</param>
        /// <returns>False if date is on a holiday or weekend, otherwise false.</returns>
        private static bool IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }

            if (date.IsHoliday(new WorkingDayCultureInfo(CULTURE)))
            {
                return true;
            }

            return false;
        }
    }
}