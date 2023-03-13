using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Repos
{
	class Emergency : Vehicle
	{
		public override string GetName()
		{
			return "Emergency";
		}

		public override bool IsTollFree()
		{
			return true;
		}
	}
}
