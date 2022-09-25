import com.toll.manager.TollCalculator;
import com.toll.model.Car;
import com.toll.model.Vehicle;
import java.io.IOException;
import java.time.LocalDateTime;

public class TestTollCalculator {
    public static void main(String[] args) throws IOException {
        TollCalculator a = new TollCalculator(2022);

        Vehicle car = new Car();

        LocalDateTime[] list = {LocalDateTime.parse("2022-09-26T07:50:00"),//normal working day
                //LocalDateTime.parse("2022-12-26T07:50:00"),//holiday date
                //  LocalDateTime.parse("2022-09-25T07:59:00"),//sunday(weekend)
                    LocalDateTime.parse("2022-09-26T08:28:28")//normal working day
                  };
        System.out.println(a.getTotalDailyFee(car, list));

    }


}
