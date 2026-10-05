import java.time.LocalTime;

/**
 * FeeInterval
 */
public record FeeInterval(LocalTime startTime, LocalTime endTime, int fee) {
}
