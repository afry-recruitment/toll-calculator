using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.Repos;

namespace TollCalculator.Data
{
	class PaymentIntervals
	{

		// just to make the datetime creation shorter, 
		private static DateTime MakeDateTime(int hour, int min)
		{
			DateTime dt =  new (1, 1, 1);
			dt = dt.AddHours(hour).AddMinutes(min);
			return dt;
		}
		// Observe, this 
		public static List<PaymentInterval> PIList = new()
		{
			// morning
			new PaymentInterval(8, MakeDateTime(6,0), MakeDateTime(6,29)),
			new PaymentInterval(13, MakeDateTime(6,30), MakeDateTime(6,59)),
			new PaymentInterval(18, MakeDateTime(7,0), MakeDateTime(7,59)),
			new PaymentInterval(13, MakeDateTime(8,0), MakeDateTime(8,29)),
			// Day
			new PaymentInterval(8, MakeDateTime(8, 30), MakeDateTime(14, 59)),
			// afternoon
			new PaymentInterval(13, MakeDateTime(15, 0), MakeDateTime(15, 29)),
			new PaymentInterval(18, MakeDateTime(15, 30), MakeDateTime(16, 59)),
			new PaymentInterval(13, MakeDateTime(17, 0), MakeDateTime(17, 59)),
			new PaymentInterval(8, MakeDateTime(18, 0), MakeDateTime(18, 59)),
		};
	}
}
