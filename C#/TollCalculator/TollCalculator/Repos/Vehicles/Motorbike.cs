using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Repos
{
	class Motorbike : Vehicle
	{
		public override string GetName()
		{
			return "Motorbike";
		}

		public override bool IsTollFree()
		{
			return true;
		}
	}
}
