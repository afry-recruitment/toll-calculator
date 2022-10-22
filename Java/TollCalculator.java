
import java.lang.reflect.Array;
import java.time.LocalDate;
import java.time.LocalTime;
import java.time.MonthDay;
import java.util.*;

public class TollCalculator {

  Calendar gregorianCalendar = GregorianCalendar.getInstance();
  final int monthOffset = 1;
  final int minuteInSeconds = 60;
  final int hourInSeconds = 3600;
  final int dayMaxFee = 60;


  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return - the total toll fee for that day
   */
  public int getTollFee(Vehicle vehicle, Date... dates) {
    //Check for null pointers
    //TODO make this cleaner
    if(vehicle == null || dates == null){
      throw new NullPointerException("Hi there");
    }
    //Check if needed to run at all
    if(!tollable(vehicle, dates)){
      return 0;
    }
    //Remove free dates as they are not to be counted
    dates = removeTollFreeDates(dates);

    //Sort dates into sets of days
    HashMap<LocalDate, ArrayList<Date>> datesByDay = getDaysHashmap(dates);

    int totalToll = 0;

    //Doing the toll calculations
    for (LocalDate day:datesByDay.keySet()) {
      Date[][] daySegmentedByHour = getSegmentedDay(datesByDay.get(day),hourInSeconds);

      int tollForDay = 0;

      for (Date[] segment:daySegmentedByHour) {
        tollForDay += getMaxTollFeeFromDates(segment);//We could break if tollForADay >=dayMaxFee since it´s the limit
      }
      if(tollForDay>dayMaxFee){
        tollForDay = dayMaxFee;
      }
      totalToll +=tollForDay;
    }
    return totalToll;
  }

  private Date[] removeTollFreeDates(Date[] dates) {
    ArrayList<Date> result = new ArrayList<>();
    for (Date date:dates) {
      if(!isTollFreeDate(date)){
        result.add(date);
      }
    }
    Date[] resArr = new Date[result.size()];
    resArr = result.toArray(resArr);
    return result.toArray(resArr);
  }

  private int getMaxTollFeeFromDates(Date[] dates) {
    int toll = 0;
    for (Date d:dates) {
      int fee = Toll.getCost(d);
      if(fee>toll){
        toll = fee;
      }
    }

    return toll;
  }

  private void printArrayListDates(ArrayList<Date> dates) {
    for (Date d:dates) {
      System.out.println("  " + d.toString());
    }
  }

  //Segments Dates of one day into chunks spanning over param: seconds
  private /*HashMap<Integer, ArrayList<Date>>*/ Date[][] getSegmentedDay(ArrayList<Date> dates, int segmentLimitSeconds) {

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

  //TODO test
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

  private LocalDate getLocalDateWithOffset(Date date) {
    gregorianCalendar.setTime(date);
    int year = gregorianCalendar.get(Calendar.YEAR);
    int month = gregorianCalendar.get(Calendar.MONTH) + monthOffset;
    int day = gregorianCalendar.get(Calendar.DAY_OF_MONTH);
    return LocalDate.of(year, month, day);
  }

  //Check whether a vehicle and set of dates can be tolled
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

  private boolean isTollFreeVehicle(Vehicle vehicle) {
    if(vehicle == null)
    {
      throw new NullPointerException("Parameter vehicle was null in method isTollFreeVehicle");
    }

    String vehicleType = vehicle.getType();
    for (TollFreeVehicles tf:TollFreeVehicles.values()) {
      if(vehicleType.equals(tf.getType())){
        return true;
      }
    }
    return false;
  }

  public int getTollFee(Date date, Vehicle vehicle) {
    if(isTollFreeDate(date) || isTollFreeVehicle(vehicle)) {
      return 0;
    }
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    int hour = calendar.get(Calendar.HOUR_OF_DAY);
    int minute = calendar.get(Calendar.MINUTE);

    return Toll.getCost(LocalTime.of(hour,minute,0));
  }

  private Boolean isTollFreeDate(Date date) {
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);

    int year = calendar.get(Calendar.YEAR);
    int month = calendar.get(Calendar.MONTH) + 1; //Offset 1 to make January 1 instrad of 0
    int day = calendar.get(Calendar.DAY_OF_MONTH);

    int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
    if (isWeekendDay(dayOfWeek)) {
      return true;
    }

    //TODO, is yearcheck needed?
    return Toll.isTollFreeDateMonthDay(MonthDay.of(month, day));
  }

  private boolean isWeekendDay(int dayOfWeek) {
    return dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY;
  }

  private enum TollFreeVehicles {
    MOTORBIKE("Motorbike"),
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

