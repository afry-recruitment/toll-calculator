import java.time.Month;
import java.time.MonthDay;

public class HolidayPeriod {
    private MonthDay start;
    private MonthDay end;

    HolidayPeriod(Month startMonth, int startDayOfMonth, Month endMonth, int endDayOfMonth) {
        this.start = MonthDay.of(startMonth, startDayOfMonth);
        this.end = MonthDay.of(endMonth, endDayOfMonth);
    }

    HolidayPeriod(Month month, int dayOfMonth) {
        this.start = MonthDay.of(month, dayOfMonth);
        this.end = MonthDay.of(month, dayOfMonth);
    }

    boolean contains(MonthDay other) {
        return !other.isBefore(start) && !other.isAfter(end);
    }
}
