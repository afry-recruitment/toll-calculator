import model.Car;
import service.RushHoursHub;

import java.time.LocalDateTime;
import java.time.Month;
import java.time.format.DateTimeFormatter;


public class App {

    public static void main(String[] args) {
        RushHoursHub rushHoursHub = new RushHoursHub();
        Car car = new Car();

        DateTimeFormatter format = DateTimeFormatter.ofPattern("HH:mm, dd MMM uuuu");

        String date = "2013-07-29 15:30";
        LocalDateTime ldt = LocalDateTime.parse(date, DateTimeFormatter.ofPattern("uuuu-MM-dd HH:mm"));
        System.out.println("LocalDateTime : " + format.format(ldt));
        

        int result = rushHoursHub.getTollFeeAtPeakTimesCalculus(ldt, car);
        System.out.println(result);
    }
}
