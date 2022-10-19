import java.time.LocalTime;
import java.util.Calendar;
import java.util.Date;
import java.time.LocalDateTime;
import java.util.GregorianCalendar;

public class Main {
    public static void main(String[] args){
        run();
    }

    private static void run() {
        TollCalculator tc = new TollCalculator();
        Car c1 = new Car();
        Motorbike m1 = new Motorbike();
        Date[] dates = new Date[0];
        int result = tc.getTollFee(c1,dates);
        //int result2 = tc.getTollFee(c1,null);
        System.out.println(result);

        System.out.println();
        test();
    }

    static void test() {
        //Testing time
        LocalTime t  = LocalTime.now();
        LocalTime t2  = LocalTime.now();
        LocalTime t3  = LocalTime.of(12,15,0,0);
        System.out.println(t);
        System.out.println(t2);
        System.out.println(t2.compareTo(t));
        System.out.println(t.compareTo(t2));
        System.out.println(t3);



    }
}
