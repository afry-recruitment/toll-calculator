import java.util.*;
import java.io.FileReader;
import java.io.IOException;
import java.io.Reader;
import org.json.simple.parser.*;
import org.json.simple.parser.ParseException;
import org.json.JSONArray;
import org.json.JSONObject;

import Classes.TollFeeForTime;

public class TollCalculator {

  String plateRegex = "^[A-Z][A-Z][A-Z]\\s[0-9][0-9][A-Z0-9]$";

  /**
   * Calculate the total toll fee of a vehicle for one day
   *
   * @param plate - The plate number of the vehicle
   * @return - the total toll fee for that day
   */
  public int getTollFeeForTheDay(String plate) {
    int totalFee = 0;
    
    if (plate.matches(plateRegex)) {
      JSONParser parser = new JSONParser();
      try (Reader reader = new FileReader("../Json/TollUserDB.json")) {

          JSONObject jsonObject = (JSONObject) parser.parse(reader);
          JSONArray jsonArr = jsonObject.getJSONArray(plate);

          for (int x = 0; x < jsonArr.length(); x++) {
            JSONObject tollEntry = jsonArr.getJSONObject(x);
            totalFee+=tollEntry.getInt("Fee");
          }

      } catch (IOException e) {
          e.printStackTrace();
      } catch (ParseException e) {
          e.printStackTrace();
      }
    }

    if (totalFee > 60) totalFee = 60;
    return totalFee;
  }

  public int getTollFee(final Date date, Vehicle.VehicleList vehicle) {
    Vehicle checkVehicle = new Vehicle();
    CheckDateIfTollFree checkDate = new CheckDateIfTollFree(date);
    Integer fee = 0;

    if (checkVehicle.isVehicleTollFree(vehicle) || checkDate.checkIfTollFree()) {
      return 0;
    } else {
      Calendar calendar = Calendar.getInstance();
      calendar.setTime(date);
      JSONParser parser = new JSONParser();
      try (Reader reader = new FileReader("../Json/TollFees.json")) {

          JSONObject jsonObject = (JSONObject) parser.parse(reader);
          JSONArray jsonArr = jsonObject.getJSONArray("TollFees");

          for (int x = 0; x < jsonArr.length(); x++) {
            JSONObject tollFee = jsonArr.getJSONObject(x);
            if (tollFee.get("HourStart").equals(calendar.get(Calendar.HOUR_OF_DAY))) {
              TollFeeForTime tollFeeObject = new TollFeeForTime(tollFee.getInt("HourStart"), tollFee.getInt("HourEnd"), tollFee.getInt("MinuteStart"), tollFee.getInt("MinuteEnd"), tollFee.getInt("Fee"));
              if (calendar.get(Calendar.MINUTE) >= tollFeeObject.getMinuteStart() && calendar.get(Calendar.MINUTE) <= tollFeeObject.getMinuteEnd()) {
                fee = tollFeeObject.getFee();
              }
            }
          }

      } catch (IOException e) {
          e.printStackTrace();
      } catch (ParseException e) {
          e.printStackTrace();
      }
    }

    return fee;
  }

  private void saveFeeForVehicle(String plate, int hour, int fee) throws ParseException {
    if (plate.matches(plateRegex)) {
      JSONParser parser = new JSONParser();
      try (Reader reader = new FileReader("../Json/TollUserDB.json")) {

          JSONObject jsonObject = (JSONObject) parser.parse(reader);
          JSONArray jsonArr = jsonObject.getJSONArray(plate);
          
          if (jsonArr.length() > 0) {
            for (int x = 0; x < jsonArr.length(); x++) {
              JSONObject tollFee = jsonArr.getJSONObject(x);
              if (tollFee.get("Hour").equals(hour) && tollFee.getInt("Fee") < fee) {
                tollFee.put("Fee", fee);
                return;
              }
            }

            jsonArr.put(generateFeeEntry(hour, fee));
          } else {
            generateEntry(plate, generateFeeEntry(hour, fee));
          }

      } catch (IOException e) {
          e.printStackTrace();
      }
    }
  }

  private JSONObject generateEntry(String plate, JSONObject feeEntry) throws ParseException {
    StringBuilder builder = new StringBuilder();
    builder .append(plate)
            .append(":[")
            .append(feeEntry)
            .append("]");
    String jsonString = builder.toString();
    JSONParser parser = new JSONParser();
    JSONObject jsonObj = (JSONObject) parser.parse(jsonString);
    return jsonObj;
  }

  private JSONObject generateFeeEntry(int hour, int fee) throws ParseException {
    StringBuilder builder = new StringBuilder();
    builder .append("{\"Hour\":")
            .append(hour)
            .append(", \"Fee\":")
            .append(fee)
            .append("}");
    String jsonString = builder.toString();
    JSONParser parser = new JSONParser();
    JSONObject jsonObj = (JSONObject) parser.parse(jsonString);
    return jsonObj;
  }
}

