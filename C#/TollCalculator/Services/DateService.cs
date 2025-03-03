using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.Services
{
    public interface IDateService
    {
        bool IsHoliday(DateTime date);
    }

    public class DateService
    {
        public bool IsHoliday(DateTime date)
        {
            //This is a very simple implementation that only checks if the date is a weekend.
            //Would need to be expanded to include public holidays. An idea is to use a public api.
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
