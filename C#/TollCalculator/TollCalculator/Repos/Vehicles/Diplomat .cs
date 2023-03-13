using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Repos
{
	public class Diplomat : Vehicle
	{
		public override string GetName()
		{
			return "Diplomat";
		}

		public override bool IsTollFree()
		{
			return true;
		}
	}
}
