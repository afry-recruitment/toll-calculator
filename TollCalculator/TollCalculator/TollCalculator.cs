using System;
using TollFeeCalculator;
using TollFeeCalculator.Service;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        private HolidayService _holidayService;
        private TollFeeRateService _tollFeeRateService;

        public TollCalculator()
        {
            _holidayService = new HolidayService();
            _tollFeeRateService = new TollFeeRateService();
        }

        /// <summary>
        /// Calculates the total toll fee for a vehicle given a list of dates and times when the vehicle passes through the toll.
        /// Fees are determined based on the time of day, with consideration for rush hours and toll-free vehicles.
        /// </summary>
        /// <param name="vehicle">The vehicle for which the toll fee is being calculated. The vehicle can be toll-free based on its type.</param>
        /// <param name="dates">An array of DateTime objects representing the dates and times the vehicle passes through the toll.</param>
        /// <returns>The total toll fee for the given vehicle and dates, not exceeding the daily maximum fee.</returns>
        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            // checks for null vehicle or null/empty dates array and handle
            if (vehicle == null || dates == null || dates.Length == 0)
            {
                Console.WriteLine("Vehicle is null or dates array is empty/null. Returning fee of 0.");
                return 0;
            }

            // Sort dates in ascending order
            Array.Sort(dates);

            int totalFee = 0;
            DateTime intervalStart = dates[0];
            int maxHourlyFee = 0;

            Console.WriteLine($"Starting GetTollFee with dates: {string.Join(", ", dates)}");

            foreach (DateTime date in dates)
            {
                int nextFee = GetTollFee(date, vehicle);
                Console.WriteLine($"Date: {date}, Next Fee: {nextFee}");

                double hoursDifference = (date - intervalStart).TotalHours;
                Console.WriteLine($"Hours Difference: {hoursDifference}");

                if (hoursDifference >= 1)
                {
                    totalFee += maxHourlyFee;
                    intervalStart = date;
                    maxHourlyFee = nextFee;
                    Console.WriteLine($"Adding max hourly fee to total fee: {maxHourlyFee}");
                }
                else
                {
                    maxHourlyFee = Math.Max(maxHourlyFee, nextFee);
                    Console.WriteLine($"Updating max hourly fee: {maxHourlyFee}");
                }
            }

            totalFee += maxHourlyFee;
            Console.WriteLine($"Final total fee: {totalFee}");

            if (totalFee > 60) totalFee = 60;
            Console.WriteLine($"Final total fee after cap: {totalFee}");

            return totalFee;
        }

        /// <summary>
        /// Toll-free vehicles.
        /// </summary>
        /// <param name="vehicle">The vehicle to check for toll-free status.</param>
        /// <returns>True if the vehicle is toll-free; otherwise, false.</returns>
        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null) return false;
            VehicleType Type = vehicle.Type;
            return Type.Equals(VehicleType.Motorbike) ||
                        Type.Equals(VehicleType.Tractor) ||
                        Type.Equals(VehicleType.Emergency) ||
                        Type.Equals(VehicleType.Diplomat) ||
                        Type.Equals(VehicleType.Foreign) ||
                        Type.Equals(VehicleType.Military);
        }

        /// <summary>
        /// Calculates the toll fee for a single pass of a vehicle based on the time of day.
        /// Fees vary depending on the time, with higher fees during rush hours and no fees during toll-free hours,
        /// weekends, and public holidays.
        /// </summary>
        /// <param name="date">The date and time of the vehicle's pass.</param>
        /// <param name="vehicle">The vehicle passing through the toll. The vehicle can be toll-free based on its type or the date.</param>
        /// <returns>The toll fee for the single pass</returns>
        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (_holidayService.IsHoliday(date) || IsTollFreeVehicle(vehicle)) return 0;

            var rates = _tollFeeRateService.LoadTollFeeRates();
            var time = date.TimeOfDay; // Get the TimeSpan representing the time of day

            foreach (var rate in rates)
            {
                // Converting the start and end times to TimeSpan
                TimeSpan startTime = TimeSpan.Parse(rate.Start);
                TimeSpan endTime = TimeSpan.Parse(rate.End);

                if (time >= startTime && time <= endTime)
                {
                    Console.WriteLine($"Applying fee {rate.Fee} for time {time} between {startTime} and {endTime}");
                    return rate.Fee;
                }
            }

            Console.WriteLine($"No matching rate found for time {time}");
            return 0; // Default fee if no rate matches
        }
    }
}
