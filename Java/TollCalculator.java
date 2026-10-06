
import java.time.Instant;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.ZoneId;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.EnumSet;
import java.util.List;
import java.util.NavigableMap;
import java.util.Objects;
import java.util.Set;
import java.util.TreeMap;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class TollCalculator {

    private final ZoneId tollStationZone;

    private final Set<VehicleType> tollVehicleTypes;

    private final NavigableMap<LocalTime, Integer> scheduledFees;

    private final int maxFeePerDay;

    private final boolean useGeneralSummerHoliday;

    /**
     * Creates a calculator with the default configuration, see {@link TollConfiguration#defaults()}.
     */
    public TollCalculator() {
        this(TollConfiguration.defaults());
    }

    /**
     * Creates a calculator with the given configuration.
     *
     * @param configuration - vehicle types, timezone, fee intervals, maximum fee per day and whether July is toll-free
     */
    public TollCalculator(TollConfiguration configuration) {
        if (configuration == null) {
            throw new IllegalArgumentException("Configuration must not be null");
        }

        tollVehicleTypes = configuration.getTollFeeVehicleTypes().stream()
                .map(VehicleType::valueOf)
                .collect(Collectors.toCollection(() -> EnumSet.noneOf(VehicleType.class)));

        this.tollStationZone = ZoneId.of(configuration.getTollStationTimezone());

        this.maxFeePerDay = configuration.getMaxFeePerDay();

        this.useGeneralSummerHoliday = configuration.useGeneralSummerHoliday();

        List<FeeInterval> intervalFees = configuration.getFeeIntervals();
        scheduledFees = new TreeMap<>();
        scheduledFees.put(LocalTime.MIDNIGHT, 0);
        for (FeeInterval interval : intervalFees) {
            scheduledFees.put(interval.startTime(), interval.fee());
            scheduledFees.putIfAbsent(interval.endTime(), 0);
        }
    }

    /**
     * NOTE: This method is deprecated because it uses the old Date class, which is less precise and less convenient than the newer Instant class.
     * It is recommended to use the getTollFee(Vehicle vehicle, Instant... instants) method instead.
     * 
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    @Deprecated 
    public int getTollFee(Vehicle vehicle, Date... dates) {
        // Keep nulls as nulls so that validateInput rejects them the same way as for Instants
        Instant[] instants = dates == null ? null : Arrays.stream(dates)
                .map(date -> date == null ? null : date.toInstant())
                .toArray(Instant[]::new);
        return getTollFee(vehicle, instants);
    }

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param instants   - instants of all passes on one day
     * @return - the total toll fee for that day
     */
    public int getTollFee(Vehicle vehicle, Instant... instants) {
        validateInput(vehicle, instants);
        
        // If no passgaes or toll-free vehicle, return 0 (no fee)
        if (instants.length == 0) {
            return 0;
        }

        // Check that all passages are on the same day
        List<LocalDateTime> localDateTimes = Arrays.stream(instants)
                .map(instant -> instant.atZone(tollStationZone).toLocalDateTime())
                .toList();
        if (!isSameDay(localDateTimes)) {
            throw new IllegalArgumentException("All instants must be on the same day");
        }

        // Check if the date is toll-free (weekend or holiday) or if the vehicle is toll-free
        if (isTollFreeDate(localDateTimes.get(0)) || isTollFreeVehicle(vehicle)) {
            return 0;
        }

        // Calculate the total toll fee for the day, considering the maximum fee per day 
        return getTollFeeForOneDay(localDateTimes);

    }

    private int getTollFeeForOneDay(List<LocalDateTime> localDateTimes) {
        List<List<LocalDateTime>> windows = groupPassagesIntoHourWindows(localDateTimes);

        int totalFee = windows.stream()
                .mapToInt(window -> window.stream().mapToInt(this::feeAt).max().orElse(0))
                .sum();
            return Math.min(totalFee, maxFeePerDay);
    }

    private List<List<LocalDateTime>> groupPassagesIntoHourWindows(List<LocalDateTime> localDateTimes) {
        List<List<LocalDateTime>> windows = new ArrayList<>();
        LocalDateTime windowStart = null;
        for (LocalDateTime dateTime : localDateTimes.stream().sorted().toList()) {
            if (windowStart == null || !dateTime.isBefore(windowStart.plusHours(1))) {
                windowStart = dateTime;
                windows.add(new ArrayList<>());
            }
            windows.get(windows.size() - 1).add(dateTime);
        }
        return windows;
    }

    private int feeAt(LocalDateTime dateTime) {
        return scheduledFees.floorEntry(dateTime.toLocalTime()).getValue();
    }

    private boolean isSameDay(List<LocalDateTime> localDateTimes) {
        LocalDate firstDay = localDateTimes.get(0).toLocalDate();
        return localDateTimes.stream()
            .allMatch(dateTime -> dateTime.toLocalDate().equals(firstDay));
    }

    private boolean isTollFreeVehicle(Vehicle vehicle) {
        return !tollVehicleTypes.contains(vehicle.getType());
    }

    private Boolean isTollFreeDate(LocalDateTime localDateTime) {
        return DateHelper.isTollFreeDate(localDateTime, useGeneralSummerHoliday);
    }

    private void validateInput(Vehicle vehicle, Instant[] instants) {
        if (vehicle == null) {
            throw new IllegalArgumentException("Vehicle must not be null");
        }
        if (instants == null) {
            throw new IllegalArgumentException("Instants must not be null");
        }
        if (Stream.of(instants).anyMatch(Objects::isNull)) {
            throw new IllegalArgumentException("Instant must not be null");
        }
    }
}

