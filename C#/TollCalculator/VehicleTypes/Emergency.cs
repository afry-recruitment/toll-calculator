using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Emergency : IVehicle
    {
        public string VehicleType { get => nameof(Emergency); }
        public bool IsTollFree { get => true; }
    }
}