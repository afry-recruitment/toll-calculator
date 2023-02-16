using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculatorJon
{
    public class Vehicle
    {
        //The definiton of car includes passenger cars, busses and trucks according to Transport Styrelsen 
        public Boolean Car { get; } = true;
        public Boolean Emergency { get; } = false;
        public Boolean Diplomat { get; } = false;
        public Boolean Military { get; } = false;

        public Vehicle()
        {
            
        }
        public Vehicle(bool car,bool emergency, bool diplomat, bool military)
        {
            Car = car;
            Emergency = emergency;
            Diplomat = diplomat;
            Military = military;

        }
    }
}