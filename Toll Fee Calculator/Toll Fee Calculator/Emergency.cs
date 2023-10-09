using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toll_Fee_Calculator
{
    public class Emergency : Vehicle
    {
        public Emergency(DateTime[] dates) : base(dates, true)
        {

        }
        public override string GetVehicleType()
        {
            return "Emergency";
        }
    }
}
