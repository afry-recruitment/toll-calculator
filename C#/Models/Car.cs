using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Models
{
    public class Car : Vehicle
    {
        public string GetVehicleType()
        {
            return "Car";
        }
    }
}