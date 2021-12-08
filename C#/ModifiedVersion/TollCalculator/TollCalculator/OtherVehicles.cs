using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    class OtherVehicles : Vehicle
    {
        private string vehicleType;


        public string VehicleType
        {
            get { return vehicleType;  }
            set {
                if (!string.IsNullOrEmpty(value))
                    this.vehicleType = value;
            }
        }

        public string GetVehicleType()
        {
            return VehicleType;
        }

    }
}
