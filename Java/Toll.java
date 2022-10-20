import java.time.LocalDate;
import java.time.LocalTime;
import java.time.Month;
import java.time.MonthDay;
import java.util.*;


/***  if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;

            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;

            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;

            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
            ***/

/***
 *     if (year == 2013) {
 *       if (month == Calendar.JANUARY && day == 1 ||
 *           month == Calendar.MARCH && (day == 28 || day == 29) ||
 *           month == Calendar.APRIL && (day == 1 || day == 30) ||
 *           month == Calendar.MAY && (day == 1 || day == 8 || day == 9) ||
 *           month == Calendar.JUNE && (day == 5 || day == 6 || day == 21) ||
 *           month == Calendar.JULY ||
 *           month == Calendar.NOVEMBER && day == 1 ||
 *           month == Calendar.DECEMBER && (day == 24 || day == 25 || day == 26 || day == 31)) {
 *         return true;
 *       }
 *     }
 */

public class Toll {
    static int lowFee = 0;
    static int midLowFee = 8;
    static int midHighFee = 13;
    static int highFee = 18;

    //Map of start-times for toll-fees
    static Map<LocalTime, Integer> tollFeeMap = getTollFeeMap();
    //Array of MonthDay that is toll-free
    static MonthDay[] tollFreeMonthDays = {
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

    public static boolean isTollFreeDateMonthDay(MonthDay monthDay){
        return isTollFreeMonth(monthDay.getMonth()) || Arrays.asList(tollFreeMonthDays).contains(monthDay);
    }
    public static boolean isTollFreeMonth(Month month){
        return Arrays.asList(tollFreeMonths).contains(month);
    }


  }
