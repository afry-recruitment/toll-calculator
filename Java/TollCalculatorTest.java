
import static org.junit.jupiter.api.Assertions.*;

import java.time.Instant;
import java.time.LocalDate;
import java.time.LocalTime;
import java.time.ZoneId;
import java.util.Arrays;
import java.util.Comparator;
import java.util.Date;
import java.util.List;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Nested;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.junit.jupiter.params.provider.EnumSource;
import org.junit.jupiter.params.provider.ValueSource;

class TollCalculatorTest {

    private static final ZoneId STOCKHOLM = ZoneId.of("Europe/Stockholm");

    // Tuesday, not a holiday
    private static final LocalDate WEEKDAY = LocalDate.of(2026, 2, 3);

    // The rules from the requirements, defined here so the tests do not depend on TollConfiguration.defaults()
    private static final TollConfiguration TEST_CONFIGURATION = new TollConfiguration(
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

    private final TollCalculator calculator = new TollCalculator(TEST_CONFIGURATION);
    private final Vehicle car = new Car();

    private static Instant at(LocalDate day, LocalTime time) {
        return day.atTime(time).atZone(STOCKHOLM).toInstant();
    }

    private static Instant at(LocalDate day, String time) {
        return at(day, LocalTime.parse(time));
    }

    private static Instant weekday(String time) {
        return at(WEEKDAY, time);
    }

    private static Vehicle vehicleOfType(VehicleType type) {
        return () -> type;
    }

    private int feeAt(LocalTime time) {
        return calculator.getTollFee(car, at(WEEKDAY, time));
    }

    @Nested
    @DisplayName("Fees differ between 8 and 18 SEK depending on time of day")
    class FeeByTimeOfDay {

        @ParameterizedTest(name = "{0} -> {1} SEK")
        @CsvSource({
                "00:00, 0",
                "05:59, 0",
                "06:00, 8",
                "06:29, 8",
                "06:30, 13",
                "06:59, 13",
                "07:00, 18",
                "07:59, 18",
                "08:00, 13",
                "08:29, 13",
                "08:30, 8",
                "09:00, 8",
                "12:15, 8",
                "14:59, 8",
                "15:00, 13",
                "15:29, 13",
                "15:30, 18",
                "16:59, 18",
                "17:00, 13",
                "17:59, 13",
                "18:00, 8",
                "18:29, 8",
                "18:30, 0",
                "23:59, 0"
        })
        void feeForSinglePassage(String time, int expectedFee) {
            assertEquals(expectedFee, calculator.getTollFee(car, weekday(time)));
        }

        @Test
        void everyMinuteOfTheDayIsFreeOrBetween8And18() {
            for (LocalTime t = LocalTime.MIDNIGHT; ; t = t.plusMinutes(1)) {
                int fee = feeAt(t);
                assertTrue(fee == 0 || (fee >= 8 && fee <= 18), "Unexpected fee " + fee + " at " + t);
                if (t.equals(LocalTime.of(23, 59))) break;
            }
        }

        @Test
        void timeOfDayIsSwedishLocalTime() {
            // 06:30 UTC is 07:30 in Stockholm (winter time) -> rush hour
            assertEquals(18, calculator.getTollFee(car, Instant.parse("2026-02-03T06:30:00Z")));
        }

        @Test
        void timeOfDayFollowsSwedishSummerTime() {
            // 05:30 UTC is 07:30 in Stockholm (summer time) -> rush hour
            assertEquals(18, calculator.getTollFee(car, Instant.parse("2026-06-02T05:30:00Z")));
        }
    }

    @Nested
    @DisplayName("Rush-hour traffic renders the highest fee")
    class RushHour {

        @ParameterizedTest(name = "{0} is rush hour")
        @ValueSource(strings = {"07:00", "07:30", "07:59", "15:30", "16:00", "16:59"})
        void rushHourChargesHighestFeeOfTheDay(String time) {
            int highestFee = 0;
            for (LocalTime t = LocalTime.MIDNIGHT; ; t = t.plusMinutes(1)) {
                highestFee = Math.max(highestFee, feeAt(t));
                if (t.equals(LocalTime.of(23, 59))) break;
            }
            assertEquals(18, highestFee);
            assertEquals(highestFee, calculator.getTollFee(car, weekday(time)));
        }
    }

    @Nested
    @DisplayName("The maximum fee for one day is 60 SEK")
    class DailyMaximum {

        @Test
        void totalIsCappedAt60() {
            // 8 + 18 + 13 + 18 + 18 = 75 if uncapped
            int total = calculator.getTollFee(car,
                    weekday("06:00"), weekday("07:05"), weekday("08:10"), weekday("15:30"), weekday("16:40"));
            assertEquals(60, total);
        }

        @Test
        void totalBelowCapIsNotAffected() {
            // 8 + 18 + 13 = 39
            int total = calculator.getTollFee(car, weekday("06:00"), weekday("07:05"), weekday("08:10"));
            assertEquals(39, total);
        }
    }

    @Nested
    @DisplayName("A vehicle is only charged once an hour, the highest fee applies")
    class OncePerHour {

        @Test
        void singlePassage() {
            assertEquals(18, calculator.getTollFee(car, weekday("07:15")));
        }

        @Test
        void twoPassagesWithinAnHourChargeTheHighestFee() {
            // 8 at 06:20, 13 at 06:50
            assertEquals(13, calculator.getTollFee(car, weekday("06:20"), weekday("06:50")));
        }

        @Test
        void highestFeeAppliesEvenWhenItComesFirst() {
            // 18 at 07:50, 13 at 08:20
            assertEquals(18, calculator.getTollFee(car, weekday("07:50"), weekday("08:20")));
        }

        @Test
        void manyPassagesWithinAnHourAreChargedOnce() {
            assertEquals(18, calculator.getTollFee(car,
                    weekday("07:00"), weekday("07:10"), weekday("07:20"), weekday("07:30"), weekday("07:59")));
        }

        @Test
        void passagesMoreThanAnHourApartAreChargedSeparately() {
            // 8 at 06:00, 18 at 07:05
            assertEquals(26, calculator.getTollFee(car, weekday("06:00"), weekday("07:05")));
        }

        @Test
        void newHourPeriodStartsAtFirstPassageAfterThePreviousPeriod() {
            // Period 1: 06:00 (8), 06:45 (13) -> 13
            // Period 2: 07:30 (18), 08:15 (13) -> 18
            assertEquals(31, calculator.getTollFee(car,
                    weekday("06:00"), weekday("06:45"), weekday("07:30"), weekday("08:15")));
        }

        @Test
        void passageJustUnderAnHourLaterIsInTheSamePeriod() {
            // 06:00 (8), 06:59:59 (13) -> 13
            assertEquals(13, calculator.getTollFee(car, weekday("06:00"), weekday("06:59:59")));
        }

        @Test
        void passageExactlyAnHourLaterStartsANewPeriod() {
            // 06:00 (8), 07:00 (18) -> 8 + 18
            assertEquals(26, calculator.getTollFee(car, weekday("06:00"), weekday("07:00")));
        }

        @Test
        void passagesInAnyOrderGiveTheSameFee() {
            // Same passages as newHourPeriodStartsAtFirstPassageAfterThePreviousPeriod, shuffled
            assertEquals(31, calculator.getTollFee(car,
                    weekday("08:15"), weekday("06:45"), weekday("07:30"), weekday("06:00")));
        }

        @Test
        void passagesInReverseOrderGiveTheSameFee() {
            assertEquals(calculator.getTollFee(car, weekday("06:00"), weekday("07:05"), weekday("08:10")),
                    calculator.getTollFee(car, weekday("08:10"), weekday("07:05"), weekday("06:00")));
        }
    }

    @Nested
    @DisplayName("Some vehicle types are fee-free")
    class FeeFreeVehicles {

        @Test
        void motorbikeIsFeeFree() {
            assertEquals(0, calculator.getTollFee(new Motorbike(), weekday("07:30")));
        }

        @ParameterizedTest(name = "{0} is fee-free")
        @EnumSource(value = VehicleType.class, names = "CAR", mode = EnumSource.Mode.EXCLUDE)
        void feeFreeVehicleTypes(VehicleType type) {
            Vehicle vehicle = vehicleOfType(type);
            assertEquals(0, calculator.getTollFee(vehicle, weekday("07:30")));
            assertEquals(0, calculator.getTollFee(vehicle,
                    weekday("06:00"), weekday("07:05"), weekday("15:30"), weekday("16:40")));
        }

        @Test
        void carIsCharged() {
            assertEquals(18, calculator.getTollFee(car, weekday("07:30")));
        }
    }

    @Nested
    @DisplayName("Weekends and holidays are fee-free")
    class FeeFreeDays {

        private void assertFeeFree(String date) {
            LocalDate day = LocalDate.parse(date);
            assertEquals(0, calculator.getTollFee(car, at(day, "07:30")));
            assertEquals(0, calculator.getTollFee(car, at(day, "07:30"), at(day, "16:00")));
        }

        private void assertCharged(String date) {
            assertEquals(18, calculator.getTollFee(car, at(LocalDate.parse(date), "07:30")));
        }

        @ParameterizedTest(name = "{0} is a weekend")
        @ValueSource(strings = {
                "2026-02-07", "2026-02-08",
                "2027-03-06", "2027-03-07",
                "2028-02-26", "2028-02-27",
                "2032-02-28", "2032-02-29"    // leap day on a Sunday
        })
        void weekendsAreFeeFree(String date) {
            assertFeeFree(date);
        }

        @ParameterizedTest(name = "{0} is a fixed holiday")
        @ValueSource(strings = {
                "2026-01-01", "2026-01-06", "2026-05-01", "2026-12-25",
                "2028-01-06", "2028-05-01", "2028-06-06", "2028-12-25", "2028-12-26",
                "2030-01-01", "2030-05-01", "2030-06-06", "2030-12-25", "2030-12-26",
                "2032-01-01", "2032-01-06",
                "2035-01-01", "2035-05-01", "2035-06-06", "2035-12-25", "2035-12-26"
        })
        void fixedHolidaysAreFeeFree(String date) {
            assertFeeFree(date);
        }

        @ParameterizedTest(name = "{0} is the day before a fixed holiday")
        @ValueSource(strings = {
                "2026-01-05", "2026-04-30", "2026-06-05", "2026-12-24", "2026-12-31",
                "2028-01-05", "2028-06-05",
                "2030-04-30", "2030-06-05", "2030-12-24", "2030-12-31",
                "2032-01-05", "2032-04-30", "2032-12-24", "2032-12-31",
                "2035-01-05", "2035-04-30", "2035-12-24", "2035-12-31"
        })
        void dayBeforeFixedHolidayIsFeeFree(String date) {
            assertFeeFree(date);
        }

        @ParameterizedTest(name = "{0} is an Easter-based holiday")
        @ValueSource(strings = {
                "2026-04-03", "2026-04-06", "2026-05-14",   // Easter 2026-04-05
                "2027-03-26", "2027-03-29", "2027-05-06",   // Easter 2027-03-28
                "2028-04-14", "2028-04-17", "2028-05-25",   // Easter 2028-04-16 (leap year)
                "2030-04-19", "2030-04-22", "2030-05-30",   // Easter 2030-04-21
                "2032-03-26", "2032-03-29", "2032-05-06",   // Easter 2032-03-28 (leap year)
                "2035-03-23", "2035-03-26", "2035-05-03"    // Easter 2035-03-25
        })
        void easterBasedHolidaysAreFeeFree(String date) {
            assertFeeFree(date);
        }

        @ParameterizedTest(name = "{0} is the day before an Easter-based holiday")
        @ValueSource(strings = {
                "2026-04-02", "2026-05-13",
                "2027-03-25", "2027-05-05",
                "2028-04-13", "2028-05-24",
                "2030-04-18", "2030-05-29",
                "2032-03-25", "2032-05-05",
                "2035-03-22", "2035-05-02"
        })
        void dayBeforeEasterBasedHolidayIsFeeFree(String date) {
            assertFeeFree(date);
        }

        @ParameterizedTest(name = "{0} is the day before Midsummer Day or All Saints' Day")
        @ValueSource(strings = {
                "2026-06-19", "2026-10-30",
                "2027-06-25", "2027-11-05",
                "2028-06-23", "2028-11-03",
                "2030-06-21", "2030-11-01",
                "2032-06-25", "2032-11-05",
                "2035-06-22", "2035-11-02"
        })
        void dayBeforeSaturdayHolidayIsFeeFree(String date) {
            assertFeeFree(date);
        }

        @ParameterizedTest(name = "{0} is in July")
        @ValueSource(strings = {"2026-07-01", "2026-07-15", "2026-07-31", "2028-07-03", "2032-07-30"})
        void julyIsFeeFree(String date) {
            assertFeeFree(date);
        }

        @ParameterizedTest(name = "{0} is an ordinary weekday")
        @ValueSource(strings = {
                "2026-02-03",   // ordinary Tuesday
                "2026-04-07",   // day after Easter Monday
                "2026-05-15",   // Friday after Ascension Day
                "2026-06-18",   // Thursday before Midsummer Eve
                "2026-06-30",   // day before July
                "2026-08-03",   // day after July
                "2027-06-07",   // Monday after National Day on a Sunday
                "2028-02-28",   // day before leap day
                "2028-02-29",   // leap day on a Tuesday
                "2028-03-01",   // day after leap day
                "2028-04-18",   // day after Easter Monday
                "2030-06-07",   // Friday after National Day
                "2030-12-27",   // Friday after Boxing Day
                "2032-03-01",   // Monday after leap day
                "2032-06-24",   // Thursday before Midsummer Eve
                "2035-05-04"    // Friday after Ascension Day
        })
        void ordinaryWeekdaysAreCharged(String date) {
            assertCharged(date);
        }
    }

    @Nested
    @DisplayName("Invalid input is rejected")
    class InvalidInput {

        @Test
        void nullVehicle() {
            assertThrows(IllegalArgumentException.class, () -> calculator.getTollFee(null, weekday("07:30")));
        }

        @Test
        void nullInstantArray() {
            assertThrows(IllegalArgumentException.class, () -> calculator.getTollFee(car, (Instant[]) null));
        }

        @Test
        void nullInstantInArray() {
            assertThrows(IllegalArgumentException.class,
                    () -> calculator.getTollFee(car, weekday("07:30"), null));
        }

        @Test
        void passagesOnDifferentDays() {
            assertThrows(IllegalArgumentException.class, () -> calculator.getTollFee(car,
                    weekday("07:30"), at(WEEKDAY.plusDays(1), "07:30")));
        }
    }

    @Nested
    @DisplayName("No passages cost nothing")
    class NoPassages {

        // calculator.getTollFee(car) with no passages does not compile:
        // it is ambiguous between getTollFee(Vehicle, Instant...) and getTollFee(Vehicle, Date...)

        @Test
        void emptyArray() {
            assertEquals(0, calculator.getTollFee(car, new Instant[0]));
        }

        @Test
        void feeFreeVehicle() {
            assertEquals(0, calculator.getTollFee(new Motorbike(), new Instant[0]));
        }
    }

    @Nested
    @DisplayName("Deprecated Date input gives the same result as Instant input")
    @SuppressWarnings("deprecation")
    class DateInput {

        @Test
        void singlePassage() {
            assertEquals(18, calculator.getTollFee(car, Date.from(weekday("07:30"))));
        }

        @Test
        void severalPassages() {
            Instant[] instants = {weekday("06:00"), weekday("06:45"), weekday("07:30"), weekday("08:15")};
            Date[] dates = Arrays.stream(instants).map(Date::from).toArray(Date[]::new);
            assertEquals(calculator.getTollFee(car, instants), calculator.getTollFee(car, dates));
        }

        @Test
        void feeFreeDay() {
            assertEquals(0, calculator.getTollFee(car, Date.from(at(LocalDate.of(2026, 12, 24), "07:30"))));
        }

        @Test
        void nullDateArrayIsRejectedLikeInstants() {
            assertThrows(IllegalArgumentException.class, () -> calculator.getTollFee(car, (Date[]) null));
        }

        @Test
        void nullDateInArrayIsRejectedLikeInstants() {
            assertThrows(IllegalArgumentException.class,
                    () -> calculator.getTollFee(car, Date.from(weekday("07:30")), null));
        }
    }

    @Nested
    @DisplayName("The default configuration is valid")
    class DefaultConfiguration {

        // All rule tests use TEST_CONFIGURATION, so this ties the defaults to the requirements
        @Test
        void defaultsMatchTheRequirements() {
            TollConfiguration defaults = TollConfiguration.defaults();
            assertAll(
                    () -> assertEquals(TEST_CONFIGURATION.getTollFeeVehicleTypes(), defaults.getTollFeeVehicleTypes()),
                    () -> assertEquals(TEST_CONFIGURATION.getTollStationTimezone(), defaults.getTollStationTimezone()),
                    () -> assertEquals(TEST_CONFIGURATION.getFeeIntervals(), defaults.getFeeIntervals()),
                    () -> assertEquals(TEST_CONFIGURATION.getMaxFeePerDay(), defaults.getMaxFeePerDay()),
                    () -> assertEquals(TEST_CONFIGURATION.useGeneralSummerHoliday(), defaults.useGeneralSummerHoliday()));
        }

        @Test
        void timezoneIsValid() {
            assertDoesNotThrow(() -> ZoneId.of(TollConfiguration.defaults().getTollStationTimezone()));
        }

        @Test
        void vehicleTypesAreKnown() {
            for (String type : TollConfiguration.defaults().getTollFeeVehicleTypes()) {
                assertDoesNotThrow(() -> VehicleType.valueOf(type), "Unknown vehicle type: " + type);
            }
        }

        @Test
        void maxFeePerDayIsNotNegative() {
            assertTrue(TollConfiguration.defaults().getMaxFeePerDay() >= 0);
        }

        @Test
        void feeIntervalsAreValid() {
            for (FeeInterval interval : TollConfiguration.defaults().getFeeIntervals()) {
                assertTrue(interval.startTime().isBefore(interval.endTime()), "Start must be before end: " + interval);
                assertTrue(interval.fee() >= 0, "Fee must not be negative: " + interval);
            }
        }

        @Test
        void feeIntervalsDoNotOverlap() {
            List<FeeInterval> sorted = TollConfiguration.defaults().getFeeIntervals().stream()
                    .sorted(Comparator.comparing(FeeInterval::startTime))
                    .toList();
            for (int i = 1; i < sorted.size(); i++) {
                FeeInterval previous = sorted.get(i - 1);
                FeeInterval current = sorted.get(i);
                assertFalse(current.startTime().isBefore(previous.endTime()),
                        "Overlapping intervals: " + previous + " and " + current);
            }
        }
    }

    @Nested
    @DisplayName("A custom configuration can be given")
    class CustomConfiguration {

        private final TollConfiguration base = TEST_CONFIGURATION;

        private TollCalculator calculatorWith(List<String> vehicleTypes, String timezone,
                List<FeeInterval> feeIntervals, int maxFeePerDay) {
            return new TollCalculator(new TollConfiguration(vehicleTypes, timezone, feeIntervals, maxFeePerDay,
                    base.useGeneralSummerHoliday()));
        }

        @Test
        void defaultConstructorUsesDefaultConfiguration() {
            TollCalculator withDefaults = new TollCalculator(TollConfiguration.defaults());
            TollCalculator defaultConstructor = new TollCalculator();
            Instant[] passages = {weekday("06:00"), weekday("07:05"), weekday("08:10"), weekday("15:30"), weekday("16:40")};
            assertEquals(withDefaults.getTollFee(car, passages), defaultConstructor.getTollFee(car, passages));
        }

        @Test
        void customFeeIntervals() {
            TollCalculator custom = calculatorWith(base.getTollFeeVehicleTypes(), base.getTollStationTimezone(),
                    List.of(new FeeInterval(LocalTime.of(5, 30), LocalTime.of(7, 0), 25)),
                    base.getMaxFeePerDay());
            assertEquals(0, custom.getTollFee(car, weekday("05:29")));
            assertEquals(25, custom.getTollFee(car, weekday("05:30")));
            assertEquals(25, custom.getTollFee(car, weekday("06:59")));
            assertEquals(0, custom.getTollFee(car, weekday("07:00")));
        }

        @Test
        void customMaxFeePerDay() {
            TollCalculator custom = calculatorWith(base.getTollFeeVehicleTypes(), base.getTollStationTimezone(),
                    base.getFeeIntervals(), 30);
            // 8 + 18 + 13 = 39 with the test schedule
            assertEquals(30, custom.getTollFee(car, weekday("06:00"), weekday("07:05"), weekday("08:10")));
        }

        @Test
        void customVehicleTypes() {
            TollCalculator custom = calculatorWith(List.of("CAR", "MOTORBIKE"), base.getTollStationTimezone(),
                    base.getFeeIntervals(), base.getMaxFeePerDay());
            assertEquals(18, custom.getTollFee(new Motorbike(), weekday("07:30")));
            assertEquals(0, custom.getTollFee(vehicleOfType(VehicleType.TRACTOR), weekday("07:30")));
        }

        @Test
        void customTimezone() {
            TollCalculator custom = calculatorWith(base.getTollFeeVehicleTypes(), "UTC",
                    base.getFeeIntervals(), base.getMaxFeePerDay());
            // 07:30 UTC is rush hour in UTC, but 08:30 in Stockholm
            Instant passage = Instant.parse("2026-02-03T07:30:00Z");
            assertEquals(18, custom.getTollFee(car, passage));
            assertEquals(8, calculator.getTollFee(car, passage));
        }

        @Test
        void nullConfigurationIsRejected() {
            assertThrows(IllegalArgumentException.class, () -> new TollCalculator(null));
        }
    }

    @Nested
    @DisplayName("The general summer holiday (July) can be turned on or off")
    class GeneralSummerHoliday {

        private TollCalculator calculatorWithSummerHoliday(boolean useGeneralSummerHoliday) {
            return new TollCalculator(new TollConfiguration(
                    TEST_CONFIGURATION.getTollFeeVehicleTypes(),
                    TEST_CONFIGURATION.getTollStationTimezone(),
                    TEST_CONFIGURATION.getFeeIntervals(),
                    TEST_CONFIGURATION.getMaxFeePerDay(),
                    useGeneralSummerHoliday));
        }

        private int feeAt(TollCalculator calculator, String date) {
            return calculator.getTollFee(car, at(LocalDate.parse(date), "07:30"));
        }

        @ParameterizedTest(name = "{0} is free when the summer holiday is on")
        @ValueSource(strings = {"2026-07-01", "2026-07-15", "2026-07-31", "2028-07-03", "2032-07-30"})
        void julyWeekdaysAreFreeWhenOn(String date) {
            assertEquals(0, feeAt(calculatorWithSummerHoliday(true), date));
        }

        @ParameterizedTest(name = "{0} is charged when the summer holiday is off")
        @ValueSource(strings = {"2026-07-01", "2026-07-15", "2026-07-31", "2028-07-03", "2032-07-30"})
        void julyWeekdaysAreChargedWhenOff(String date) {
            assertEquals(18, feeAt(calculatorWithSummerHoliday(false), date));
        }

        @ParameterizedTest(name = "{0} is still free when the summer holiday is off")
        @ValueSource(strings = {
                "2026-07-04", "2026-07-05",   // weekend in July
                "2026-12-25",                 // fixed holiday
                "2026-04-03",                 // Easter-based holiday
                "2026-06-19"                  // day before Midsummer Day
        })
        void otherFreeDaysAreNotAffectedWhenOff(String date) {
            assertEquals(0, feeAt(calculatorWithSummerHoliday(false), date));
        }

        @ParameterizedTest(name = "{0} is charged whether the summer holiday is on or off")
        @ValueSource(strings = {"2026-06-30", "2026-08-03"})
        void daysAroundJulyAreChargedEitherWay(String date) {
            assertEquals(18, feeAt(calculatorWithSummerHoliday(true), date));
            assertEquals(18, feeAt(calculatorWithSummerHoliday(false), date));
        }

        @Test
        void calculatorsWithDifferentSettingsDoNotAffectEachOther() {
            TollCalculator withoutSummerHoliday = calculatorWithSummerHoliday(false);
            TollCalculator withSummerHoliday = calculatorWithSummerHoliday(true);
            assertEquals(18, feeAt(withoutSummerHoliday, "2026-07-15"));
            assertEquals(0, feeAt(withSummerHoliday, "2026-07-15"));
        }
    }
}
