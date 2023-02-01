using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Model
{
    public class Buss : Vehicle
    {
        int weight;
        public override int Weight
        {
            set
            {
                if (value <= 0)
                {
                    weight = 1;
                }
                else
                {
                    weight = value;
                }
            }
        }

        public override bool ExceptionsFromCongestionTax()
        {
            return true ? weight >= 14 : false;
        }
    }
}
