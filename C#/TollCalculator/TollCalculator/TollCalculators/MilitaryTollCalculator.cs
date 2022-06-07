using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class MilitaryTollCalculator : TollCalculatorBase, IVehicleTollCalculator
    {
        private readonly Military military;
        public MilitaryTollCalculator(Military military) :
            base(military)
        {
            this.military = military;
        }
    }
}
