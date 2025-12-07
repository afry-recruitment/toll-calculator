package afry.tollcollector.configuration;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertTrue;

import afry.tollcollector.domain.Holidays;
import afry.tollcollector.domain.TollFees;
import java.util.Calendar;
import java.util.function.BiFunction;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

@SpringBootTest
public class DefaultConfigTest {

  /**
   * Tests using the previous implementation of toll fee hour.
   */
  @Test
  public void getTollFeeHourMinTest() {
    BiFunction<Integer, Integer, Integer> func = DefaultConfigTest::getTollFeeHourOld;
    initialTestParameters(func);
  }

  @Autowired
  private DefaultConfig defaultConfig;

  @Test
  public void getTollFeesWithDefaultData() {
    TollFees defaultTollFees = defaultConfig.defaultTollFees();
    initialTestParameters(defaultTollFees::getTollFees);
  }

  @Test
  public void defaultHolidayTest() {
    Holidays holidays = defaultConfig.defaultHolidays();
    assertTrue(holidays.isHolidayIfKnown(2013, Calendar.JANUARY, 1));
    assertTrue(holidays.isHolidayIfKnown(2013, Calendar.MAY, 8));
    assertTrue(holidays.isHolidayIfKnown(2013, Calendar.MAY, 9));

    assertFalse(holidays.isHolidayIfKnown(2013, Calendar.JANUARY, 7));
    assertFalse(holidays.isHolidayIfKnown(2013, Calendar.DECEMBER, 31));

    assertFalse(holidays.isHolidayIfKnown(2014, Calendar.JANUARY, 1));

    assertTrue(holidays.isHolidayIfKnown(2025, Calendar.JANUARY, 1));
  }

  private void initialTestParameters(BiFunction<Integer, Integer, Integer> func) {
    assertEquals(8, func.apply(6, 28));
    assertEquals(13, func.apply(6, 38));
    assertEquals(18, func.apply(7, 38));
    assertEquals(13, func.apply(8, 12));
    assertEquals(8, func.apply(9, 31));
    assertEquals(0, func.apply(14, 22));
    assertEquals(8, func.apply(14, 31));
    assertEquals(13, func.apply(15, 12));
    // TODO: verify this unexpected functionality - the first half hour in the middle of day is free with the current specification
    for (int hour = 9; hour <= 14; hour++) {
      assertEquals(0, func.apply(hour, 21));
      assertEquals(8, func.apply(hour, 35));
    }
    assertEquals(18, func.apply(15, 34));
    assertEquals(18, func.apply(16, 15));
    assertEquals(18, func.apply(16, 35));
    assertEquals(13, func.apply(17, 39));
    assertEquals(8, func.apply(18, 11));
  }

  // Used to compare the default Tollfees with the initial specification
  static int getTollFeeHourOld(int hour, int minute) {
    if (hour == 6 && minute >= 0 && minute <= 29) return 8;
    else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
    else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
    else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
    else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
    else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
    else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
    else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
    else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
    else return 0;
  }

}