using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculatorAfry
{
    public class Car : IVehicle
    {
        public String GetVehicleType()
        {
            return "Car";
        }
    }
}