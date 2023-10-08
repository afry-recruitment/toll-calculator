import java.time.*;

public class TollCalculator {

    private static final int DAY_MAXIMUM_TOTAL_FEE = 60;

    private static int calculateAll(Vehicles vehicle, LocalDateTime... dates) {
        int total = calculateOne(vehicle, dates[0]);

        for (int i = 0; i < dates.length; i++) {
            LocalDateTime date = dates[i];
            int feeThisTrip = calculateOne(vehicle, date);
            long minutesSinceFirstTrip = Duration.between(dates[0], date).toMinutes();
            // check if trip is longer than 60s
            if (minutesSinceFirstTrip > 60) {
                // Full price
                total += feeThisTrip;
            } else if (feeThisTrip > total) {
                // If this trip is more expensive, add the difference
                total += (feeThisTrip - total);
            }
        }

        // return the lesser value
        return Math.min(total, DAY_MAXIMUM_TOTAL_FEE);
    }

    private static int calculateOne(Vehicles vehicle, LocalDateTime date) {
        if (VehicleChecker.isTollFree(vehicle) || DateChecker.isTollFree(date.toLocalDate())) {
            return 0;
        }
        return FeeTimeStampCalc.calculate(date.toLocalTime());
    }

    public static void main(String[] args) {

        LocalDateTime date1 = LocalDateTime.parse("2023-10-04T08:20:00");
        LocalDateTime date2 = LocalDateTime.parse("2023-10-04T09:00:00");
        // LocalDateTime date3 = LocalDateTime.parse("2023-10-04T10:30:00");

        int totalFee = calculateAll(Vehicles.CAR, date1, date2);
        // int totalFee = calculateAll(Vehicles.MOTORBIKE, date1, date2);

        System.out.println(totalFee + "kr");
    }

}
