using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public enum VehicleType
    {
        Car,
        Motorbike,
        Tractor,
        Emergency,
        Diplomat,
        Foreign,
        Military
    }
    public class Vehicle
    {
        public VehicleType Type { get; }

        public Vehicle(VehicleType type)
        {
            Type = type;
        }
    }
}
