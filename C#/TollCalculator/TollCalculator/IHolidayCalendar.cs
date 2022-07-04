using System;
using System.Collections.Generic;

namespace TollCalculator.TollCalculator
{
    public interface IHolidayCalendar
    {
        List<DateTime> HolidaysList { get; }
    }
}