package calculator.calendar;

import java.time.LocalDate;
import java.util.List;

public interface CalendarFetcher
{
    List<LocalDate> getHolidays(CalendarRegion calendarRegion);
}
