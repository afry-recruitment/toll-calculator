using PublicHoliday;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        private const int MaxDailyFee = 60;
        private const int ChargeWindowMinutes = 60;

        private readonly SwedenPublicHoliday _holidays = new SwedenPublicHoliday();

        private static readonly TimeZoneInfo _swedenTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Stockholm");

        // Must be sorted by start time. Before 06:00 and from 18:30 it is free.
        private static readonly FeePeriod[] _feeSchedule =
        {
            new FeePeriod(6, 0, 8),
            new FeePeriod(6, 30, 13),
            new FeePeriod(7, 0, 18),
            new FeePeriod(8, 0, 13),
            new FeePeriod(8, 30, 8),
            new FeePeriod(15, 0, 13),
            new FeePeriod(15, 30, 18),
            new FeePeriod(17, 0, 13),
            new FeePeriod(18, 0, 8),
            new FeePeriod(18, 30, 0)
        };

        private class FeePeriod
        {
            public TimeSpan Start { get; }
            public int Fee { get; }

            public FeePeriod(int hour, int minute, int fee)
            {
                Start = new TimeSpan(hour, minute, 0);
                Fee = fee;
            }
        }

        /// <summary>
        /// Calculate the total toll fee for the given passes. The passes can be in any order
        /// and from several days, each day is capped at 60 SEK.
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        /// <param name="dates">Date and time of all passes</param>
        /// <returns>The total toll fee</returns>
        /// <exception cref="ArgumentNullException">If vehicle or dates is null</exception>
        public int GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            ArgumentNullException.ThrowIfNull(dates);
            ArgumentNullException.ThrowIfNull(vehicle);

            // the 60 SEK limit is per day so we handle one day at a time
            Dictionary<DateTime, List<DateTime>> passesPerDay = new Dictionary<DateTime, List<DateTime>>();
            foreach (DateTime pass in dates)
            {
                DateTime date = ToSwedishTime(pass);
                if (!passesPerDay.ContainsKey(date.Date))
                {
                    passesPerDay[date.Date] = new List<DateTime>();
                }
                passesPerDay[date.Date].Add(date);
            }

            int totalFee = 0;
            foreach (List<DateTime> passes in passesPerDay.Values)
            {
                totalFee += GetTollFeeForOneDay(vehicle, passes);
            }
            return totalFee;
        }

        private int GetTollFeeForOneDay(IVehicle vehicle, List<DateTime> passes)
        {
            passes.Sort();

            // An hour starts with the first pass that costs something and lasts 60 minutes.
            // Only the highest fee in each hour is charged. The next pass after that
            // starts a new hour.
            int dayFee = 0;
            int hourFee = 0;
            DateTime? hourStart = null;

            foreach (DateTime pass in passes)
            {
                int fee = GetTollFee(vehicle, pass);

                // a free pass (for example before 06:00) should not start an hour
                if (fee == 0) continue;

                if (hourStart == null || (pass - hourStart.Value).TotalMinutes > ChargeWindowMinutes)
                {
                    dayFee += hourFee;
                    hourFee = 0;
                    hourStart = pass;
                }

                if (fee > hourFee) hourFee = fee;
            }
            dayFee += hourFee;

            return Math.Min(dayFee, MaxDailyFee);
        }

        private bool IsTollFreeVehicle(IVehicle vehicle)
        {
            VehicleType type = vehicle.GetVehicleType();
            return type == VehicleType.Motorbike ||
                   type == VehicleType.Tractor ||
                   type == VehicleType.Emergency ||
                   type == VehicleType.Diplomat ||
                   type == VehicleType.Foreign ||
                   type == VehicleType.Military;
        }

        /// <summary>
        /// Calculate the toll fee for a single pass. A UTC time is converted to Swedish time first.
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        /// <param name="date">Date and time of the pass</param>
        /// <returns>The fee for the pass, 0 if the vehicle or the day is toll free</returns>
        /// <exception cref="ArgumentNullException">If vehicle is null</exception>
        public int GetTollFee(IVehicle vehicle, DateTime date)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            DateTime swedishTime = ToSwedishTime(date);
            if (IsTollFreeDate(swedishTime) || IsTollFreeVehicle(vehicle)) return 0;

            // the fee is valid from the start time until the next one in the table
            int fee = 0;
            foreach (FeePeriod period in _feeSchedule)
            {
                if (swedishTime.TimeOfDay >= period.Start) fee = period.Fee;
            }
            return fee;
        }

        // UTC times are converted to Swedish time. Any other time is assumed to be Swedish time already.
        private static DateTime ToSwedishTime(DateTime date)
        {
            if (date.Kind == DateTimeKind.Utc) return TimeZoneInfo.ConvertTimeFromUtc(date, _swedenTimeZone);
            return date;
        }

        private bool IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            if (date.Month == 7) return true;

            if (_holidays.IsPublicHoliday(date)) return true;

            // The day before a public holiday is free as well. The package also counts midsummer,
            // christmas and new year's eve as holidays, but the days before those are normal days.
            DateTime tomorrow = date.AddDays(1).Date;
            bool tomorrowIsAnEve = tomorrow == SwedenPublicHoliday.MidsummerEve(tomorrow.Year) ||
                                   tomorrow == SwedenPublicHoliday.ChristmasEve(tomorrow.Year) ||
                                   tomorrow == SwedenPublicHoliday.NewYearsEve(tomorrow.Year);
            // The package does not see All Saints' Day when it falls on 31 October, so check it here
            bool tomorrowIsAHoliday = _holidays.IsPublicHoliday(tomorrow) ||
                                      tomorrow == SwedenPublicHoliday.AllSaintsDay(tomorrow.Year);
            return tomorrowIsAHoliday && !tomorrowIsAnEve;
        }
    }
}
