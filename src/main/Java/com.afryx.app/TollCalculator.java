package com.afryx.app;

import java.io.IOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.Duration;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.Locale;
import java.util.concurrent.TimeUnit;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import org.json.JSONException;

public class TollCalculator {

  /**
   * Calculate the total toll fee for one day. If dates from multiple days are provided, returns 0.
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return - the total toll fee for that day
   */
  public int getTollFee(Vehicle vehicle, Date... dates) {
    // Sort the dates, with the earliest date first
    Arrays.sort(dates);
    int totalFee = 0;
    // Init with the first fee received, marks start of hour tracked
    Date currentSessionStart = dates[0];
    // Ensure that the dates given all lies on the same day
    var largestTimeDiff = dates[dates.length - 1].getTime() - currentSessionStart.getTime();
    if (dates.length > 1
        && TimeUnit.DAYS.convert(largestTimeDiff,
        TimeUnit.MILLISECONDS) > 0) {
      return 0;
    }
    int tempFee = getTollFee(currentSessionStart, vehicle);

    for (Date date : dates) {
      int nextFee = getTollFee(date, vehicle);
      // Compare the time of the last toll with the current one
      TimeUnit timeUnit = TimeUnit.MINUTES;
      long diffInMillis = date.getTime() - currentSessionStart.getTime();
      long minutes = timeUnit.convert(diffInMillis, TimeUnit.MILLISECONDS);

      // Only add the fee once per hour. I also assume one hour has passed if e.g. 07:00->08:00
      if (minutes >= 60) {
        totalFee += tempFee;
        // The current loop date marks the start of a new hour
        tempFee = nextFee;
        currentSessionStart = date;
      } else {
        // Store the largest fee for the current hour
        tempFee = Math.max(nextFee, tempFee);
      }
    }
    // Add the last fee accounted for as only "closed" hours are added in the loop
    totalFee += tempFee;

    // Max of 60kr per day in tolls
    return Math.min(totalFee, 60);
  }

  /**
   * Checks if the given `Vehicle` is exempted from tolls according to the `TollFreeVehicles` enum.
   *
   * @param vehicle to check if exempted.
   * @return true if exempted, false otherwise.
   */
  private boolean isTollFreeVehicle(Vehicle vehicle) {
    if (vehicle == null) {
      return false;
    }
    try {
      // We only care of the vehicle's type match any of the TollFreeVehicles'
      TollFreeVehicles.valueOf(vehicle.getType().toUpperCase(Locale.ROOT));
      return true;
    } catch (IllegalArgumentException e) {
      // If no enum matched the provided vehicle's type, an error is caught and false return
      return false;
    }
  }

  private int getTollFee(final Date date, Vehicle vehicle) {
    // Create and set the calendar date once
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    if (isTollFreeCalendarDateAPI(calendar) || isTollFreeVehicle(vehicle)) {
      return 0;
    }
    int hour = calendar.get(Calendar.HOUR_OF_DAY);
    int minute = calendar.get(Calendar.MINUTE);

    if (hour == 6 && minute <= 29) {
      return 8;
    } else if (hour == 6) {
      return 13;
    } else if (hour == 7) {
      return 18;
    } else if (hour == 8 && minute <= 29) {
      return 13;
    } else if (hour >= 8 && hour <= 14) {
      return 8;
    } else if (hour == 15 && minute <= 29) {
      return 13;
    } else if (hour == 15 || hour == 16) {
      return 18;
    } else if (hour == 17) {
      return 13;
    } else if (hour == 18 && minute <= 29) {
      return 8;
    } else {
      return 0;
    }
  }

  // Leaving this method here if we want to calculate for year 2013
  private Boolean isTollFreeCalendarDate(Calendar calendar) {
    int year = calendar.get(Calendar.YEAR);
    int month = calendar.get(Calendar.MONTH);
    int day = calendar.get(Calendar.DAY_OF_MONTH);

    int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
    if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) {
      return true;
    }

    if (year == 2013) {
      return month == Calendar.JANUARY && day == 1 ||
          month == Calendar.MARCH && (day == 28 || day == 29) ||
          month == Calendar.APRIL && (day == 1 || day == 30) ||
          month == Calendar.MAY && (day == 1 || day == 8 || day == 9) ||
          month == Calendar.JUNE && (day == 5 || day == 6 || day == 21) ||
          month == Calendar.JULY ||
          month == Calendar.NOVEMBER && day == 1 ||
          month == Calendar.DECEMBER && (day == 24 || day == 25 || day == 26 || day == 31);
    }
    return false;
  }

  /**
   * Makes an API request to "Svenska Dagar v2.1", a site that returns info about days from a
   * Swedish context. This is an attempt at adding future support and steer away from hard-coded
   * calendar cases.
   *
   * @param calendar the Gregorian calendar with the date in question set.
   * @return true if the given date is assumed toll-free, false otherwise.
   */
  private Boolean isTollFreeCalendarDateAPI(Calendar calendar) {
    // Handel special-case days provided Transportstyrelsen's online info page
    if (isExemptedTollCalendarDate(calendar)) {
      return true;
    }
    int year = calendar.get(Calendar.YEAR);
    int month = calendar.get(Calendar.MONTH) + 1; // Zero indexed month enum, add 1 for API
    int day = calendar.get(Calendar.DAY_OF_MONTH);
    String req = year + "/" + month + "/" + day;

    HttpClient httpClient = HttpClient.newBuilder()
        .version(HttpClient.Version.HTTP_2)
        .connectTimeout(Duration.ofSeconds(10))
        .build();
    try {
      HttpRequest request = HttpRequest.newBuilder()
          .GET()
          .uri(URI.create("https://sholiday.faboul.se/dagar/v2.1/" + req))
          .build();
      HttpResponse<String> response = httpClient.send(request,
          HttpResponse.BodyHandlers.ofString());

      // As this codebase wasn't using non-java libraries before, opt to keep it that way.
      // Might be a deliberate decision, will use regex rather than json library
      Pattern pattern = Pattern.compile("\"arbetsfri dag\":\"Ja\",\"");
      Matcher matcher = pattern.matcher(response.body());
      // True if the day is tagged as work-free "red-day"
      return matcher.find();

      // After introducing JUnit for testing, I revisited this part and added json parsing.
      // Would prefer this solution if using mvn (or other package tool) was allowed.
//      JSONObject obj = new JSONObject(response.body());
//      JSONArray arr = obj.getJSONArray("dagar");
//      return arr.getJSONObject(0).get("arbetsfri dag") == "Ja";
    } catch (IOException | JSONException |
             InterruptedException e) {
      e.printStackTrace();
    }
    return false;
  }

  /**
   * Checks if the given calendar with a set date is exempted from tolls according to
   * "Transportstyrelsen Göteborg". Only looks at special dates, outside of weekends and red-days.
   * Note: This is made for the year 2023 and only works for that year. Could not find an API for
   * this.
   *
   * @param calendar the Gregorian calendar with the date in question set.
   * @return true if exempted from tolls this special date, false otherwise.
   */
  private boolean isExemptedTollCalendarDate(Calendar calendar) {
    int year = calendar.get(Calendar.YEAR);
    int month = calendar.get(Calendar.MONTH); // Note: Zero indexed month enum
    // The month of July is always excepted from tolls
    if (month == Calendar.JULY) {
      return true;
    }
    // The specific dates only apply to the year 2023
    if (year != 2023) {
      return false;
    }

    int day = calendar.get(Calendar.DAY_OF_MONTH);

    return switch (month) {
      case Calendar.JANUARY -> day == 5 || day == 6;
      case Calendar.APRIL -> day == 6 || day == 7;
      case Calendar.MAY -> day == 1 || day == 17 || day == 18;
      case Calendar.JUNE -> day == 5 || day == 6 || day == 23;
      case Calendar.NOVEMBER -> day == 3;
      case Calendar.DECEMBER -> day == 25 || day == 26;
      default -> false;
    };
  }

  private enum TollFreeVehicles {
    MOTORBIKE("Motorbike"),
    TRACTOR("Tractor"),
    EMERGENCY("Emergency"),
    DIPLOMAT("Diplomat"),
    // Since 2015, even foreign vehicles need to pay a toll!
    //FOREIGN("Foreign"),
    //Trailers are also exempted
    TRAILER("Trailer"),
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

