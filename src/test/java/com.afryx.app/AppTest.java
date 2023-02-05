package com.afryx.app;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Collections;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.List;
import org.junit.Test;
import org.junit.runners.Parameterized.Parameters;
import java.util.Collection;
import org.junit.runner.RunWith;
import org.junit.runners.Parameterized;

@RunWith(Parameterized.class)
public class AppTest {

  private final TollCalculator tollCalculator;
  private final Vehicle car;
  private final Vehicle bike;
  private final Vehicle military;

  public AppTest(TestData data) {
    this.data = data;
    this.tollCalculator = new TollCalculator();
    this.car = new Car();
    this.bike = new Motorbike();
    this.military = new Military();
  }

  private record TestData(Date[] dates, int expected){}

  @Parameters(name = "data")
  public static Collection<TestData> data() {
    // List of all date-arrays + expected tools to test
    List<TestData> returnList = new ArrayList<>();
    List<Date> d = new ArrayList<>();
    var g = new GregorianCalendar(2023, Calendar.JANUARY, 2, 5, 0);

    // Never tolls this early
    d.add(g.getTime());
    returnList.add(new TestData(d.toArray(new Date[0]), 0));

    // 8kr
    g.add(Calendar.HOUR_OF_DAY, 1);
    d.add(g.getTime());
    returnList.add(new TestData(d.toArray(new Date[0]), 8));

    // Still 8kr, even if passed multiple times
    d.add(g.getTime());
    d.add(g.getTime());
    returnList.add(new TestData(d.toArray(new Date[0]), 8));

    // Now 13 for this hour
    g.add(Calendar.MINUTE, 30);
    d.add(g.getTime());
    returnList.add(new TestData(d.toArray(new Date[0]), 13));

    // Now 13 + 18 at 07:00
    g.add(Calendar.MINUTE, 30);
    d.add(g.getTime());
    returnList.add(new TestData(d.toArray(new Date[0]), 31));

    // Pass both before and after 8:30, 31 + 13
    g.add(Calendar.HOUR_OF_DAY,1);
    d.add(g.getTime()); // 13kr
    g.add(Calendar.MINUTE, 30);
    d.add(g.getTime()); // 8kr, "ignored"
    returnList.add(new TestData(d.toArray(new Date[0]), 44));

    // Pass at 17:00, stay right below 60kr total, 44+13
    g.set(Calendar.HOUR_OF_DAY,17);
    g.set(Calendar.MINUTE, 0);
    d.add(g.getTime());
    returnList.add(new TestData(d.toArray(new Date[0]), 57));

    // Now will remain at 60 after passing again at 18:29
    g.add(Calendar.HOUR_OF_DAY,1);
    g.add(Calendar.MINUTE, 29);
    d.add(g.getTime());
    returnList.add(new TestData(d.toArray(new Date[0]), 60));
    return returnList;
  }

  private final TestData data;

  /**
   * Tests a variety of dates on a regular toll day for a normal car.
   */
  @Test
  public void testCarToll() {
    assert tollCalculator.getTollFee(car, data.dates) == data.expected;
  }

  /**
   * Ensure if dates from different days are given, returns zero always.
   */
  @Test
  public void testDifferentDays(){
    var g = new GregorianCalendar(2023, Calendar.JANUARY, 2, 8, 0);
    List<Date> d = new ArrayList<>();
    d.add(g.getTime());
    g.add(Calendar.DAY_OF_MONTH, 1);
    d.add(g.getTime());
    assert tollCalculator.getTollFee(car, d.toArray(new Date[0])) == 0;
  }

  /**
   * Ensure proper function in the case the dates have mixed ordering.
   */
  @Test
  public void testWrongOrderDates(){
    var temp = Arrays.asList(data.dates.clone());
    Collections.shuffle(temp);
    assert tollCalculator.getTollFee(car,temp.toArray(new Date[0])) == data.expected;

  }

  /**
   * Test some special exempted days at which tolls are free.
   */
  @Test
  public void testCarTollExemptedDays() {
    var g = new GregorianCalendar(2023, Calendar.JANUARY, 5, 8, 0);

    // Special day
    assert tollCalculator.getTollFee(car, g.getTime()) == 0;
    // Red-day, Sunday
    g.set(Calendar.DAY_OF_MONTH, 8);
    assert tollCalculator.getTollFee(car, g.getTime()) == 0;
    // No fee in July
    g.set(Calendar.MONTH, Calendar.JULY);
    assert tollCalculator.getTollFee(car, g.getTime()) == 0;
  }

  /**
   * Exempted vehicle type, always free.
   */
  @Test
  public void testMotorBikeToll() {
    assert tollCalculator.getTollFee(bike, data.dates) == 0;
  }

  /**
   * Exempted vehicle type, always free.
   */
  @Test
  public void testMilitaryToll() {
    assert tollCalculator.getTollFee(military, data.dates) == 0;
  }
}
