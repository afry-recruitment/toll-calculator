import model.Car;
import service.RushHoursHub;

import java.time.LocalDateTime;
import java.time.Month;

public class App {

    public static void main(String[] args) {
        RushHoursHub rushHoursHub = new RushHoursHub();
        Car car = new Car();

        LocalDateTime newDate = LocalDateTime.of(2015, Month.JULY, 29, 6, 00);

        int result = rushHoursHub.getTollFeeAtPeakTimesCalculus(newDate, car);
        System.out.println(result);
    }
}
