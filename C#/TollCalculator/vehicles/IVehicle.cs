using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.TollCalculator;

namespace TollCalculator.vehicles
{
    public interface IVehicle
    {
        public bool IsTollFreeVehicle { get; }
        public string GetVehicleType();
        public ITollCalculator TollCalculator { get; }
    }
}
