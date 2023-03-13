import java.util.ArrayList;
import java.util.Date;
import java.util.List;

// Hello, I put around 8 hours on modifying this project. 

/* Long time since I coded in java but I created a starting point with the main function in this file */
public class App {
    public static void main(String[] args) throws Exception {
        // Creating the toll calculator object
        TollCalculator tollCalculator = new TollCalculator();

        /*
         * Creating a regular car object. In the real world the type is being fetched
         * from the database.
         */
        Car car = new Car("Regular");

        /*
         * Creating an example list of timestamps. Read from the database as well in a
         * real live environment.
         */
        List<Date> timestamps = new ArrayList<>();
        timestamps.add(new Date(1678455409779L));
        timestamps.add(new Date(1678455409779L + 3500000));
        timestamps.add(new Date(1678455409779L + 3700000));
        timestamps.add(new Date(1678455409779L + 7200000));

        // Calculating the bill and print it
        int sumForBill = tollCalculator.getTollFee(car, timestamps);
        System.out.println(sumForBill);

    };

}
