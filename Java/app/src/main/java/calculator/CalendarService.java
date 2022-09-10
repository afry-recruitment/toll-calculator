package calculator;

import java.time.ZoneId;
import java.time.ZonedDateTime;
import java.util.List;

public class CalendarService
{
    List<ZonedDateTime> holidays;

    public CalendarService()
    {
        CalenderHandler calenderHandler = getCalenderHandler();
        ZoneId swedenTimeZone = ZoneId.of("Europe/Stockholm");
        this.holidays = calenderHandler.getHolidays(swedenTimeZone);
    }

    static CalenderHandler getCalenderHandler()
    {
        return new GoogleCalendarHandler();
    }
}
