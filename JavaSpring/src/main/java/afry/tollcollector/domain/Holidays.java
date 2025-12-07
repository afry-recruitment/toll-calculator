package afry.tollcollector.domain;

import java.util.Map;
import java.util.function.Function;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class Holidays {
  private Map<Integer, HolidaysInYear> holidays;

  public Holidays(HolidaysInYear... holidays) {
    this.holidays = Stream.of(holidays)
        .collect(Collectors.toMap(HolidaysInYear::year, Function.identity()));
  }

  public boolean isHolidayIfKnown(int year, int month, int day) {
    return holidays.getOrDefault(year, HolidaysInYear.EMPTY).isHoliday(month, day);
  }

}
