import model.Car;
import service.RushHoursHub;

import java.time.LocalDateTime;
import java.time.Month;

public class App {

    public static void main(String[] args) {
        RushHoursHub rushHoursHub = new RushHoursHub();
        Car car = new Car();
        // unit test
        LocalDateTime newDate = LocalDateTime.of(2013, Month.JULY, 29, 7, 00);

        int result = rushHoursHub.getTollFeeAtPeakTimesCalculus(newDate, car);
        System.out.println(result);
    }
}
