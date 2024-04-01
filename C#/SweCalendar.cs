using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator { 

public class SweCalendar
{
		public bool isHoliday(DateTime date)//2024
		{
            DateTime[] holidays = new DateTime[]
             {
            new DateTime(2024,01,01),
            new DateTime(2024,01,06),
            new DateTime(2024,03,29),
            new DateTime(2024,03,30),
            new DateTime(2024,03,31),
            new DateTime(2024,01,01),
            new DateTime(2024,05,01),
            new DateTime(2024,05,09),
            new DateTime(2024,05,19),
            new DateTime(2024,06,06),
            new DateTime(2024,06,21),
            new DateTime(2024,06,22),
            new DateTime(2024,11,02),
            new DateTime(2024,12,24),
            new DateTime(2024,12,25),
            new DateTime(2024,12,26),
            new DateTime(2024,12,31)
             };

            if (holidays.Contains(date))
                return true;
            else return false;
        }
}

}