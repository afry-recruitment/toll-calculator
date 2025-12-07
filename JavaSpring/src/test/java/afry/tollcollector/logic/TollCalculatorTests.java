package afry.tollcollector.logic;

import afry.tollcollector.domain.Car;
import afry.tollcollector.domain.Motorbike;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.util.Calendar;
import java.util.Date;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.fail;

@SpringBootTest
public class TollCalculatorTests {

  @Autowired
  private TollCalculator tollCalculator;

  @Test
  public void checkTollsWithNullVehicle() {
    int tollFee = tollCalculator.getTollFee(null, createDate(Calendar.FEBRUARY, 12, 7, 38));
    assertEquals(18, tollFee);
  }

  @Test
  public void checkTwoTollsSameHourDuringRushHourRegularDay() {
    int tollFee = tollCalculator.getTollFee(new Car(),
        createDate(Calendar.FEBRUARY, 12, 7, 35),
        createDate(Calendar.FEBRUARY, 12, 7, 45));
    assertEquals(18, tollFee);
  }

  @Test
  public void checkTwoTollsSameHourDuringSplitHourRegularDay() {
    int tollFee = tollCalculator.getTollFee(new Car(),
        createDate(Calendar.FEBRUARY, 12, 8, 5),
        createDate(Calendar.FEBRUARY, 12, 8, 45));
    assertEquals(13, tollFee);
  }

  @Test
  public void checkMultipleTollsDuringRegularDay() {
    int tollFee = tollCalculator.getTollFee(new Car(),
        createDate(Calendar.FEBRUARY, 12, 7, 35),
        createDate(Calendar.FEBRUARY, 12, 7, 45),
        // 18
        createDate(Calendar.FEBRUARY, 12, 8, 5),
        createDate(Calendar.FEBRUARY, 12, 8, 45),
        // 13
        createDate(Calendar.FEBRUARY, 12, 12, 47),
        // 8
        createDate(Calendar.FEBRUARY, 12, 17, 35)
        // 13
                                           );
    assertEquals(18 + 13 + 8 + 13, tollFee);
  }

  @Test
  public void checkMultipleTollsDuringRegularDayExceedingMaximum() {
    int tollFee = tollCalculator.getTollFee(new Car(),
        createDate(Calendar.FEBRUARY, 12, 7, 32),
        // 18
        createDate(Calendar.FEBRUARY, 12, 8, 49),
        // 13
        createDate(Calendar.FEBRUARY, 12, 12, 31),
        // 8
        createDate(Calendar.FEBRUARY, 12, 13, 47),
        // 8
        createDate(Calendar.FEBRUARY, 12, 14, 47),
        // 8
        createDate(Calendar.FEBRUARY, 12, 17, 35)
        // 13
                                           );
    assertEquals(60, tollFee);
  }

  @Test
  public void checkMultipleZeroTollsDuringRegularDay() {
    int tollFee = tollCalculator.getTollFee(new Car(),
        createDate(Calendar.FEBRUARY, 12, 5, 32),
        // 0
        createDate(Calendar.FEBRUARY, 12, 9, 12),
        // 0
        createDate(Calendar.FEBRUARY, 12, 10, 1),
        createDate(Calendar.FEBRUARY, 12, 10, 29),
        // 0
        createDate(Calendar.FEBRUARY, 12, 13, 19),
        // 0
        createDate(Calendar.FEBRUARY, 12, 14, 27),
        // 0
        createDate(Calendar.FEBRUARY, 12, 18, 35)
        // 0
                                           );
    assertEquals(0, tollFee);
  }

  @Test
  public void checkTwoTollsSameHourDuringRushHourRegularDayFreeVehicle() {
    int tollFee =
        tollCalculator.getTollFee(
            new Motorbike(),
            createDate(Calendar.FEBRUARY, 12, 7, 35),
            createDate(Calendar.FEBRUARY, 12, 7, 45));
    assertEquals(0, tollFee);
  }

  @Test
  public void checkTwoTollsSameHourDuringRushHourWeekend() {
    int tollFee = tollCalculator.getTollFee(new Car(),
        createDate(Calendar.FEBRUARY, 9, 7, 21),
        createDate(Calendar.FEBRUARY, 9, 7, 55));
    assertEquals(0, tollFee);
  }

  private static Date createDate(int month, int day, int hour, int minute) {
    Calendar calendar =  Calendar.getInstance();
    calendar.set(2013, month, day, hour, minute);
    return calendar.getTime();
  }

}
