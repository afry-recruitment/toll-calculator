using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.TollCalculator
{
    public class DefaultHolidayCalendar : IHolidayCalendar
    {
        private List<DateTime> _holidayList;
        public List<DateTime> HolidaysList { get => GetHolidays(); }

        public DefaultHolidayCalendar()
        {
            _holidayList = new List<DateTime>();
            _holidayList.Add(new DateTime(2022, 7, 1));
            _holidayList.Add(new DateTime(2022, 7, 4));
            _holidayList.Add(new DateTime(2022, 7, 5));
        }

        private List<DateTime> GetHolidays()
        {
            return _holidayList;
        }




    }
}
