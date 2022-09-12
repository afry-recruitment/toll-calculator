package calculator.calendar;

import java.time.LocalDate;

public interface CalendarServiceInterface
{
    boolean isWeekend(LocalDate date);
    boolean isHoliday(LocalDate date);
}
