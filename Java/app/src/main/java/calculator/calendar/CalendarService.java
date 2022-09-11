package calculator.calendar;

import calculator.PropertiesAccessor;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

public class CalendarService
{
    private List<LocalDate> holidays;

    public CalendarService(CalendarRegion calendarRegion)
    {
        CalenderHandler calenderHandler = getCalenderHandler();
        // todo should be decoupled
        this.holidays = calenderHandler.getHolidays(calendarRegion);
    }

    /**
     * Returns a copy of holidays from the region which it was instantiated with.
     * @return list holidays as LocalDate
     */
    public List<LocalDate> getHolidays()
    {
        return new ArrayList<>(holidays);
    }

    private static CalenderHandler getCalenderHandler()
    {
        return new GoogleCalendarHandler();
    }
}
