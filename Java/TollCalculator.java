package Rayar.AFRY.com;

import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.concurrent.TimeUnit;

public class TollCalculator {

	/**
	 * Calculate the total toll fee for one day
	 *
	 * @param vehicle - the vehicle
	 * @param dates   - date and time of all passes on one day
	 * @return - the total toll fee for that day
	 */
	
	// Made Calendar global variable since it was used in many places
	private static Calendar calendar = GregorianCalendar.getInstance();
	
	public int getTollFee(Vehicle vehicle, Date... dates) {
		Date intervalStart = dates[0];
		int totalFee = 0;
		
		for (Date date : dates) {
			// can implement to check all dates passed should be the same date month and year but different in timings 
			int nextFee = getTollFee(date, vehicle);
			int tempFee = getTollFee(intervalStart, vehicle);

			TimeUnit timeUnit = TimeUnit.MINUTES;
			long diffInMillies = date.getTime() - intervalStart.getTime();
			long minutes = timeUnit.convert(diffInMillies, TimeUnit.MILLISECONDS);
			if (minutes <= 60) {
				if (totalFee > 0)
					totalFee -= tempFee;
				if (nextFee >= tempFee)
					tempFee = nextFee;
				totalFee += tempFee;
			} else {
				totalFee += nextFee;
			}
		}
		if (totalFee > 60)
			totalFee = 60;
		return totalFee;
	}

	private synchronized int getTollFee(final Date date, Vehicle vehicle) {
		if (isTollFreeDate(date) || isTollFreeVehicle(vehicle))
			return 0;

		calendar.setTime(date);
		int hour = calendar.get(Calendar.HOUR_OF_DAY);
		int minute = calendar.get(Calendar.MINUTE);

		// here i have optimized below if condition where ever it is returning common value
		if ((hour == 6 && minute >= 0 && minute <= 29)
				
				/*in the below condition the time for example : (12 hours and 10 minutes)was
				 getting failed which was not supposed to.
				 hours which are between 8 to 14 and have minutes are not 30 or 50 was getting failed
				 So, i have modified the below condition to get proper result */
				
				|| ((hour == 8 && minute >= 30) || (hour > 8 && hour <= 14 && minute <= 59))
				|| (hour == 18 && minute >= 0 && minute <= 29))
			return 9;
		else if ((hour == 6 && minute >= 30 && minute <= 59) || (hour == 8 && minute >= 0 && minute <= 29)
				|| (hour == 15 && minute >= 0 && minute <= 29) || (hour == 17 && minute >= 0 && minute <= 59))
			return 16;
		else if ((hour == 7 && minute >= 0 && minute <= 59)
				|| (hour == 15 && minute >= 30 || hour == 16 && minute <= 59))
			return 18;
		else
			return 0;
	}
	
	private boolean isTollFreeVehicle(Vehicle vehicle) {
		if (vehicle == null)
			return false;
		String vehicleType = vehicle.getType();

		//Here made changes to check only if it is Toll Free vehicle ( Emergency and Military)
		return vehicleType.equals(TollFreeVehicles.EMERGENCY.getType())
				|| vehicleType.equals(TollFreeVehicles.MILITARY.getType());
	}

	private synchronized Boolean isTollFreeDate(Date date) {
		calendar.setTime(date);
		int year = calendar.get(Calendar.YEAR);
		int month = calendar.get(Calendar.MONTH);
		int day = calendar.get(Calendar.DAY_OF_MONTH);
		int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);

		/*In the below line have removed hard coded year value and updated with dynamically fetches the
		 existing year and added weekend condition in the same if condition */ 
		if (year == GregorianCalendar.getInstance().getWeekYear() 
				&& dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY 
				|| month == Calendar.JANUARY && day == 1
				|| month == Calendar.MARCH && (day == 28 || day == 29)
				|| month == Calendar.APRIL && (day == 1 || day == 30)
				|| month == Calendar.MAY && (day == 1 || day == 8 || day == 9)
				|| month == Calendar.JUNE && (day == 5 || day == 6 || day == 21) || month == Calendar.JULY
				|| month == Calendar.NOVEMBER && day == 1
				|| month == Calendar.DECEMBER && (day == 24 || day == 25 || day == 26 || day == 31)) {

			return true;
		}
		return false;
	}

	private enum TollFreeVehicles {
		MOTORBIKE("Motorbike"), TRACTOR("Tractor"), EMERGENCY("Emergency"), DIPLOMAT("Diplomat"), FOREIGN("Foreign"),
		MILITARY("Military"), CAR("Car");

		private final String type;

		TollFreeVehicles(String type) {
			this.type = type;
		}

		public String getType() {
			return type;
		}
	}

}
