package afry.tollcollector.configuration;

import afry.tollcollector.domain.Holiday;
import afry.tollcollector.domain.Holidays;
import afry.tollcollector.domain.HolidaysInYear;
import afry.tollcollector.domain.TollFee;
import afry.tollcollector.domain.TollFees;
import afry.tollcollector.domain.TollFreeVehicleTypes;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.util.Arrays;
import java.util.Calendar;

@Configuration
public class DefaultConfig {

  @Bean
  public Holidays defaultHolidays() {
    return new Holidays(defaultHolidaysIn2013(), defaultHolidaysIn2025());
  }

  @Bean
  public TollFees defaultTollFees() {
    return new TollFees(Arrays.asList(firstHalfHour(6, 8),
        secondHalfHour(6, 13),
        fullHour(7, 18),
        firstHalfHour(8, 13),
        secondHalfHour(8, 8),
        // TODO: should these be full hour?
        secondHalfHour(9, 8),
        secondHalfHour(10, 8),
        secondHalfHour(11, 8),
        secondHalfHour(12, 8),
        secondHalfHour(13, 8),
        secondHalfHour(14, 8),
        firstHalfHour(15, 13),
        secondHalfHour(15, 18),
        fullHour(16, 18),
        fullHour(17, 13),
        firstHalfHour(18, 8)
                                     ));
  }

  @Bean
  public TollFreeVehicleTypes defaultTollFreeVehicleTypes() {
    return new TollFreeVehicleTypes(Arrays.asList("Motorbike",
        "Tractor",
        "Emergency",
        "Diplomat",
        "Foreign",
        "Military"));
  }

  static TollFee firstHalfHour(int hour, int tollFee) {
    return new TollFee(hour, 0, 29, tollFee);
  }

  static TollFee secondHalfHour(int hour, int tollFee) {
    return new TollFee(hour, 30, 59, tollFee);
  }

  static TollFee fullHour(int hour, int tollFee) {
    return new TollFee(hour, 0, 59, tollFee);
  }

  static HolidaysInYear defaultHolidaysIn2013() {
    return new HolidaysInYear(2013, Arrays.asList(
        new Holiday(Calendar.JANUARY, 1),
        new Holiday(Calendar.MARCH, 28),
        new Holiday(Calendar.MARCH, 29),
        new Holiday(Calendar.APRIL, 1),
        new Holiday(Calendar.APRIL, 30),
        new Holiday(Calendar.MAY, 1),
        new Holiday(Calendar.MAY, 8),
        new Holiday(Calendar.MAY, 9),
        new Holiday(Calendar.JUNE, 5),
        new Holiday(Calendar.JUNE, 6),
        new Holiday(Calendar.JUNE, 21),
        new Holiday(Calendar.JULY, 1),
        new Holiday(Calendar.NOVEMBER, 1),
        new Holiday(Calendar.DECEMBER, 24),
        new Holiday(Calendar.DECEMBER, 25),
        new Holiday(Calendar.DECEMBER, 26)
                                                 ));
  }

  static HolidaysInYear defaultHolidaysIn2025() {
    return new HolidaysInYear(2025, Arrays.asList(
        new Holiday(Calendar.JANUARY, 1),
        new Holiday(Calendar.JANUARY, 6),
        new Holiday(Calendar.APRIL, 18),
        new Holiday(Calendar.APRIL, 20),
        new Holiday(Calendar.APRIL, 21),
        new Holiday(Calendar.MAY, 1),
        new Holiday(Calendar.MAY, 29),
        new Holiday(Calendar.JUNE, 6),
        new Holiday(Calendar.JUNE, 8),
        new Holiday(Calendar.JUNE, 21),
        new Holiday(Calendar.JULY, 1),
        new Holiday(Calendar.NOVEMBER, 1),
        new Holiday(Calendar.DECEMBER, 25),
        new Holiday(Calendar.DECEMBER, 26)
                                                 ));
  }
}
