package calculator.calendar;

public class CalendarServiceFactory
{
    private CalendarServiceFactory()
    {
    }

    public static CalendarService getCalendarService(String calendarRegion)
    {
        return new CalendarServiceImpl(CalendarRegion.valueOf(calendarRegion));
    }
}