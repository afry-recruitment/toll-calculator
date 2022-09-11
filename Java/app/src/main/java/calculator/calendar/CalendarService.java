package calculator.calendar;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

public class CalendarService
{
    private List<LocalDate> holidays;

    public CalendarService(CalendarRegion calendarRegion)
    {
        // todo should not check more than once a week maybe
        CalenderHandler calenderHandler = getCalenderHandler();
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

    public boolean isHoliday(LocalDate date){
       return holidays.stream().anyMatch(h->h.equals(date));
    }

    private static CalenderHandler getCalenderHandler()
    {
        return new GoogleCalendarHandler();
    }
}
