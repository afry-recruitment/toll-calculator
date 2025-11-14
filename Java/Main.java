import java.time.LocalDateTime;
import java.time.Month;
import java.util.List;

public class Main {

    public static void main(String[] args) {
        TollCalculator calculator = new TollCalculator();

        // DIFFERENT vehicle types
        Vehicle car = new Vehicle(VehicleType.CAR); // not toll-free
        Vehicle motorcycle = new Vehicle(VehicleType.MOTORBIKE); // toll-free
        Vehicle emergency = new Vehicle(VehicleType.EMERGENCY); // toll-free
        Vehicle tractor = new Vehicle(VehicleType.TRACTOR); // not toll-free

        //4 passages on the same day, 2 passages on official holidays
        List<LocalDateTime> weekdayPassages = List.of(
                LocalDateTime.of(2025, Month.JANUARY, 14, 6, 15),  // 06:15
                LocalDateTime.of(2025, Month.JANUARY, 14, 6, 45),  // 06:45
                LocalDateTime.of(2025, Month.JANUARY, 14, 7, 15),  // 07:15
                LocalDateTime.of(2025, Month.JANUARY, 14, 15, 5)   // 15:05
        );

        //test weekday passages for car and motorbike
        int carWeekdayFee = calculator.getTollFee(car, weekdayPassages);
        int motorWeekdayFee = calculator.getTollFee(motorcycle, weekdayPassages);

        //should be non zero for car and zero for motorbike
        System.out.println("Car weekday toll: " + carWeekdayFee + " SEK");
        System.out.println("Motorbike weekday toll (toll-free): " + motorWeekdayFee + " SEK");

        //weekend example: should be 0 even for a non toll free vehicle
        List<LocalDateTime> weekendPassages = List.of(
                LocalDateTime.of(2013, Month.JANUARY, 12, 7, 30),  // Saturday
                LocalDateTime.of(2013, Month.JANUARY, 12, 8, 15)
        );

        //test weekend passages for car
        int carWeekendFee = calculator.getTollFee(car, weekendPassages);
        System.out.println("Car weekend toll: " + carWeekendFee + " SEK");

        //test emergency and tractor on weekday
        int emergencyWeekdayFee = calculator.getTollFee(emergency, weekdayPassages);
        int tractorWeekdayFee = calculator.getTollFee(tractor, weekdayPassages);

        //should be zero for emergency vehicles and non zero for tractor
        System.out.println("Emergency vehicle weekday toll (toll-free): " + emergencyWeekdayFee + " SEK");
        System.out.println("Tractor weekday toll: " + tractorWeekdayFee + " SEK");
    }
}
