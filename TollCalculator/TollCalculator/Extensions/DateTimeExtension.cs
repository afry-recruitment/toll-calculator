namespace TollFeeCalculator.Extensions
{
    internal static class DateTimeExtension
    {
        /// <summary>
        /// Determine if a date is in between to different dates
        /// </summary>
        internal static bool IsInbetween(this DateTime date, DateTime startdate, DateTime endDate)
        {
            if(date.Equals(startdate) || date.Equals(endDate) ||
                date > startdate && date < endDate)
            {
                return true;
            }

            return false;
        }
    }
}
