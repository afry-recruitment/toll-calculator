
import Vehicles.Vehicle;

import java.time.LocalDate;
import java.time.MonthDay;
import java.util.*;

public class TollCalculator {

  Calendar gregorianCalendar = GregorianCalendar.getInstance();
  /**Offset for converting Date into localdate*/
  final int monthOffset = 1;
  final int secondsInAMinute = 60;
  final int secondsInAnHour = 3600;
  /**Maxfee for one day*/
  final int dayMaxFee = 60;


  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return int - the total toll fee for that day
   */
  public int getTollFee(Vehicle vehicle, Date... dates) {

    //Check for null pointers
    if(vehicle == null){
      throw new NullPointerException("vehicle cannot be null");
    }

    if(dates == null){
      throw new NullPointerException("dates cannot be null");
    }

    //Check if needed to run at all
    if(!tollable(vehicle, dates)){
      return 0;
    }

    //Remove free dates as they are not to be counted
    dates = removeTollFreeDates(dates);

    //Sort dates into sets of days
    HashMap<LocalDate, ArrayList<Date>> datesByDay = getDaysHashmap(dates);

    //Doing the toll calculations
    int totalToll = 0;

    for (LocalDate day:datesByDay.keySet()) {
      //Sort day into segments spanning 60 minutes
      Date[][] daySegmentedByHour = getSegmentedDay(datesByDay.get(day), secondsInAnHour);

      int tollForDay = 0;

      //Get max fee for each segment and add to daily toll
      for (Date[] segment:daySegmentedByHour) {
        tollForDay += getMaxTollFeeAmongDates(segment); //We could break if tollForADay >=dayMaxFee since it´s the limit
      }
      if(tollForDay > dayMaxFee){
        tollForDay = dayMaxFee;
      }
      totalToll += tollForDay;
    }
    return totalToll;
  }

  /**
   * Removes the tollfree dates from an array of dates
   *
   * @param dates - the dates to clear from tollfree dates
   * @return Date[] - a datearray without tollfree dates
   */
  private Date[] removeTollFreeDates(Date[] dates) {
    ArrayList<Date> result = new ArrayList<>();
    for (Date date:dates) {
      if(!isTollFreeDate(date)){
        result.add(date);
      }
    }
    Date[] resArr = result.toArray(new Date[0]);
    return resArr;
  }

  /**
   * Returns the highest fee that can be tolled from dates
   *
   * @param dates - the dates to find highest toll value from
   * @return int - the highest toll from one date in the dates array
   */
  private int getMaxTollFeeAmongDates(Date[] dates) {
    int toll = 0;
    for (Date d:dates) {
      int fee = Toll.getFee(d);
      if(fee>toll){
        toll = fee;
      }
    }
    return toll;
  }

  /**
   * Segments Dates of one day into chunks spanning over param:segmentLimitSeconds seconds
   *
   * @param dates - the dates that should be segmented
   * @param segmentLimitSeconds - the limit in seconds on how large time a segment can span
   * @return Date[][] - the day segmented into groups spanning over segmentLimitSeconds time ([group][individualValue])
   */
  private Date[][] getSegmentedDay(ArrayList<Date> dates, int segmentLimitSeconds) {

    //Sort to easily loop through
    dates.sort((d1, d2) -> d1.compareTo(d2));

    //Init segmentation variables
    HashMap<Integer, ArrayList<Date>> segmentedDay = new HashMap<>();
    int counter = 0;
    segmentedDay.put(counter, new ArrayList<Date>());
    ArrayList<Date> segment = new ArrayList<>();
    Date segmentBase = dates.get(0);

    //Do segmentation
    for (Date date:dates) {
      long diffSeconds = (date.getTime()-segmentBase.getTime()) / 1000; //divide by 1000 to get rid of milli seconds
      if(diffSeconds< segmentLimitSeconds){
        segmentedDay.get(counter).add(date);
      }else {
        segmentedDay.put(++counter, new ArrayList<Date>(Arrays.asList(date)));
        segmentBase = date;
      }
    }

    //Convert do 2d Date array
    int lenght = segmentedDay.size();
    Date[][] result = new Date[lenght][];
    for (int i = 0; i < lenght; i++) {
      result[i] = segmentedDay.get(i).toArray(new Date[0]);
    }
    return result;
  }

  /**
   * Groups dates into groups of same day and puts them into a map with the day as key
   *
   * @param dates - the dates that should be mapped into days
   * @return HashMap<LocalDate, ArrayList<Date>> - A list of dates for each unique day
   */
  private HashMap<LocalDate, ArrayList<Date>> getDaysHashmap(Date[] dates) {
    HashMap<LocalDate, ArrayList<Date>> dateMap = new HashMap<>();
    for (Date date:dates) {
      LocalDate localDay = getLocalDateWithOffset(date);
      if(dateMap.containsKey(localDay)){
        dateMap.get(localDay).add(date);
      }else{
        ArrayList aList = new ArrayList();
        aList.add(date);
        dateMap.put(localDay, aList);
      }
    }
    return dateMap;
  }

  /**
   * Takes a date and offset it's month so January is 1 instead of 0 and returns a LocalDate.
   *
   * @param date - the date to be used for the localDate
   * @return LocalDate - the local date of same date as parameter
   */
  private LocalDate getLocalDateWithOffset(Date date) {
    gregorianCalendar.setTime(date);
    int year = gregorianCalendar.get(Calendar.YEAR);
    int month = gregorianCalendar.get(Calendar.MONTH) + monthOffset;
    int day = gregorianCalendar.get(Calendar.DAY_OF_MONTH);
    return LocalDate.of(year, month, day);
  }

  /**
   *  Checks whether a vehicle and set of dates can be tolled.
   *
   * @param dates - the dates to check for tollability
   * @param vehicle - the vehicle on which to check if tollable
   * @return boolean - if vehicle and dates are toghether tollable
   */
  private boolean tollable(Vehicle vehicle, Date... dates) {
    //Check if vehicle not tollable
    if(isTollFreeVehicle(vehicle)){
      return false;
    }

    //Check if any dates are tollable
    for (Date date:dates) {
      if(!isTollFreeDate(date)){
        return true;
      }
    }
    return false;
  }

  /**
   *  Checks whether a vehicle is toll free
   *
   * @param vehicle - the vehicle on which to check if tollfree
   * @return boolean - tollfree vehicle
   */
  private boolean isTollFreeVehicle(Vehicle vehicle) {
    String vehicleType = vehicle.getType();
    for (TollFreeVehicles tf:TollFreeVehicles.values()) {
      if(vehicleType.equals(tf.getType())){
        return true;
      }
    }
    return false;
  }

  /**
   *  Checks whether a date is toll free
   *
   * @param date - the date to check if tollfree
   * @return boolean - tollfree date
   */
  private Boolean isTollFreeDate(Date date) {
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);

    //int year = calendar.get(Calendar.YEAR);
    int month = calendar.get(Calendar.MONTH) + 1; //Offset 1 to make January 1 instrad of 0
    int day = calendar.get(Calendar.DAY_OF_MONTH);

    //note Sunday = 1 .... saturday = 7
    int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);

    if (isWeekendDay(dayOfWeek)) {
      return true;
    }
    return Toll.isTollFreeDateMonthDay(MonthDay.of(month, day));
  }

  /**
   *  Checks whether a date is toll free
   *
   * @param dayOfWeek - the day of the week from Calender enum
   * @return boolean - true if day belongs too weekend
   */
  private boolean isWeekendDay(int dayOfWeek) {
    return dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY;
  }

  /**
   *  Vehicles that are tollfree
   */
  private enum TollFreeVehicles {
    MOTORBIKE("Vehicles.Motorbike"),
    TRACTOR("Tractor"),
    EMERGENCY("Emergency"),
    DIPLOMAT("Diplomat"),
    FOREIGN("Foreign"),
    MILITARY("Military");
    private final String type;

    TollFreeVehicles(String type) {
      this.type = type;
    }

    public String getType() {
      return type;
    }
  }
}

