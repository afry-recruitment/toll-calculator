import java.time.*;
import java.util.*;

public class Toll {
    static int lowFee = 0;
    static int midLowFee = 8;
    static int midHighFee = 13;
    static int highFee = 18;

    //Map of start-times for toll-fees
    static Map<LocalTime, Integer> tollFeeMap = getTollFeeMap();

    //Array of MonthDay that is toll-free
    public static MonthDay[] tollFreeMonthDays = {
                    MonthDay.of(Month.JANUARY,1),
                    MonthDay.of(Month.MARCH,28),
                    MonthDay.of(Month.MARCH,29),
                    MonthDay.of(Month.APRIL,1),
                    MonthDay.of(Month.APRIL,30),
                    MonthDay.of(Month.MAY,1),
                    MonthDay.of(Month.MAY,8),
                    MonthDay.of(Month.MAY,9),
                    MonthDay.of(Month.JUNE,5),
                    MonthDay.of(Month.JUNE,6),
                    MonthDay.of(Month.JUNE,21),
                    MonthDay.of(Month.NOVEMBER,1),
                    MonthDay.of(Month.DECEMBER,24),
                    MonthDay.of(Month.DECEMBER,25),
                    MonthDay.of(Month.DECEMBER,26),
                    MonthDay.of(Month.DECEMBER,31),
            };

    static Month[] tollFreeMonths = {
            Month.JULY,
    };


    //Generates the tollFeeMap
    public static HashMap<LocalTime, Integer> getTollFeeMap(){
        HashMap<LocalTime, Integer> map = new HashMap<>();
        map.put(LocalTime.of(0,0), lowFee);
        map.put(LocalTime.of(6,0), midLowFee );
        map.put(LocalTime.of(6,30), midHighFee );
        map.put(LocalTime.of(7,0), highFee );
        map.put(LocalTime.of(8,0), midHighFee );
        map.put(LocalTime.of(8,30), midLowFee );
        map.put(LocalTime.of(15,0), midHighFee);
        map.put(LocalTime.of(15,30), highFee);
        map.put(LocalTime.of(17,0), midHighFee);
        map.put(LocalTime.of(18,0),midLowFee);
        map.put(LocalTime.of(18,30),lowFee);

        return map;
    }

    public static int getCost(LocalTime time){
        List<LocalTime> keys = new ArrayList<LocalTime>(tollFeeMap.keySet());
        Collections.sort(keys);
        int toll = 0;

        for (LocalTime key:keys) {
            int compNbr = time.compareTo(key);
            if(compNbr < 0){
                return toll;
            }
            toll = tollFeeMap.get(key);
        }
        return 0;
    }

    public static int getCost(Date date){
        LocalTime time = LocalDateTime.ofInstant(date.toInstant(),
                ZoneId.systemDefault()).toLocalTime();
        return getCost(time);
    }

    public static boolean isTollFreeDateMonthDay(MonthDay monthDay){
        return isTollFreeMonth(monthDay.getMonth()) || Arrays.asList(tollFreeMonthDays).contains(monthDay);
    }

    public static boolean isTollFreeMonth(Month month){
        return Arrays.asList(tollFreeMonths).contains(month);
    }


  }
