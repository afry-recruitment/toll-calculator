using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Diplomat : IVehicle
    {
        public string VehicleType { get => nameof(Diplomat); }
        public bool IsTollFree { get => true; }
    }
}
// Motorbike = 0,
// Tractor = 1,
// Emergency = 2,
// Diplomat = 3,
// Foreign = 4,
// Military = 5