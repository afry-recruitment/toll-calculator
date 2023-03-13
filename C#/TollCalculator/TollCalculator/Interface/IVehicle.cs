using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Interface
{
	public interface IVehicle
	{
		public string GetName();
		public bool IsTollFree();

	}
}
