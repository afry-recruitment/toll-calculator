import java.time.LocalDateTime;
import java.util.List;

public class FirstPassStrategy implements TollingStrategy {

    @Override
    public int getTollFee(Vehicle vehicle, List<LocalDateTime> dates) {
        LocalDateTime intervalStart = dates.get(0);
        int intervalToll = 0;

        int total = 0;
        for (LocalDateTime ldt : dates) {
            int currentToll = vehicle.getTollFee(ldt);
            if (currentToll == 0) {
                continue;
            }
            if (ldt.isBefore(intervalStart.plusHours(1))) {
                intervalToll = Math.max(intervalToll, currentToll);
                continue;
            }
            intervalStart = ldt;
            total += intervalToll;
            intervalToll = currentToll;
        }
        total += intervalToll;
        return total;
    }
    
}
