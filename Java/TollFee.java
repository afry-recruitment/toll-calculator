import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * TollFee
 */
public class TollFee {

    public static void main(String[] args) throws Exception {

        TollCalculator tollCalculator = new TollCalculator();
        Vehicle vehicle = new Car();
        // Vehicle vehicle = new Motorbike();

        // Sample Test data for 2022-09-12 (Monday)
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat ("yyyy-MM-dd HH:mm:ss");
        try {
            Date[] dates = {
                simpleDateFormat.parse("2022-09-12 05:05:05"),
                simpleDateFormat.parse("2022-09-12 06:06:06")
            };
      
            int totalTollFee = tollCalculator.getTollFee(vehicle, dates);
            System.out.println("Total toll fee = " + totalTollFee); 
      
          } catch (ParseException e) {
              e.printStackTrace();
          }
    }
}