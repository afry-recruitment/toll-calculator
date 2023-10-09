using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toll_Fee_Calculator
{
    public abstract class Vehicle
    {
        public DateTime[] Dates { get; set; }
        public Dictionary<DateTime, int> TotalTollFee { get; set; }
        public bool IsTollFree { get; set; }

        public Vehicle(DateTime[] dates, bool isTollFree)
        {
            Dates = dates;
            IsTollFree = isTollFree;
        }

        public abstract String GetVehicleType();
    }
}