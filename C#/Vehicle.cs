using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TollFeeCalculator 
{
    public class Vehicle //Changed from interface
    {
        public string VehicleType { get; set; }

        public Vehicle(string type)
        {
            this.VehicleType = type;
        }
        public string GetVehicleType()
        {
            return VehicleType;
        }
    }
}