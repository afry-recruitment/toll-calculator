using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Military : IVehicle
    {
        public string VehicleType { get => nameof(Military); }
        public bool IsTollFree { get => true; }
    }
}
