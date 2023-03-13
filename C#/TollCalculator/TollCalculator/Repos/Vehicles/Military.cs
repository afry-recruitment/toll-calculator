using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Repos
{
	class Military : Vehicle
	{
		public override string GetName()
		{
			return "Military";
		}

		public override bool IsTollFree()
		{
			throw new NotImplementedException();
		}
	}
}
