package afry.tollcollector.domain;

public record TollFee(int hour, int minuteStart, int minuteEndInclusive, int tollFee) {

  public boolean matches(int hour, int minute) {
    return hour == this.hour && minute >= minuteStart && minute <= minuteEndInclusive;
  }
}
