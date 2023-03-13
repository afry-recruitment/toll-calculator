using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Repos
{
	class Tractor : Vehicle
	{
		public override string GetName()
		{
			return "Tractor";
		}

		public override bool IsTollFree()
		{
			throw new NotImplementedException();
		}
	}
}
