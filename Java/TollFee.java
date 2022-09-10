import java.util.Date;

/**
 * TollFee - Where user can pass the vehicle type and it's parking sessions to calculate the fee
 */
public class TollFee {

    private static final String TOTAL = "Total toll fee = ";

    public static void main(String[] args) throws Exception {

        TollCalculator tollCalculator = new TollCalculator();
        Vehicle vehicle = new Car();
        // Vehicle vehicle = new Motorbike();

        Date[] dates = new SampleData().getParkingSessions();

        if (dates.length > 0) {
            int totalTollFee = tollCalculator.getTollFee(vehicle, dates);
            System.out.println(TOTAL + totalTollFee); 
        } else {
            System.out.println(TOTAL + 0); 
        }
    }
}