import java.time.LocalDate;
import java.time.MonthDay;
import java.time.ZoneId;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.Random;

public class DateGenerator {
    /**randomizer for class*/
    static Random rnd = new Random();
    /**Lowest year to return from random year generator*/
    static int lowestYear = 2000;
    /**Highest year to return from random year generator*/
    static int highestYear = 2022;

    /**just to check whether there is a problem with date generator*/
    static int errorcounter = 0;

    /**
     * Returns tollfree dates from the Toll class.
     *
     * @return Date[] - All dates that are tollfree
     */
    public static Date[] getTollFreeDates() {
        ArrayList<Date> dates = new ArrayList<Date>();
        for (MonthDay md:Toll.tollFreeMonthDays) {
            LocalDate localDate = LocalDate.of(getRandomYear(), md.getMonth(), md.getDayOfMonth());
            dates.add(getDateFromLocalDate(localDate, getRandomHour(), getRandomMinute()));
        }
        return dates.toArray(new Date[dates.size()]);
    }

    /**
     *Generates an arry with random dates.
     *
     * @param amount the number of random dates to generate
     * @return Date[] - The random dates
     */
    public static Date[] getRandomDates(int amount) {
        ArrayList<Date> dates = new ArrayList<Date>();
        for (int i = 0; i < amount; i++) {
            try{
                int year = getRandomYear();
                int month = getRandomMonth();
                int day = getRandomDay(month,year);
                LocalDate localDate = LocalDate.of(year, month, day);
                dates.add(getDateFromLocalDate(localDate, getRandomHour(),getRandomMinute()));

            }catch (Exception e)
            {
                i--;
                System.out.println("Error in getrandom dates:" );
                e.printStackTrace();
            }
        }
        return dates.toArray(new Date[dates.size()]);
    }

    /**
     * Converts a DocalDate, hour and minute to a Date.
     *
     * @param localDate - the LocalDate
     * @param hour - the hour
     * @param minute - the minute
     * @return Date - the date corresponding to params
     */
    private static Date getDateFromLocalDate(LocalDate localDate, int hour,int minute) {
        ZoneId zoneid = ZoneId.of("Europe/Stockholm");
        Date date = Date.from(localDate.atStartOfDay(zoneid).toInstant());
        date.setHours(hour);
        date.setMinutes(minute);
        return date;
    }

    /**
     * Generates a random minute in span 0-59
     * @return int - the minute
     */
    private static int getRandomMinute(){ return rnd.nextInt(60); }

    /**
     * Generates a random hour in span 0-23
     * @return int - the hour
     */
    private static int getRandomHour(){ return rnd.nextInt(24); }

    /**
     *Generates a random day in the year and month provided
     *
     * @param month - the month in which to generate day
     * @param year - the year in which to generate day
     * @return int - the day
     */
    private static int getRandomDay(int month, int year){
        //Get days for that month
        int days = DaysInMonth.values()[month-1].getDays();
        //Check if extra day in month apply
        if(month == Calendar.FEBRUARY + 1/*indexing diff*/ && isLongYear(year)){
            days +=1;
        }
        return rnd.nextInt(days)+1;
    } //+ 1 because we want lowest value to be 1

    /**
     * Checks if year has an extra day, every year that can be divided by 4 without remainder
     * @param year
     * @return boolean - true if year has extra day. Otherwise false.
     */
    private static boolean isLongYear(int year) {
        float remainder = (float) year / 4;
        return remainder % 1 == 0;
    }

    /**
     * Gets a random month in range 1-12
     * @return int - the month
     */
    private static int getRandomMonth(){ return rnd.nextInt(12)+1;} //+ 1 because we want lowest value to be 1

    /**
     * redurns random year in range lowestYear - highestYear
     * @return int - the random year.
     */
    private static int getRandomYear(){ return rnd.nextInt(highestYear -lowestYear + 1)+lowestYear;} //+1 to include highest value in the possible outcomes

    /**
     *  Month with days
     */
    public enum DaysInMonth {
        January(31),
        February(28),
        March(31),
        April(30),
        May(31),
        June(30),
        July(31),
        August(31),
        September(30),
        October(31),
        November(30),
        December(31);

        public final int days;

        DaysInMonth(int days) {
            this.days = days;
        }

        public int getDays(){
            return days;
        }
    }
}
