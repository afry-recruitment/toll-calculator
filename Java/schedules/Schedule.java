package schedules;

import java.time.LocalDateTime;

public interface Schedule {
    public int getTollFee(LocalDateTime ldt);
}
