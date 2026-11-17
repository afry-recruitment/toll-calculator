using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Vehicles
{
    public interface IVehicle
    {
        VehicleType Type { get; }
    }
}