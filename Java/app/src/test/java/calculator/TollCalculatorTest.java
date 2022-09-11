package calculator;

import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

class TollCalculatorTest
{
    /*    // make calendar service with predictable date - holidays?
        CalendarService calendarService*/
    @BeforeAll
    void beforeAll()
    {

    }

    @Test
    void getTollFee_IsHoliday_0()
    {
    }

    @Test
    void getTollFee_IsSaturday_0()
    {
    }

    @Test
    void getTollFee_Pass1830_0()
    {
    }

    @Test
    void getTollFee_PassManyTimes_PayNoMoreThan60()
    {
    }

    @Test
    void getTollFee_TollFreeVehicle_0()
    {
    }

    @Test
    void getTollFee_PassTwiceInHour_HighestTollFee()
    {
    }

    @Test
    void getTollFee_PassThreeTimesHourlyOverlapping_PayForEveryHourlyInterval()
    {
    }
//    @Test
//    void getTollFee_DateAreUnOrdered_MatchPriceOfOrdered(){}

    @BeforeEach

    void setUp()
    {

    }

    @AfterEach
    void tearDown()
    {
    }
}