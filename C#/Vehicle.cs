using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Vehicle
    {
        //String GetVehicleType();

        public string VehicleType { get; set; }

        public Vehicle(string VehicleType)
        {
            this.VehicleType = VehicleType;
        }

        public string GetVehicleType()
        {
            return VehicleType;
        }
    }
}