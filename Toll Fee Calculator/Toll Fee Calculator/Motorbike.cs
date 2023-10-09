using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toll_Fee_Calculator
{
    public class Motorbike : Vehicle
    {
        public Motorbike(DateTime[] dates) : base(dates, true)
        {

        }
        public override string GetVehicleType()
        {
            return "Motorbike";
        }
    }
}
