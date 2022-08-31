package MyJava;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

public class Main {

  public static void main (String[] args) throws Exception {

    SimpleDateFormat sdf = new SimpleDateFormat ("yyyy-MM-dd HH:mm:ss");
    try {
      Date[] dates = {
        sdf.parse("2022-08-30 06:55:00"),
        sdf.parse("2022-08-30 08:10:00"),
        sdf.parse("2022-08-30 08:50:00"),
        sdf.parse("2022-08-30 10:10:00"),
        sdf.parse("2022-08-30 15:20:00"),
        sdf.parse("2022-08-30 15:55:00")
      };

    TollCalculator tollCalculator = new TollCalculator();
    // Vehicle vehicle = new Motorbike();
    Vehicle vehicle = new Car();
    int totalTollFee = tollCalculator.getTollFeeTotal(vehicle, dates);
    System.out.println("Total tollfee= " + totalTollFee);

    } catch (ParseException e) {
        e.printStackTrace();
    }
  }
}
