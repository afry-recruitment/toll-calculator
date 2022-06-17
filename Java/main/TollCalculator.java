package main;

import java.time.*;
import java.util.*;
import java.util.concurrent.*;
import java.io.*;
import java.net.MalformedURLException;
import java.net.*;
import java.net.http.*;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse.BodyHandlers;
import org.json.*;

import com.google.gson.JsonParser;

import vehicles.Vehicle;

public class TollCalculator {

	
	private int year;
	private HashSet<String> holidays;
	private APIfields apifields;
	
	
	public TollCalculator(int year) {
		this.year = year;
		this.holidays = new HashSet<String>();
		
		this.apifields = getAPIfields();
		
		getHolidayJSON();
		
	}
	
	public APIfields getAPIfields() {
		
		
		
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
            
			return new APIfields(obj.getString("access_token"),obj.getString("token_type"), obj.getInt("expires_in"));
			
			
		} catch (MalformedURLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} 
		
		return null;
		
	}
	
	private String getHolidayJSON() {
		
		try {
			URL url = new URL("https://svenskahelgdagar.info/v2/year/"+this.year);
			
			HttpURLConnection con = (HttpURLConnection) url.openConnection();
			con.setRequestMethod("GET");
			con.setDoOutput(true);
			
			con.setRequestProperty("Authorization",this.apifields.getToken_type() + " " + this.apifields.getAccess_token().toString());
			con.setRequestProperty("Content-Type", "application/json");
			con.setRequestProperty("Accept", "application/json");
            
            
            InputStream inputStream = con.getInputStream();
            BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(inputStream));

            String stringLine;
            stringLine = bufferedReader.readLine();
            
           
            
            JSONObject o = new JSONObject(stringLine);
            JSONObject arr = o.getJSONObject("response");
            Set<String> s = arr.keySet();
            Iterator<String> it = s.iterator();
            while(it.hasNext()) {
            	this.holidays.add(it.next().toString());
            }
         
			
		} catch (MalformedURLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		return "he";
		
	}
	
	/*private HashSet<String> getHolidays(){
		//Create a new HashSet<String>
		HashSet<String> holidays = new HashSet<String>();
		
		
		String path = "/cache/"+year+".txt";
		String dirName = "cache";
		//if directory exist
		File dir = new File(dirName);
		
		if(!dir.isDirectory()) {
			//Create directory
			dir.mkdir();
		}
		File file = new File(path);
		
		String holidayString = getHolidayJSON();
		
		try {
			if(file.createNewFile()) {
				FileWriter writer = new FileWriter(path);
				writer.write(holidayString);
				writer.close();
				
			}
		} catch (IOException e) {
			System.out.println("An error occurred while writing to cache file.");
			e.printStackTrace();
		}
				//if file exists
					//read file
					//parse JSON content
					//add dates to holiday HashSet
				//if file not exist
					//create file
					//http req to API
					//add request data to file
		}
		
			
		
			*/
		
	

	
  /**
   * Calculate the total toll fee for one day  <- this means that all the dates are the same, only time differs
   *
   * @param vehicle - the vehicle
   * @param dateTimes   - date and time of all passes on one day
   * @return - the total toll fee for that day
   */
  public int getTotalDailyFee(Vehicle vehicle, LocalDateTime... dateTimes) {
    
	  int totalFee = 0;
	  ArrayList<int[][]> range= new ArrayList<int[][]>(); //this will hold pairs of int ranges and int, <int[],int>,
	  													  //where the int[] is a range of minutes over a day and the int is the fee for that range
	  
	  
	  for(LocalDateTime date : dateTimes) {
		  
		  
		  //occupy an interval of time for a specific fee
		  int start = date.getHour()*60 + date.getMinute();
		  int finish = date.getHour()*60 + date.getMinute() + 60;
		  int[] partRange = {start, finish};
		  int fee = getFee(vehicle, date);
		  int[][] a = { partRange, {fee} };
		  
		  
		  boolean overLap = false;
		  //look for overlap
		  for(int[][] occupiedRange : range) {
			  
			  if(start >= occupiedRange[0][0] && start <= occupiedRange[0][1]) {
				  
				  //update fee if it is larger
				  if(fee > occupiedRange[1][0]) {
					  occupiedRange[1][0] = fee;
				  }
				  overLap = true;
				  continue;
			  }
			  
			  
		  }
		  
		  //if there is no overlap, add to range
		  if(!overLap) {
			  range.add(a);
			  overLap = true;
		  }
		
		  
		  
	  }
	  
	  //add the fees together for the day
	  for(int[][] partRange : range) {
		  totalFee += partRange[1][0];
	  }
	  if(totalFee > 60) totalFee = 60; //maximum fee is 60 sek
	  
	  
	  return totalFee;
	  
	 /*Date intervalStart = dates[0];
    int totalFee = 0;
    for (Date date : dates) {
      int nextFee = getFee(date, vehicle);
      int tempFee = getFee(intervalStart, vehicle);

      TimeUnit timeUnit = TimeUnit.MINUTES;
      long diffInMillies = date.getTime() - intervalStart.getTime();
      long minutes = timeUnit.convert(diffInMillies, TimeUnit.MILLISECONDS);

      if (minutes <= 60) {
        if (totalFee > 0) totalFee -= tempFee;
        if (nextFee >= tempFee) tempFee = nextFee;
        totalFee += tempFee;
      } else {
        totalFee += nextFee;
      }
    }
    if (totalFee > 60) totalFee = 60;
    return totalFee;*/
  }



  private int getFee(Vehicle vehicle, final LocalDateTime dateTime) {
    if(isTollFreeDate(dateTime) || isTollFreeVehicle(vehicle)) return 0;
    int hour = dateTime.getHour();
    int minute = dateTime.getMinute();

    if (hour == 6 && minute >= 0 && minute <= 29) return 8;
    else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
    else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
    else if(hour >= 8 && hour <= 14) {
    	if((hour == 8) && (minute <= 29)) return 13;
    	return 8;
    }
    else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
    else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
    else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
    else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
    else return 0;
	
	
  }
  
  private boolean isTollFreeVehicle(Vehicle vehicle) {
	    if(vehicle == null) return false;
	    String vehicleType = vehicle.getType();
	    return vehicleType.equals(TollFreeVehicles.MOTORBIKE.getType()) ||
	           vehicleType.equals(TollFreeVehicles.TRACTOR.getType()) ||
	           vehicleType.equals(TollFreeVehicles.EMERGENCY.getType()) ||
	           vehicleType.equals(TollFreeVehicles.DIPLOMAT.getType()) ||
	           vehicleType.equals(TollFreeVehicles.FOREIGN.getType()) ||
	           vehicleType.equals(TollFreeVehicles.MILITARY.getType());
	  }

  private Boolean isTollFreeDate(LocalDateTime date) {
   
    int year = date.getYear();
    int month = date.getMonthValue();
    int day = date.getDayOfMonth();

    DayOfWeek dayOfWeek = date.getDayOfWeek();
    if (dayOfWeek == DayOfWeek.SATURDAY || dayOfWeek == DayOfWeek.SUNDAY) return true;

    String dateStr = year+"-"+month+"-"+day;
    if(holidays.contains(dateStr)) return true;
    
    /*if (year == 2013) {
      if (month == Calendar.JANUARY && day == 1 ||
          month == Calendar.MARCH && (day == 28 || day == 29) ||
          month == Calendar.APRIL && (day == 1 || day == 30) ||
          month == Calendar.MAY && (day == 1 || day == 8 || day == 9) ||
          month == Calendar.JUNE && (day == 5 || day == 6 || day == 21) ||
          month == Calendar.JULY ||
          month == Calendar.NOVEMBER && day == 1 ||
          month == Calendar.DECEMBER && (day == 24 || day == 25 || day == 26 || day == 31)) {
        return true;
      }
    }*/
    
    
    return false;
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

