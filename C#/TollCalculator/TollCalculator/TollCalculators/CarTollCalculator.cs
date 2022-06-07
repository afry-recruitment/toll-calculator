using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    public class CarTollCalculator : TollCalculatorBase, IVehicleTollCalculator
    {
        private readonly Car car;
        public CarTollCalculator(Car car) : 
            base(car)
        {
            this.car = car;
        }
    }
}
