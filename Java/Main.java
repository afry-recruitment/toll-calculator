import Vehicles.Car;
import Vehicles.Motorbike;

import java.time.*;
import java.util.ArrayList;
import java.util.Date;
import java.util.Random;

public class Main {
    static Random rnd = new Random();
    static int timesToRun = 2;

    public static void main(String[] args){
        run();
    }

    private static void run() {
        for (int i = 0; i <timesToRun ; i++) {
            testTollCalculator();
        }
    }

    static void testTollCalculator(){
        TollCalculator tc = new TollCalculator();
        Car c1 = new Car();
        Motorbike m1 = new Motorbike();
        Date[] dates = getTollFreeDates();
        Date[] dates2 = getRandomDates(30);

        //Free dates
        int result = tc.getTollFee(c1,dates);
        //Free vehicle
        int result2 = tc.getTollFee(m1,dates2);
        //Tollable (probably)
        int result3 = tc.getTollFee(c1,dates2);
        dates2[2] = dates[2];
        dates2[3] = dates[3];
        //TOllable with same or smaller res than result3
        int result4 = tc.getTollFee(c1,dates2);
        //int result2 = tc.getTollFee(c1,null);
        System.out.println("Result1: " + result);
        System.out.println("Resuklt2: " + result2);
        System.out.println("Resuklt3: " + result3);
        System.out.println("Resuklt4: " + result4);

        System.out.println();
    }

    private static Date[] getTollFreeDates() {
        ArrayList<Date> dates = new ArrayList<Date>();
        for (MonthDay md:Toll.tollFreeMonthDays) {
            LocalDate localDate = LocalDate.of(getRandomYear(), md.getMonth(), md.getDayOfMonth());
            dates.add(getDateFromLocalDate(localDate, getRandomHour(), getRandomMinute()));
        }
        return dates.toArray(new Date[dates.size()]);
    }

    private static Date[] getRandomDates(int amount) {
        ArrayList<Date> dates = new ArrayList<Date>();
        for (int i = 0; i < amount; i++) {
            LocalDate localDate = LocalDate.of(getRandomYear(), getRandomMotnh(), getRandomDay());
            dates.add(getDateFromLocalDate(localDate, getRandomHour(),getRandomMinute()));
        }
        return dates.toArray(new Date[dates.size()]);
    }

    private static Date getDateFromLocalDate(LocalDate localDate, int hour,int minute) {
        ZoneId zoneid = ZoneId.of("Europe/Stockholm");
        Date date = Date.from(localDate.atStartOfDay(zoneid).toInstant());
        date.setHours(hour);
        date.setMinutes(minute);
        return date;
    }

    //TODO get rid of ghostnumbers?
    private static int getRandomMinute(){ return rnd.nextInt(60); }
    private static int getRandomHour(){ return rnd.nextInt(24); }
    private static int getRandomDay(){ return rnd.nextInt(28)+1;}
    private static int getRandomMotnh(){ return rnd.nextInt(12)+1;}
    private static int getRandomYear(){ return rnd.nextInt(22)+2000+1;}

}
