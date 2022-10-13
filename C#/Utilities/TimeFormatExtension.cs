using System;

namespace TollFeeCalculator.Utilities
{
    public static class TimeFormatExtension
    {
        // Remove nanoseconds, milliseconds and seconds.
        public static DateTime RoundDownDateTime(DateTime time)
        {
            var ticks = time.Ticks;
            return new DateTime(ticks - (ticks % (1000 * 1000 * 10 * 60)));
        }
    }
}