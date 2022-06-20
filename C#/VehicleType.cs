using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    class VehicleType : Vehicle
    {
        private readonly string _vehicle;

        public VehicleType(string vehicle)
        {
            _vehicle = vehicle;
        }

        public bool IsTollFreeVehicles()
        {
            switch (_vehicle)
            {
                case "Car":
                    return false;
                    break;
                case "Emergency":
                    return true;
                    break;
                case "Military":
                    return true;
                    break;
                case "Tractor":
                    return false;
                    break;
                case "Diplomat":
                    return true;
                    break;
                case "Foreign":
                    return true;
                    break;
            }

            return false;
        }
    }
}
