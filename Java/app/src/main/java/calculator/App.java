/*
 * This Java source file was generated by the Gradle 'init' task.
 */
package calculator;

import calculator.calendar.CalendarRegion;
import calculator.calendar.CalendarService;
import calculator.vehicles.VehicleType;

import java.time.Instant;
import java.time.temporal.ChronoUnit;
import java.util.Date;

public class App
{

    public App(String[] args)
    {
        // todo handle args
        start("SWEDISH");
    }

    void start(String holidayRegion)
    {
//        CalendarService calendarService = new CalendarService(CalendarRegion.valueOf(holidayRegion));
//        System.out.println(calendarService.getHolidays());
        TollCalculator calculator = new TollCalculator();
        Date[]dates = new Date[]{Date.from(Instant.now()),Date.from(Instant.now().plus(10,
                                                                                       ChronoUnit.HOURS)),
                                 Date.from(Instant.now().plus(11, ChronoUnit.HOURS))};
        System.out.println(calculator.getTollFee(VehicleType.CAR, dates));
    }

    private App()
    {
    }

    public static void main(String[] args)
    {
        //        System.out.println(new App().getGreeting());
        new App(args);
    }
}
