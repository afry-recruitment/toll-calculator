

import java.time.LocalDateTime;
import java.util.List;

public interface TollingStrategy {
    public int getTollFee(Vehicle vehicle, List<LocalDateTime> dates);
}
