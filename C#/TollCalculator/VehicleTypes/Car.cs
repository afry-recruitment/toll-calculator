using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Car : IVehicle
    {
        public string VehicleType { get => nameof(Car); }
        public bool IsTollFree { get => false; }
    }
}