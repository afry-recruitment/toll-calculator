import java.time.LocalTime;
import java.util.List;

public final class TollConfiguration {

    private static final TollConfiguration DEFAULTS = new TollConfiguration(
            List.of("CAR"),
            "Europe/Stockholm",
            List.of(
                    new FeeInterval(LocalTime.of(6, 0), LocalTime.of(6, 30), 8),
                    new FeeInterval(LocalTime.of(6, 30), LocalTime.of(7, 0), 13),
                    new FeeInterval(LocalTime.of(7, 0), LocalTime.of(8, 0), 18),
                    new FeeInterval(LocalTime.of(8, 0), LocalTime.of(8, 30), 13),
                    new FeeInterval(LocalTime.of(8, 30), LocalTime.of(15, 0), 8),
                    new FeeInterval(LocalTime.of(15, 0), LocalTime.of(15, 30), 13),
                    new FeeInterval(LocalTime.of(15, 30), LocalTime.of(17, 0), 18),
                    new FeeInterval(LocalTime.of(17, 0), LocalTime.of(18, 0), 13),
                    new FeeInterval(LocalTime.of(18, 0), LocalTime.of(18, 30), 8)
            ),
            60,
            true
    );

    private final List<String> tollFeeVehicleTypes;
    private final String tollStationTimezone;
    private final List<FeeInterval> feeIntervals;
    private final int maxFeePerDay;
    private final boolean useGeneralSummerHoliday;

    public TollConfiguration(List<String> tollFeeVehicleTypes, String tollStationTimezone,
            List<FeeInterval> feeIntervals, int maxFeePerDay, boolean useGeneralSummerHoliday) {
        this.tollFeeVehicleTypes = List.copyOf(tollFeeVehicleTypes);
        this.tollStationTimezone = tollStationTimezone;
        this.feeIntervals = List.copyOf(feeIntervals);
        this.maxFeePerDay = maxFeePerDay;
        this.useGeneralSummerHoliday = useGeneralSummerHoliday;
    }

    /**
     * The default configuration: Swedish time, only cars are charged,
     * fees between 8 and 18 SEK and a maximum of 60 SEK per day.
     */
    public static TollConfiguration defaults() {
        return DEFAULTS;
    }

    public List<String> getTollFeeVehicleTypes() {
        return tollFeeVehicleTypes;
    }

    public String getTollStationTimezone() {
        return tollStationTimezone;
    }

    public List<FeeInterval> getFeeIntervals() {
        return feeIntervals;
    }

    public int getMaxFeePerDay() {
        return maxFeePerDay;
    }

    public boolean useGeneralSummerHoliday() {
        return useGeneralSummerHoliday;
    }
}
