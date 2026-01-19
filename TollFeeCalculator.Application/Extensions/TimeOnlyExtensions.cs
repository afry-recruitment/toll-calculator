namespace TollFeeCalculator.Application.Extensions
{
    public static class TimeOnlyExtensions
    {
        public static List<List<TimeOnly>> ChunkByChargeWindow(this IEnumerable<TimeOnly> times, int chargeWindowMinutes = 60)
        {
            var groups = new List<List<TimeOnly>>();

            if (times is null)
            {
                return groups;
            }

            var ordered = times.OrderBy(t => t).ToList();

            if (!ordered.Any())
            {
                return groups;
            }

            var chargeWindow = TimeSpan.FromMinutes(ValidateChargeWindowMinutes(chargeWindowMinutes));
            var windowStart = ordered.First();
            List<TimeOnly> currentWindow = new();

            foreach (var t in ordered)
            {
                TimeSpan diff = t.ToTimeSpan() - windowStart.ToTimeSpan();

                if (diff < chargeWindow)
                {
                    currentWindow.Add(t);
                }
                else
                {
                    groups.Add(currentWindow);

                    windowStart = t;
                    currentWindow = new();
                    currentWindow.Add(windowStart);
                }
            }

            groups.Add(currentWindow);
            return groups;
        }

        private static int ValidateChargeWindowMinutes(int chargeWindowMinutes) => chargeWindowMinutes < 1 ? 60 : chargeWindowMinutes;
    }
}
