import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.ZoneId;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.Random;

public class Main {
    public static void main(String[] args){
        run();
    }

    private static void run() {
        for (int i = 0; i <100 ; i++) {
            testTollCalculator();
        }
    }

    static void testTollCalculator(){
        TollCalculator tc = new TollCalculator();
        Car c1 = new Car();
        Motorbike m1 = new Motorbike();
        Date[] dates = new Date[0];
        Date[] dates2 = getRandomDates(15);

        int result = tc.getTollFee(c1,dates);
        int result2 = tc.getTollFee(c1,dates2);
        //int result2 = tc.getTollFee(c1,null);
        System.out.println("Result1: " + result);
        System.out.println("Resuklt2: " + result2);

        System.out.println();
    }

    private static Date[] getRandomDates(int amount) {
        ArrayList<Date> dates = new ArrayList<Date>();
        Random rnd = new Random();
        for (int i = 0; i < amount; i++) {
            int year = rnd.nextInt(22)+2000+1;
            int month = rnd.nextInt(12)+1;
            int day = rnd.nextInt(28)+1;
            int hour = rnd.nextInt(24);
            int minute = rnd.nextInt(60);
            LocalDate localDate = LocalDate.of(year, month, day);
            ZoneId zoneid = ZoneId.of("Europe/Stockholm");
            Date date = Date.from(localDate.atStartOfDay(zoneid).toInstant());
            date.setHours(hour);
            date.setMinutes(minute);
            dates.add(date);
        }
        Date[] d =  dates.toArray(new Date[dates.size()]);
        return d;
    }

    static void test() {
        //Testing time
//        LocalTime t  = LocalTime.now();
//        LocalTime t2  = LocalTime.now();
//        LocalTime t3  = LocalTime.of(12,15,0,0);
//        System.out.println(t);
//        System.out.println(t2);
//        System.out.println(t2.compareTo(t));
//        System.out.println(t.compareTo(t2));
//        System.out.println(t3);

//        Toll v = new Toll();
//        for (int i = 0; i < 24; i++) {
//            LocalTime t1 = LocalTime.of(i,1,0);
//            LocalTime t2 = LocalTime.of(i,31,0);
//            int toll = v.getCost(t1);
//            int toll2 = v.getCost(t2);
//
//            System.out.println(t1 +" " +toll);
//            System.out.println(t2 +" " +toll2);
//        }

    }
}
