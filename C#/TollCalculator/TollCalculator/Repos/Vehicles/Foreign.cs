using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Repos
{
	class Foreign : Vehicle
	{
		public override string GetName()
		{
			return "Foreign";
		}

		public override bool IsTollFree()
		{
			return true;
		}
	}
}
