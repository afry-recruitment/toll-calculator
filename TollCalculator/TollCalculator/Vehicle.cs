using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Vehicle
    {
        public  string RegNumber { get; set; }
        public VehicleType Type { get; set; }

        public Vehicle(string regNumber, VehicleType type)
        {
            RegNumber = regNumber;
            Type = type;
        }
    }
}
