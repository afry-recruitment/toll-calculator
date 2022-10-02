package calculator;

import calculator.calendar.CalendarService;
import calculator.tollrate.IntervalTollRate;
import calculator.tollrate.TollRateService;
import calculator.vehicle.VehicleService;
import calculator.vehicle.VehicleType;
import lombok.extern.slf4j.Slf4j;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.LocalTime;
import java.time.ZonedDateTime;
import java.util.Arrays;
import java.util.List;
import java.util.Set;

import static org.junit.jupiter.api.Assertions.assertEquals;

@Slf4j
class TollCalculatorTest
{
    private static TollCalculator tollCalculator;

    @BeforeAll
    static void beforeAll()
    {

        CalendarService calendarService = new CalendarService()
        {
            // 2022-9-12 a monday, 2022-9-14 a wednessday
            Set<LocalDate> holidays = Set.of(LocalDate.of(2022, 9, 12), LocalDate.of(2022, 9, 14));
            Set<DayOfWeek> weekendDays = Set.of(DayOfWeek.SATURDAY, DayOfWeek.SUNDAY);
            @Override
            public boolean isWeekend(LocalDate date)
            {
                return weekendDays.contains(date.getDayOfWeek());
            }

            @Override
            public boolean isHoliday(LocalDate date)
            {
                return holidays.contains(date);
            }
        };
        VehicleService vehicleService = new VehicleService()
        {
            @Override
            public boolean isToolFree(String vehicleType)
            {
                return getVehicleType(vehicleType).isTollFree();
            }

            @Override
            public Set<VehicleType> listVehicleTypes()
            {
                return null;
            }

            @Override
            public Set<String> listVehicleTypeNames()
            {
                return null;
            }

            @Override
            public VehicleType getVehicleType(String name)
            {
                if (name.equalsIgnoreCase("CAR")) return new VehicleType("CAR", false);
                if (name.equalsIgnoreCase("MOTORBIKE")) return new VehicleType("MOTORBIKE", true);
                throw new IllegalArgumentException();
            }
        };
        TollRateService tollRateService = new TollRateService()
        {
            Set<IntervalTollRate> tollRates =
                    Set.of(new IntervalTollRate(LocalTime.parse("00:00:00"), LocalTime.parse("06:00:00"), 0),
                           new IntervalTollRate(LocalTime.parse("06:00:00"), LocalTime.parse("06:30:00"), 8),
                           new IntervalTollRate(LocalTime.parse("06:30:00"), LocalTime.parse("07:00:00"), 13),
                           new IntervalTollRate(LocalTime.parse("07:00:00"), LocalTime.parse("08:00:00"), 18),
                           new IntervalTollRate(LocalTime.parse("08:00:00"), LocalTime.parse("08:30:00"), 18),
                           new IntervalTollRate(LocalTime.parse("08:30:00"), LocalTime.parse("15:00:00"), 8),
                           new IntervalTollRate(LocalTime.parse("15:00:00"), LocalTime.parse("15:30:00"), 13),
                           new IntervalTollRate(LocalTime.parse("15:30:00"), LocalTime.parse("17:00:00"), 18),
                           new IntervalTollRate(LocalTime.parse("17:00:00"), LocalTime.parse("18:00:00"), 13),
                           new IntervalTollRate(LocalTime.parse("18:00:00"), LocalTime.parse("18:30:00"), 8),
                           new IntervalTollRate(LocalTime.parse("18:30:00"), LocalTime.parse("23:59:59"), 0));

            @Override
            public int getTollFeeAtTime(final LocalTime localTime)
            {
                return this.tollRates.parallelStream()
                                     .filter(it -> it.encompasses(localTime))
                                     .findFirst()
                                     .orElseThrow(() -> new RuntimeException(
                                             "Toll-rate table does not fully cover a day. " + localTime))
                                     .getRate();
            }
        };
        tollCalculator = new TollCalculator(calendarService, vehicleService, tollRateService);

    }

    @Test
    void getTollFee_IsHoliday_0()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-12 07:00", "2022-09-14 07:00");
        assertEquals(0,
                     tollCalculator.getTollFee("CAR", passes)
                                   .getActualFee(),
                     "Holidays should pass for 0. ");
    }

    @Test
    void getTollFee_IsSaturday_0()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-10 07:00");
        assertEquals(0,
                     tollCalculator.getTollFee("CAR", passes)
                                   .getActualFee(),
                     "Weekends should pass for 0. ");
    }

    @Test
    void getTollFee_Pass1830_0()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-13 18:30");
        assertEquals(0,
                     tollCalculator.getTollFee("CAR", passes)
                                   .getActualFee(),
                     "A monday at 18.30 should pass for 0. ");
    }

    @Test
    void getTollFee_PassManyTimes_PayNoMoreThan60()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-13 06:00", // 8
                                                  "2022-09-13 07:00", // 18
                                                  "2022-09-13 08:00", // 13
                                                  "2022-09-13 09:00", // 8
                                                  "2022-09-13 10:00", // 8
                                                  "2022-09-13 11:00", // 8
                                                  "2022-09-13 12:00");// 8
        assertEquals(60,
                     tollCalculator.getTollFee("CAR", passes)
                                   .getActualFee(),
                     "Max price per day should be 60. ");
    }

    @Test
    void getTollFee_TollFreeVehicle_0()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-13 07:00");
        assertEquals(0,
                     tollCalculator.getTollFee("MOTORBIKE", passes)
                                   .getActualFee(),
                     "MOTORBIKE" + " should be pass for 0. ");
    }

    @Test
    void getTollFee_PassTwiceInHour_HighestTollFee()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-13 06:00", // 8
                                                  "2022-09-13 06:30");// 13
        assertEquals(13,
                     tollCalculator.getTollFee("CAR", passes)
                                   .getActualFee(),
                     "Should be the highest fee when passing multiple passes in one hour. ");
    }

    // scenario: pass at three occasions A, B and C so that B-A<1h and C-B<1h but C-A>1h
    @Test
    void getTollFee_PassThreeTimesHourlyOverlapping_NoResetOfInterval()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-13 06:00", // 8
                                                  "2022-09-13 06:30", // 13
                                                  "2022-09-13 07:15");// 18
        assertEquals(31,
                     tollCalculator.getTollFee("CAR", passes)
                                   .getActualFee(),
                     "Hour interval should not be reset with subsequent passes with hour. ");
    }

    @Test
    void getTollFee_PassForMaxPriceOnTwoDays_ShouldPayForEachDay()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-15 06:00", // 8
                                                  "2022-09-15 07:00", // 18
                                                  "2022-09-15 08:00", // 13
                                                  "2022-09-15 09:00", // 8
                                                  "2022-09-15 10:00", // 8
                                                  "2022-09-15 11:00", // 8
                                                  "2022-09-15 12:00", // 8
                                                  "2022-09-16 06:00", // 8
                                                  "2022-09-16 07:00", // 18
                                                  "2022-09-16 08:00", // 13
                                                  "2022-09-16 09:00", // 8
                                                  "2022-09-16 10:00", // 8
                                                  "2022-09-16 11:00", // 8
                                                  "2022-09-16 12:00");// 8
        assertEquals(120,
                     tollCalculator.getTollFee("CAR", passes)
                                   .getActualFee(),
                     "Max price on two days should be 60 x 2 = 120. ");
    }

    /*
     *
     * Helper method to reduce some redundancy as TollFeeCalculator requires ZonedDateTime but the time
     * zone is not really relevant for most of the tests.
     *
     * @param dateTime yyyy-MM-dd hh:mm
     * @return ZonedDateTime with time zone UTC+0 and 00 seconds. If time is null time is 12:00:00

     */

    private static List<ZonedDateTime> parseForDate(String... dateTime)
    {
        return Arrays.stream(dateTime)
                     .map(dt ->
                          {
                              String[] split = dt.split(" ");
                              if (split.length != 2) log.error("Bada data for parseForDate(String...)");
                              return ZonedDateTime.parse(split[0] + "T" + split[1] + ":00+00:00");
                          })
                     .toList();
    }
}
