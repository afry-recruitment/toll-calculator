using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator;

namespace TollFeeCalculator
{
    public class Car : Vehicle
    {
        public int GetVehicleType()
        {
            return (int)VehicleType.Car;
        }
    }
}