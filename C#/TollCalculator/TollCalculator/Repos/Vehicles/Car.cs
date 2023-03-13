using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Repos
{
	public class Car : Vehicle
	{
		public override string GetName()
		{
			return "Car";
		}

		public override bool IsTollFree()
		{
			return false;
		}
	}
}
