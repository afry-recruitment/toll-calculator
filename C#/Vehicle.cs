using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Vehicle
    {
        
        public VehicleType Type { get; }

        // Constructor forcing every vehicle to have a valid VehicleType
        public Vehicle(VehicleType type)
        {
            Type = type;
        }
    }
}