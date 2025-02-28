namespace TollFeeCalculator.Models.Configuration
{
    internal class TollHour(int startHour, int startMinutes, int endHour, int endMinutes, int fee)
    {
        public TimeSpan StartTime => new TimeSpan(startHour, startMinutes, 00);
        public TimeSpan EndTime => new TimeSpan(endHour, endMinutes, 00);
        public int Fee => fee;
    }
}
