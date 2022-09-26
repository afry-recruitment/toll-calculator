namespace TollFeeCalculator
{
    public class TollCalculator : ITollCalculator
    {
        private readonly DateOnly[] _holidays;

        /// <summary>
        /// Create a new TollCalculator with a list of holidays that are exempt from toll.
        /// </summary>
        /// <param name="holidays">Array of date of holidays that are exempt from toll.</param>
        public TollCalculator(DateOnly[] holidays)
        {
            _holidays = holidays ?? new DateOnly[0];
        }
        /// <summary>
        /// Create a new TollCalculator without specifying holidays that are exempt from toll.
        /// </summary>
        public TollCalculator()
        : this(new DateOnly[0])
        {
        }

        /// <summary>
        /// Calculate the total toll fee for one day.
        /// </summary>
        /// <returns>The total toll fee for that day</returns>
        /// <param name="vehicle">The vehicle</param>
        /// <param name="date">The date</param>
        /// <param name="times">Times (local time) of all passes on one day</param>

        public int GetTotalTollFee(IVehicle vehicle, DateOnly date, TimeOnly[] times)
        {
            if (vehicle is null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }

            if (times is null)
            {
                throw new ArgumentNullException(nameof(times));
            }

            if (times.Length == 0)
                return 0;

            int totalFee = 0;

            var sortedTimes = times.OrderBy(t => t).ToArray();
            var windowStart = sortedTimes[0];
            int currentWindowFee = 0;

            foreach (TimeOnly time in sortedTimes)
            {
                var elapsed = time - windowStart;
                if (elapsed > TimeSpan.FromHours(1))
                {
                    totalFee += currentWindowFee;
                    windowStart = time;
                }
                var feeForPassage = GetTollFee(GetDateTime(date, time), vehicle);
                currentWindowFee = Math.Max(feeForPassage, currentWindowFee);
            }
            var lastTime = sortedTimes.Last();
            currentWindowFee = GetTollFee(GetDateTime(date, lastTime), vehicle);
            totalFee += currentWindowFee;
            return Math.Min(totalFee, 60);
        }

        /// <summary>
        /// Calculate the toll fee for one pass.
        /// </summary>
        /// <returns>The toll fee for the pass.</returns>
        /// <param name="vehicle">The vehicle</param>
        /// <param name="date">The date, must be of DateTimeKind Local.</param>
        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            if (vehicle is null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }
            CheckDate(date);

            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
                return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29)
                return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59)
                return 13;
            else if (hour == 7)
                return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29)
                return 13;
            else if (hour == 8 && minute <= 30)
                return 8;
            else if (hour >= 9 && hour <= 14)
                return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29)
                return 13;
            else if (hour == 15 && minute >= 30)
                return 18;
            else if (hour == 16)
                return 18;
            else if (hour == 17)
                return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29)
                return 8;
            else
                return 0;
        }

        private DateTime GetDateTime(DateOnly date, TimeOnly time)
        {
            return DateTime.SpecifyKind(date.ToDateTime(time), DateTimeKind.Local);
        }

        private void CheckDate(DateTime date)
        {
            if (date.Kind != DateTimeKind.Local)
            {
                throw new InvalidOperationException("Only local DateTimes supported");
            }
        }

        private bool IsTollFreeVehicle(IVehicle vehicle)
        {
            return vehicle?.IsTollFree ?? false;
        }
        private Boolean IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return true;

            var datePart = DateOnly.FromDateTime(date);
            if (_holidays.Count(d => d == datePart) > 0)
                return true;

            return false;
        }
    }
}