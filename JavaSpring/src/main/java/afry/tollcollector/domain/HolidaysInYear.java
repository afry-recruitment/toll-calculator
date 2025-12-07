package afry.tollcollector.domain;

import java.util.Collections;
import java.util.List;

public record HolidaysInYear(int year, List<Holiday> holidays) {

  public static final HolidaysInYear EMPTY = new HolidaysInYear(Integer.MIN_VALUE, Collections.emptyList());

  public boolean isHoliday(int month, int day) {
    return holidays.stream().anyMatch(h -> h.month() == month && h.day() == day);
  }

}
