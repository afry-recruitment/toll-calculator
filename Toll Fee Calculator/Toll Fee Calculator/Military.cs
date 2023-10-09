using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toll_Fee_Calculator
{
    public class Military : Vehicle
    {
        public Military(DateTime[] dates) : base(dates, true)
        {

        }
        public override string GetVehicleType()
        {
            return "Military";
        }
    }

}
