package calculator;

import com.google.gson.*;
import lombok.extern.slf4j.Slf4j;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.time.LocalDate;
import java.time.ZoneId;
import java.time.ZonedDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

@Slf4j
public class GoogleCalendarHandler implements CalenderHandler
{

    public List<ZonedDateTime> getHolidays(ZoneId zoneId)
    {
        String rawData = fetchHolidaysFromGoogleApi();
        return parseJsonForHolidays(rawData, zoneId);
    }

    private List<ZonedDateTime> parseJsonForHolidays(String jsonString, ZoneId zoneId)
    {
        //create tree from JSON
        JsonElement rootNode = JsonParser.parseString(jsonString);
        JsonObject details = rootNode.getAsJsonObject();
        JsonArray eventArray = details.getAsJsonArray("items");
        List<ZonedDateTime> holidays = new ArrayList<>();
        eventArray.forEach(it ->
                           {
                               String startDate = it.getAsJsonObject()
                                                    .get("start")
                                                    .getAsJsonObject()
                                                    .get("date")
                                                    .getAsString();

                               String endDate = it.getAsJsonObject()
                                                  .get("end")
                                                  .getAsJsonObject()
                                                  .get("date")
                                                  .getAsString();
                               getDatesBetween(startDate, endDate, zoneId);
                           });
        return holidays;
    }

    private static List<ZonedDateTime> getDatesBetween(String startDate, String endDate, ZoneId zoneId)
    {
        return LocalDate.parse(startDate)
                        .datesUntil(LocalDate.parse(endDate))
                        .map(it -> it.atStartOfDay(zoneId))
                        .collect(Collectors.toList());
    }

    /**
     * Gets json string with all public holidays.
     *
     * note: todo returns in swedish time
     *
     * @return json string
     */
    private String fetchHolidaysFromGoogleApi()
    {
        URL url = null;
        String response = "{}";
        String urlStr = (String) PropertiesAccessor.getSecretProperty("GOOGLE_CALENDAR_URL", "NOT_FOUND");
        if (urlStr.equals("NOT_FOUND"))
        {
            log.error("Can not load url to google calendar api. ");
        }
        else
        {
            HttpURLConnection con = null;
            try
            {
                url = new URL(urlStr);
                con = (HttpURLConnection) url.openConnection();
                con.setRequestMethod("GET");
                con.setRequestProperty("Content-Type", "application/json");
                response = readConnection(con);
            } catch (IOException ex)
            {
                log.error(ex.getMessage());
                ex.printStackTrace();
            } finally
            {
                if (con != null) con.disconnect();
            }
        }
        return response;
    }


    private String readConnection(HttpURLConnection con) throws IOException
    {
        try (BufferedReader in = new BufferedReader(new InputStreamReader(con.getInputStream())))
        {
            String inputLine;
            StringBuilder content = new StringBuilder();
            while ((inputLine = in.readLine()) != null)
            {
                content.append(inputLine);
            }
            return content.toString();
        }
    }

}