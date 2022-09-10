package calculator;

import java.time.ZoneId;
import java.time.ZonedDateTime;
import java.util.List;

public interface CalenderHandler
{
    List<ZonedDateTime> getHolidays(ZoneId zoneId);
}
