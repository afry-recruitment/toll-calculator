using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Motorbike : Vehicle
    {
        public int GetVehicleType()
        {
           return (int)VehicleType.Motorbike;
        }
    }
}
