import java.time.LocalTime;

public class TollPeriod {
    private LocalTime start;
    private LocalTime end;
    private int fee;

    TollPeriod(int startHour, int startMinute, int endHour, int endMinute, int fee) {
        start = LocalTime.of(startHour, startMinute);
        end = LocalTime.of(endHour, endMinute);
        this.fee = fee;
    }

    int getFee() {
        return fee;
    }

    boolean contains(LocalTime time) {
        return !time.isBefore(start) && !time.isAfter(end);
    }
}
