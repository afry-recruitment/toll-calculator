import Vehicles.Car;
import Vehicles.Motorbike;

import java.util.Date;

public class Main {
    static int timesToRun = 3;

    /**
     * Our main method. Used to do some simple tests and runs of TollCalculator.
     * @param args The command line arguments.
     **/
    public static void main(String[] args){
        run();
        System.out.println("Errors in dateGenerator: " + DateGenerator.errorcounter);
    }

    /**
     * This method runs the testCalculator x times.
     */
    private static void run() {
        for (int i = 0; i <timesToRun ; i++) {
            testTollCalculator();
        }
    }

    /**
     * This method tests TollCalculator by running it with a Vehicle and a set of dates.
     * This is not real tests ass it doesn't assert the right values are returned, but is
     * simple tryouts to check if the tollCalculator runs without error,
     * and we can see if the output is somewhat inline with what we expect it to be.
     * Edit this freely.
     *
     */
    static void testTollCalculator(){
        TollCalculator tc = new TollCalculator();
        Car c1 = new Car();
        Motorbike m1 = new Motorbike();
        Date[] dates = DateGenerator.getTollFreeDates();
        Date[] dates2 = DateGenerator.getRandomDates(30);

        //Free dates
        int result = tc.getTollFee(c1,dates);
        //Free vehicle
        int result2 = tc.getTollFee(m1,dates2);

        //Tollable (probably)
        int result3 = tc.getTollFee(c1,dates2);

        //Tollable with same or smaller res than result3... Arrs must include at least 4 dates otherwise fail...
        dates2[2] = dates[2];
        dates2[3] = dates[3];
        int result4 = tc.getTollFee(c1,dates2);

        System.out.println("Result1: " + result);
        System.out.println("Result2: " + result2);
        System.out.println("Result3: " + result3);
        System.out.println("Result4: " + result4);
        System.out.println();
    }


}
