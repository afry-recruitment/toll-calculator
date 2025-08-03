using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Models
{
    public class TollInterval
    {
        public TimeSpan Start { get; set; }
        public TimeSpan Stop { get; set; }
        public int Cost { get; set; }

        public TollInterval(string start, string stop, int cost)
        {
            Start = TimeSpan.Parse(start);
            Stop = TimeSpan.Parse(stop);
            Cost = cost;
        }
    }
}
