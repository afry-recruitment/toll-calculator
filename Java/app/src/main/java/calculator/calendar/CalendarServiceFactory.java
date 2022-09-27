package calculator.calendar;

public class CalendarServiceFactory
{
    private CalendarService getCalendarService(CalendarRegion calendarRegion)
    {
        return new CalendarServiceImpl(calendarRegion);
    }
}