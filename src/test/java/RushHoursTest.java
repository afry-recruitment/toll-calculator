import model.Car;
import model.Motorbike;

import org.junit.Test;
import service.RushHoursHub;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

import static org.junit.Assert.assertEquals;

public class RushHoursTest {

    private RushHoursHub rushHoursHub = new RushHoursHub();

    private  Car car = new Car();

    @Test
    public void given_time_06_00_it_should_return_price_8() {


        LocalDateTime date = prepareTest("2013-08-29 06:00");

        int result = rushHoursHub.getTollFeeAtPeakTimesCalculus(date, car);

        assertEquals(8, result);
    }

    @Test
    public void given_time_06_30_it_should_return_price_13() {


        LocalDateTime date = prepareTest("2013-08-29 06:30");

        int result = rushHoursHub.getTollFeeAtPeakTimesCalculus(date, car);

        assertEquals(13, result);
    }
    @Test
    public void given_time_07_00_it_should_return_price_18() {

        LocalDateTime date = prepareTest("2013-08-29 07:00");

        int result = rushHoursHub.getTollFeeAtPeakTimesCalculus(date, car);

        assertEquals(18, result);
    }
    @Test
    public void given_time_08_00_it_should_return_price_13() {

        LocalDateTime date = prepareTest("2013-08-29 08:00");

        int result = rushHoursHub.getTollFeeAtPeakTimesCalculus(date, car);

        assertEquals(13, result);
    }
    @Test
    public void given_time_08_30_it_should_return_price_8() {

        LocalDateTime date = prepareTest("2013-08-29 08:30");

        int result = rushHoursHub.getTollFeeAtPeakTimesCalculus(date, car);

        assertEquals(8, result);
    }
    @Test
    public void given_time_15_00_it_should_return_price_13() {

        LocalDateTime date = prepareTest("2013-08-29 15:00");

        int result = rushHoursHub.getTollFeeAtPeakTimesCalculus(date, car);

        assertEquals(13, result);
    }
    @Test
    public void given_time_15_30_it_should_return_price_18() {

        LocalDateTime date = prepareTest("2013-08-29 15:30");

        int result = rushHoursHub.getTollFeeAtPeakTimesCalculus(date, car);

        assertEquals(18, result);
    }
    @Test
    public void given_time_17_00_it_should_return_price_13() {

        LocalDateTime date = prepareTest("2013-08-29 17:00");

        int result = rushHoursHub.getTollFeeAtPeakTimesCalculus(date, car);

        assertEquals(13, result);
    }


    private LocalDateTime prepareTest(String date) {

        LocalDateTime ldt = LocalDateTime.parse(date, DateTimeFormatter.ofPattern("uuuu-MM-dd HH:mm"));
        return ldt;
    }

}
