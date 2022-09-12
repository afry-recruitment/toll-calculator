package calculator;

import calculator.calendar.CalendarServiceInterface;
import calculator.vehicles.VehicleType;
import lombok.extern.slf4j.Slf4j;
import org.junit.jupiter.api.*;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.ZonedDateTime;
import java.util.Arrays;
import java.util.List;
import java.util.Set;

import static org.junit.jupiter.api.Assertions.*;

@Slf4j
class TollCalculatorTest
{
    private static CalendarServiceInterface calendarService;
    private static Set<LocalDate> holidays;
    private static Set<DayOfWeek> weekendDays;
    private static TollCalculator tollCalculator;

    @BeforeAll
    static void beforeAll()
    {
        // 2022-9-12 a monday, 2022-9-14 a wednessday
        holidays = Set.of(LocalDate.of(2022, 9, 12), LocalDate.of(2022, 9, 14));
        weekendDays = Set.of(DayOfWeek.SATURDAY, DayOfWeek.SUNDAY);
        // unnecessary and inconsistent to check
        calendarService = new CalendarServiceInterface()
        {
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
        tollCalculator = new TollCalculator(calendarService);
    }

    @Test
    void getTollFee_IsHoliday_0()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-12 07:00", "2022-09-14 07:00");
        assertEquals(0, tollCalculator.getTollFee(VehicleType.CAR, passes), "Holidays should pass for 0. ");
    }

    @Test
    void getTollFee_IsSaturday_0()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-10 07:00");
        assertEquals(0, tollCalculator.getTollFee(VehicleType.CAR, passes), "Weekends should pass for 0. ");
    }

    @Test
    void getTollFee_Pass1830_0()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-13 18:30");
        assertEquals(0,
                     tollCalculator.getTollFee(VehicleType.CAR, passes),
                     "A monday at 18.30 should pass for 0. ");
    }

    @Test @Disabled("Under development")
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
                     tollCalculator.getTollFee(VehicleType.CAR, passes),
                     "Max price per day should be 60. ");
    }

    @Test
    void getTollFee_TollFreeVehicle_0()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-13 07:00");
        assertEquals(0,
                     tollCalculator.getTollFee(VehicleType.MOTORBIKE, passes),
                     VehicleType.MOTORBIKE + " should be pass for 0. ");
    }

    @Test
    void getTollFee_PassTwiceInHour_HighestTollFee()
    {
        List<ZonedDateTime> passes = parseForDate("2022-09-13 06:00", // 8
                                                  "2022-09-13 06:30");// 13
        assertEquals(13,
                     tollCalculator.getTollFee(VehicleType.CAR, passes),
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
                     tollCalculator.getTollFee(VehicleType.CAR, passes),
                     "Hour interval should not be reset with subsequent passes with hour. ");
    }
    @Test @Disabled("Under development")
    void getTollFee_PassForMaxPriceOnTwoDays_ShouldPayForEachDay(){
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
                     tollCalculator.getTollFee(VehicleType.CAR, passes),
                     "Max price on two days should be 60 x 2 = 120. ");
    }
    //    @Test todo
    //    void getTollFee_DateAreUnOrdered_MatchPriceOfOrdered(){}

    @BeforeEach
    void setUp()
    {

    }

    @AfterEach
    void tearDown()
    {
    }

    /**
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