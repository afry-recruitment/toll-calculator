package calculator.calendar;

import java.time.LocalDate;

public interface CalendarService
{
    boolean isWeekend(LocalDate date);
    boolean isHoliday(LocalDate date);
}
