using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Motorbike : IVehicle
    {
        public string VehicleType { get => "Motorbike"; }
        public bool IsTollFree { get => true; }
    }
}