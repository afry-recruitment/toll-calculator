using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
 
    public class Car : Vehicle
    {
        // Noggranhet: Stort "S" till litet "s" på string.
        public string GetVehicleType()
        {
            return "Car";
        }
    }
}