using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator.interfaces;

namespace TollFeeCalculator.Models
{
    public class Motorbike : Vehicle
    {
        public string GetVehicleType()
        {
            return "Motorbike";
        }
    }
}
