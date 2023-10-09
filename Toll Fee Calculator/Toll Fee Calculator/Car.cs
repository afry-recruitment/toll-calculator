using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toll_Fee_Calculator
{
    public class Car : Vehicle
    {

        public Car(DateTime[] dates) : base(dates, false)
        {

        }
        public override String GetVehicleType()
        {
            return "Car";
        }
    }
}