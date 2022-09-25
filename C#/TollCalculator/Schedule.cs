using System;

namespace TollFeeCalculator
{
    public class Schedule
    {
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }
        public int Toll { get; set; }
    }
}