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
