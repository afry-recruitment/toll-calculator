using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toll_Fee_Calculator
{
    public class Holiday
    {
        int Day { get; set; }
        int Month { get; set; }

        public Holiday(int day, int month)
        {
            Day = day;
            Month = month;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Day == (obj as Holiday).Day && Month == (obj as Holiday).Month;
        }
    }
}
