package toll.calculator.services;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.TreeMap;
import java.util.concurrent.TimeUnit;
import java.util.stream.Collectors;

import toll.calculator.models.TollTaxReqObject;
import toll.calculator.models.TollTaxResObject;
import toll.calculator.models.Vehicle;
import toll.calculator.utils.RulesLoader;

public class TollTaxService {

	public TollTaxResObject getTax(TollTaxReqObject inputObject) {
		
		
		
		TollTaxResObject resObj;
		try {
			// input validations
			resObj = validate(inputObject);
			if (resObj != null && resObj.getErrorCode() != null)
							return resObj;

		} catch (Exception e) {
			e.printStackTrace();
			resObj = new TollTaxResObject(e.getLocalizedMessage(), e.getMessage(), 0, "TaxFee calculation failed.");
			return resObj;
		}


		// sort the input dates
		List<Date> inputDatesList = inputObject.getDates().stream().sorted().collect(Collectors.toList());

		// group by each day
		long millisPerDay = TimeUnit.DAYS.toMillis(1);
		Map<Long, List<Date>> grpMapDates = new TreeMap<>();
		for (Date date : inputDatesList) {
			// get the unique key for each date
			long Key = date.getTime() / millisPerDay;
			List<Date> dayWiseList = grpMapDates.get(Key);
			if (dayWiseList == null) {
				dayWiseList = new ArrayList<>();
				grpMapDates.put(Key, dayWiseList);
			}
			dayWiseList.add(date);
		}

		int totalTaxFee = 0;

		for (Map.Entry<Long, List<Date>> entry : grpMapDates.entrySet()) {

			List<Date> datesList = new ArrayList<Date>();
			datesList = entry.getValue();

			Vehicle vehicle = inputObject.getVehicle();
			String city = inputObject.getCity();

			int totalFeePerDate = 0;
			Date intervalStart = inputDatesList.get(0);

			for (int i = 0; i < datesList.size(); i++) {
				Date date = datesList.get(i);
				int tempFee = getTollFee(intervalStart, vehicle, city);
				int nextFee = getTollFee(date, vehicle, city);

				double diffInMillies = date.getTime() - intervalStart.getTime();
				double minutes = diffInMillies / 1000 / 60;

				
				
				int singleChargeRuleMins = new RulesLoader().getSingleChargeRuleMins(city);
				if( singleChargeRuleMins !=0) {
				if (minutes <= new RulesLoader().getSingleChargeRuleMins(city)) {
					if (totalFeePerDate > 0)
						totalFeePerDate -= tempFee;
					if (nextFee >= tempFee)
						tempFee = nextFee;
					totalFeePerDate += tempFee;
					//intervalStart = date;
				} else {

					intervalStart = date;
					totalFeePerDate += nextFee;
				}
				}else {
					intervalStart = date;
					totalFeePerDate += nextFee;
				}
			}

			int maxAmtperDay =  new RulesLoader().getMaxAmtPerDay(city);
			if (maxAmtperDay!=0){
			if (totalFeePerDate > maxAmtperDay)
				totalFeePerDate = maxAmtperDay;
			}
			totalTaxFee += totalFeePerDate;

		}

		resObj = new TollTaxResObject(null, null, totalTaxFee, "TaxFee for the vehicle type :"
				+ inputObject.getVehicle().getVehicleType() + " is : " + totalTaxFee + " SEK");
		return resObj;
	}
	
	private TollTaxResObject validate(TollTaxReqObject inputObject) {
		TollTaxResObject resObj = null;

		List<Integer> years = inputObject.getDates().stream().map(x -> x.getYear() + 1900).distinct()
				.collect(Collectors.toList());

		for (Integer integer : years) {
			if (new RulesLoader().validYears(inputObject.getCity()).contains(String.valueOf(integer))) 
			{
				
			}
			else
			{
				resObj = new TollTaxResObject("10000", "Year of the given dates has invalid date", 0, "valid year should be 2013 only as of now.");
			}
			
		}

		return resObj;

	}

	public static boolean isTollFreeVehicle(Vehicle vehicle, String city) {
		if (vehicle == null)
			return false;
		String vehicleType = vehicle.getVehicleType();
		return new RulesLoader().tollFreeVehicles(city).contains(vehicleType);
		// return Constants.tollFreeVehicles.contains(vehicleType);
	}

	public int getTollFee(Date date, Vehicle vehicle, String city) {
		if (isTollFreeDate(date, city) || isTollFreeVehicle(vehicle, city))
			return 0;
		Map<String, String> congestionTaxMap = new RulesLoader().getCongetstionTaxRules(city);
		Set<String> times = congestionTaxMap.keySet();
		// Set<String> times = Constants.congestionTax.keySet();

		for (String str : times) {
			if (this.includes(str, date)) {
				String result = congestionTaxMap.get(str);
				// String result = Constants.congestionTax.get(str);
				return Integer.valueOf(result);
			}
		}

		return 0;

	}

	private Boolean isTollFreeDate(Date date, String city) {
		Calendar calendar = GregorianCalendar.getInstance();
		calendar.setTime(date);
		int year = calendar.get(Calendar.YEAR);
		int month = calendar.get(Calendar.MONTH);
		int day = calendar.get(Calendar.DAY_OF_MONTH);

		StringBuffer sb = new StringBuffer();
		sb.append(year).append(month).append(day);

		int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);

		if (new RulesLoader().tollFeeDayOfWeek(city).contains(String.valueOf(dayOfWeek)))

			return true;
		if (new RulesLoader().tollFeeMonth(city).contains(String.valueOf(month)))
			return true;

		if (new RulesLoader().tollFeeDates(city).contains(String.valueOf(sb.toString())))
			return true;

		return false;
	}

	@SuppressWarnings("deprecation")
	private boolean includes(String key, Date date) {
		int hour = date.getHours();
		int minute = date.getMinutes();

		String hourStr = hour >= 10 ? ("" + hour) : "0" + hour;
		String minuteStr = minute >= 10 ? ("" + minute) : "0" + minute;

		String time = hourStr + minuteStr;

		return this.includes(key, time);
	}

	private boolean includes(String key, String time) {

		String[] keys = key.split("T");
		String start = keys[0];
		String end = keys[1];

		Integer startNum = Integer.valueOf(start);
		Integer endNum = Integer.valueOf(end);

		Integer timeNum = Integer.valueOf(time);

		if (timeNum >= startNum && timeNum <= endNum) {
			return true;
		}

		return false;
	}
}
