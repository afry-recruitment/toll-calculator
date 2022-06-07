using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class TimeToTollFee
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TollFee { get; set; }
    }
}
