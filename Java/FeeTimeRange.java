import java.time.LocalTime;

public class FeeTimeRange {
  private final LocalTime start, end;
  private final int fee;

  FeeTimeRange(LocalTime start, LocalTime end, int fee) {
    this.start = start;
    this.end = end;
    this.fee = fee;  
  }

  int getFee() {
    return fee;
  }

  boolean contains(LocalTime time) {
    if (start.equals(end))
      return time.equals(start);
    
    if (!end.isBefore(start))
      return !time.isBefore(start) && !time.isAfter(end);

    // Time range crosses midnight
    return !time.isBefore(start) || !time.isAfter(end);
  }

  public static FeeTimeRange of(int startHour, int startMinute, int endHour, int endMinute, int fee) {
    return new FeeTimeRange(LocalTime.of(startHour, startMinute), LocalTime.of(endHour, endMinute), fee);
  }

}
