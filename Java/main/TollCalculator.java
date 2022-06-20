package main;

import java.time.*;
import java.util.*;
import java.io.*;
import java.net.MalformedURLException;
import java.net.*;
import org.json.*;

import vehicles.Vehicle;

public class TollCalculator {

	private int year;
	private HashSet<String> holidays;
	private APIfields apifields;

	
	/**
	 * The constructor of this class. 
	 * 
	 * @param year The year for which fees will be calculated from. Different years have different dates for holidays.
	 */
	public TollCalculator(int year) {
		this.year = year;
		this.holidays = getHolidaySet();
		this.apifields = getAPIfields();

	}

	/**
	 * 
	 * @return Returns an instance of the APIfields class with usable API fields. Can return null if no connection to the API can be made
	 */
	private APIfields getAPIfields() {

		try {

			URL url = new URL("https://svenskahelgdagar.info/v2/access_token");

			String data = "grant_type=client_credentials&client_id=9904jolagm3211&client_secret=4b7248-468b4a-f143ec-fe603f-df4fd7";

			HttpURLConnection con = (HttpURLConnection) url.openConnection();
			con.setRequestMethod("POST");
			con.setDoOutput(true);

			con.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
			con.setRequestProperty("Accept", "application/x-www-form-urlencoded");
			con.setRequestProperty("Content-Length", Integer.toString(data.length()));

			DataOutputStream dos = new DataOutputStream(con.getOutputStream());
			dos.writeBytes(data);

			InputStream inputStream = con.getInputStream();

			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(inputStream));

			String stringLine;
			stringLine = bufferedReader.readLine();

			JSONObject obj = new JSONObject(stringLine);

			return new APIfields(obj.getString("access_token"), obj.getString("token_type"), obj.getInt("expires_in"));

		} catch (MalformedURLException e) {
			System.out.println("Something went wrong in function getAPIfields()");
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			System.out.println("Something went wrong in function getAPIfields()");
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		return null;

	}

	/**
	 * 
	 * @return Returns a String representing a JSON object containing all holidays for the year of this object. Returns null if no connection can be made to the API
	 */
	private String getHolidayJSONString() {

		try {
			URL url = new URL("https://svenskahelgdagar.info/v2/year/" + this.year);

			HttpURLConnection con = (HttpURLConnection) url.openConnection();
			con.setRequestMethod("GET");
			con.setDoOutput(true);

			con.setRequestProperty("Authorization",
					this.apifields.getToken_type() + " " + this.apifields.getAccess_token().toString());
			con.setRequestProperty("Content-Type", "application/json");
			con.setRequestProperty("Accept", "application/json");

			InputStream inputStream = con.getInputStream();
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(inputStream));

			String JSONString;
			JSONString = bufferedReader.readLine();

			return JSONString;

		} catch (MalformedURLException e) {
			System.out.println("Something went wrong in function getHolidayJSON()");
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			System.out.println("Something went wrong in function getHolidayJSON()");
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		return null;

	}

	
	/**
	 * 
	 * @return Returns a HashSet<String> object containing all holidays for the year of this object in the format "yyyy-mm-dd"
	 */
	private HashSet<String> getHolidaySet() {

		HashSet<String> holidays = new HashSet<String>();
		String path = "Java/cache";
		String fileName = "SE" + this.year + ".txt";

		File dir = new File(path);
		if (!dir.exists())
			dir.mkdir();

		File cache = new File(path + "/" + fileName);

		if (cache.length() == 0) {
			FileWriter fileWriter;
			try {
				fileWriter = new FileWriter(cache.getAbsoluteFile());

				fileWriter.write(getHolidayJSONString());
				fileWriter.close();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}

		}

		String JSONString;
		try {
			Scanner reader = new Scanner(cache);
			JSONString = reader.nextLine();
			JSONObject obj = new JSONObject(JSONString);
			JSONObject arr = obj.getJSONObject("response");
			Set<String> datesStr = arr.keySet();
			Iterator<String> it = datesStr.iterator();
			while (it.hasNext()) {
				holidays.add(it.next().toString());
			}
			reader.close();
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		return holidays;
	}

	/**
	 * Calculate the total toll fee for one day <- this means that all the dates are
	 * the same, only time differs
	 *
	 * @param vehicle   - the vehicle
	 * @param dateTimes - date and time of all passes on one day, using LocalDateTime format. These must be sorted in chronological order
	 * @return - the total toll fee for that day for the given vehicle
	 */
	public int getTotalDailyFee(Vehicle vehicle, LocalDateTime... dateTimes) {
	
		int totalFee = 0;
		ArrayList<int[][]> range = new ArrayList<int[][]>(); // this will hold pairs of int ranges and int, <int[],int>,
																// where the int[] is a range of minutes over a day and
																// the int is the fee for that range
		for (LocalDateTime date : dateTimes) {
			// occupy an interval of time for a specific fee
			int start = date.getHour() * 60 + date.getMinute();
			int finish = date.getHour() * 60 + date.getMinute() + 60;
			int[] partRange = { start, finish };
			int fee = getFee(vehicle, date);
			int[][] a = { partRange, { fee } };

			boolean overlap = false;
			// look for overlap
			// for (int[][] occupiedRange : range) { //loop over in order
			for (ListIterator<int[][]> aa = range.listIterator(range.size()); aa.hasPrevious();) { // loop over the
																									// ranges in reverse
																									// order since the
																									// possible overlaps
				int[][] occupiedRange = aa.previous();												// will only be
																									// those processed
				if (start >= occupiedRange[0][0] && start <= occupiedRange[0][1]) {					// before the
																									// current one
					// update fee if it is larger
					if (fee > occupiedRange[1][0]) {
						occupiedRange[1][0] = fee;
					}
					overlap = true;
					break;
				}
			}
																									
							
			// if there is no overlap, add to range
			if (!overlap) {
				range.add(a);
				overlap = true;
			}

		}

		// add the fees together for the day
		for (int[][] partRange : range) {
			totalFee += partRange[1][0];
		}
		if (totalFee > 60)
			totalFee = 60; // maximum fee is 60 sek

		return totalFee;
	}

	/**
	 * 
	 * @param vehicle The vehicle in question
	 * @param dateTime The date and time of registered fee
	 * @return Returns an int representing the fee the vehicle will receive based on the dateTime
	 */
	private int getFee(Vehicle vehicle, final LocalDateTime dateTime) {
		if (isTollFreeDate(dateTime) || isTollFreeVehicle(vehicle))
			return 0;
		int hour = dateTime.getHour();
		int minute = dateTime.getMinute();

		if (hour == 6 && minute >= 0 && minute <= 29)
			return 8;
		else if (hour == 6 && minute >= 30 && minute <= 59)
			return 13;
		else if (hour == 7 && minute >= 0 && minute <= 59)
			return 18;
		else if (hour >= 8 && hour <= 14) {
			if ((hour == 8) && (minute <= 29))
				return 13;
			return 8;
		} else if (hour == 15 && minute >= 0 && minute <= 29)
			return 13;
		else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59)
			return 18;
		else if (hour == 17 && minute >= 0 && minute <= 59)
			return 13;
		else if (hour == 18 && minute >= 0 && minute <= 29)
			return 8;
		else
			return 0;

	}

	/**
	 * 
	 * @param vehicle The vehicle in question
	 * @return Returns a boolean value. Returns true if the vehicle is toll free and false otherwise
	 */
	private boolean isTollFreeVehicle(Vehicle vehicle) {
		if (vehicle == null)
			return false;
		String vehicleType = vehicle.getType();
		return vehicleType.equals(TollFreeVehicles.MOTORBIKE.getType())
				|| vehicleType.equals(TollFreeVehicles.TRACTOR.getType())
				|| vehicleType.equals(TollFreeVehicles.EMERGENCY.getType())
				|| vehicleType.equals(TollFreeVehicles.DIPLOMAT.getType())
				|| vehicleType.equals(TollFreeVehicles.FOREIGN.getType())
				|| vehicleType.equals(TollFreeVehicles.MILITARY.getType());
	}

	
	/**
	 * Is the date a toll free date?
	 * @param date the date in question
	 * @return Return a boolean value. Returns true if the date is a holiday or a weekend day
	 */
	private Boolean isTollFreeDate(LocalDateTime date) {

		int year = date.getYear();
		int month = date.getMonthValue();
		int day = date.getDayOfMonth();

		DayOfWeek dayOfWeek = date.getDayOfWeek();
		if (dayOfWeek == DayOfWeek.SATURDAY || dayOfWeek == DayOfWeek.SUNDAY)
			return true;

		String dateStr = year + "-" + month + "-" + day;
		if (holidays.contains(dateStr))
			return true;

		return false;
	}

	private enum TollFreeVehicles {
		MOTORBIKE("Motorbike"), TRACTOR("Tractor"), EMERGENCY("Emergency"), DIPLOMAT("Diplomat"), FOREIGN("Foreign"),
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
