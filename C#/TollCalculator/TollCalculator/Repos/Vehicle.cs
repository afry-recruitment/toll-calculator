using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.Interface;

namespace TollCalculator
{
	 public abstract class Vehicle : IVehicle
	{
		public abstract string GetName();
		public abstract bool IsTollFree();
		public override string ToString()
		{
			return GetName();
		}
	}
}
