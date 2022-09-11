package calculator.calendar;

import java.time.LocalDate;
import java.util.List;

public interface CalenderHandler
{
    List<LocalDate> getHolidays(CalendarRegion calendarRegion);
}
