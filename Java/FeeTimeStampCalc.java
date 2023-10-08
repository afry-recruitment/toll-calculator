import java.time.LocalTime;
import java.util.HashMap;
import java.util.Map;

public class FeeTimeStampCalc {

    private static final Map<LocalTime[], Integer> TIMESTAMPS = new HashMap<>();

    static {
        addTimeStamps("06:00", "06:29", 8);
        addTimeStamps("06:30", "06:59", 13);
        addTimeStamps("07:00", "07:59", 18);
        addTimeStamps("08:00", "08:29", 13);
        addTimeStamps("08:30", "14:59", 8);;
        addTimeStamps("15:00", "15:29", 13);
        addTimeStamps("15:30", "16:59", 18);
        addTimeStamps("17:00", "17:59", 13);
        addTimeStamps("18:00", "18:30", 8);
    }
    //populate addTimeStamps with keys and fee value
    public static void addTimeStamps(String lower, String upper, int fee) {
        LocalTime startTime = LocalTime.parse(lower);
        LocalTime endTime = LocalTime.parse(upper);
        TIMESTAMPS.put(new LocalTime[] { startTime, endTime }, fee);
    }
    // itirate through the map, get key based on time-range
    public static int calculate(LocalTime localTime) {
        for (Map.Entry<LocalTime[], Integer> entry : TIMESTAMPS.entrySet()) {
            LocalTime[] range = entry.getKey();
            if (isWithinRange(localTime, range[0], range[1])) {
                return entry.getValue();
            }
        }
        return 0;
    }

    private static boolean isWithinRange(LocalTime value, LocalTime start, LocalTime end) {
        return !value.isBefore(start) && value.isBefore(end);
    }
}
