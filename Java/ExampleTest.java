import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.Month;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Random;
import java.util.concurrent.ThreadLocalRandom;

import Exception.CarCreatorException;
import Exception.DateException;
import Vehicles.Vehicle;
import Vehicles.VehicleCreator;

public class ExampleTest {

    LocalDate endDate;
    LocalDate startDate;

    // the test we create and run
    public void runTest() throws ParseException, CarCreatorException, DateException {
        System.out.println("Initiating test!");
        System.out.println("-------");

        /*
        create an instance of the toll calculator, for "real life" data fetching we could
        read from a file or a database
        */ 
        TollCalculator toll = new TollCalculator();
        startDate = LocalDate.of(toll.calcYear, Month.JANUARY, 1);
        endDate = LocalDate.of(toll.calcYear, Month.DECEMBER, 31);

        // create a vehicle of each type with different registrations
        VehicleCreator vehicleCreator = new VehicleCreator();
        ArrayList<Vehicle> vehiclesList = new ArrayList<Vehicle>();
        String[] vehicleTypes = {"Car", "Diplomat", "Emergency", "Foreign", "Military", "Motorbike", "Tractor"};
        for (int i = 0; i < vehicleTypes.length; i++) {

            String registration = "'HAX 00"+ i + "'";

            Vehicle tempVehicle = vehicleCreator.createVehicle(vehicleTypes[i], registration);
            vehiclesList.add(tempVehicle);
        }

        // print how many vehicles
        System.out.println("A total of " + vehiclesList.size() + " vehicles");
        System.out.println("-------");

        // create a date variable for parsing dates into the correct format
        SimpleDateFormat date = new SimpleDateFormat("yyyy-MM-dd HH:mm");
        
        /* 
        add random days and times for our test as well as calculate the toll
        we print it to the terminal but for real life application we could write
        to a file or a database
        */
        for (int i = 0; i < vehiclesList.size(); i++) {
            System.out.println(vehiclesList.get(i).getType() + " with registration " + vehiclesList.get(i).getRegistration() + " passed on the times: ");                        
            String randomDate = getRandomDate(startDate, endDate);

            java.util.Date[] dates = 
                {
                date.parse(randomDate + " " + getRandomTime()),
                date.parse(randomDate + " " + getRandomTime()),
                date.parse(randomDate + " " + getRandomTime()),
                date.parse(randomDate + " " + getRandomTime()),
                date.parse(randomDate + " " + getRandomTime()),
                date.parse(randomDate + " " + getRandomTime()),
                date.parse(randomDate + " " + getRandomTime()),
                date.parse(randomDate + " " + getRandomTime()),
                date.parse(randomDate + " " + getRandomTime())
                };
            Arrays.sort(dates);
            for (int di = 0; di < dates.length; di++){
                System.out.println(dates[di]);
            }

            int totalToll = toll.getTollFee(vehiclesList.get(i), dates);
            System.out.println("With a total toll of: " + totalToll + " kr");
            System.out.println("-------");
            }
        }

    // Get random date for testing purposes
    public static String getRandomDate(LocalDate startInclusive, LocalDate endExclusive) {
        long startEpochDay = startInclusive.toEpochDay();
        long endEpochDay = endExclusive.toEpochDay();
        long randomDay = ThreadLocalRandom
          .current()
          .nextLong(startEpochDay, endEpochDay);
    
        String date = "" + LocalDate.ofEpochDay(randomDay);
        return date;
    }

    // Get random time for testing purposes
    public static String getRandomTime() {
        Random rand = new Random();

        // random hours and minutes
        // there's probably a better way in the java library
        int HH = rand.nextInt(24);
        String stringHH = ""+ HH;
        int MM = rand.nextInt(60);
        String stringMM = "" + MM;

        if (HH < 10)
            stringHH = "0"+ HH;
        if (MM < 10)
            stringMM = "0" + MM;

        return stringHH + ":" + stringMM;
    }
}
