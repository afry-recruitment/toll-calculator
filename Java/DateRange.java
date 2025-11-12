import java.time.MonthDay;

public class DateRange {
  private final MonthDay start, end;

  DateRange(MonthDay start, MonthDay end) {
    this.start = start;
    this.end = end;
  }

  boolean contains(MonthDay date) {
    if (start.equals(end))
      return date.equals(start);
    
    if (!end.isBefore(start))
      return !date.isBefore(start) && !date.isAfter(end);

    // Date range crosses new-years
    return !date.isBefore(start) || !date.isAfter(end);
  }

  public static DateRange of(int startMonth, int startDay, int endMonth, int endDay) {
    return new DateRange(MonthDay.of(startMonth, startDay), MonthDay.of(endMonth, endDay));
  }
}
