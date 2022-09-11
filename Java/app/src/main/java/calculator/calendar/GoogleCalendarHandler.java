package calculator.calendar;

import calculator.PropertiesAccessor;
import com.google.gson.*;
import lombok.extern.slf4j.Slf4j;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

@Slf4j
public class GoogleCalendarHandler implements CalenderHandler
{

    public List<LocalDate> getHolidays(CalendarRegion calendarRegion)
    {
        String rawData = fetchHolidaysFromGoogleApi(getCalendarRegion(calendarRegion));
        return parseJsonForHolidays(rawData);
    }

    private List<LocalDate> parseJsonForHolidays(String jsonString)
    {
        //create tree from JSON
        JsonElement rootNode = JsonParser.parseString(jsonString);
        JsonObject details = rootNode.getAsJsonObject();
        JsonArray eventArray = details.getAsJsonArray("items");
        List<LocalDate> holidays = new ArrayList<>();
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
                               holidays.addAll(getDatesBetween(startDate, endDate));
                           });
        return holidays;
    }

    private static List<LocalDate> getDatesBetween(String startDate, String endDate)
    {
        return LocalDate.parse(startDate)
                        .datesUntil(LocalDate.parse(endDate))
                        .toList();
    }

    /**
     * Gets json string with all public holidays.
     *
     * @return json string
     */
    private String fetchHolidaysFromGoogleApi(String calendarRegion)
    {
        String response = "{}";
        String baseUrl = PropertiesAccessor.getSettingsProperty("BASE_CALENDAR_URL", "NOT_FOUND");
        String calendarId =
                PropertiesAccessor.getSettingsProperty("BASE_CALENDAR_ID_FOR_PUBLIC_HOLIDAY", "NOT_FOUND");
        String apiKey = PropertiesAccessor.getSecretProperty("GOOGLE_API_KEY", "NOT_FOUND");
        // singleEvents have no has no meaning in this case other than allowing time sorting
        String urlStr =
                baseUrl + calendarRegion + "%23" + calendarId + "/events?key=" + apiKey + "&singleEvents" +
                "=true&orderBy=startTime";
        HttpURLConnection con = null;
        try
        {
            URL url = new URL(urlStr);
            con = (HttpURLConnection) url.openConnection();
            con.setRequestMethod("GET");
            con.setRequestProperty("Content-Type", "application/json");
            response = readConnection(con);
        } catch (IOException ex)
        {
            log.error("Could not load holidays: " + ex.getMessage());
            ex.printStackTrace();
        } finally
        {
            if (con != null) con.disconnect();
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

    private String getCalendarRegion(CalendarRegion region)
    {
        String calendarRegionName = switch (region)
                {
                    case AUSTRALIAN -> "CALENDAR_REGION_AUSTRALIAN";
                    case AUSTRIAN -> "CALENDAR_REGION_AUSTRIAN";
                    case BRAZILIAN -> "CALENDAR_REGION_BRAZILIAN";
                    case CANADIAN -> "CALENDAR_REGION_CANADIAN";
                    case CHINA -> "CALENDAR_REGION_CHINA";
                    case CHRISTIAN -> "CALENDAR_REGION_CHRISTIAN";
                    case DANISH -> "CALENDAR_REGION_DANISH";
                    case DUTCH -> "CALENDAR_REGION_DUTCH";
                    case FINNISH -> "CALENDAR_REGION_FINNISH";
                    case FRENCH -> "CALENDAR_REGION_FRENCH";
                    case GERMAN -> "CALENDAR_REGION_GERMAN";
                    case GREEK -> "CALENDAR_REGION_GREEK";
                    case HONG_KONG_C -> "CALENDAR_REGION_HONG_KONG_C";
                    case HONG_KONG -> "CALENDAR_REGION_HONG_KONG";
                    case INDIAN -> "CALENDAR_REGION_INDIAN";
                    case INDONESIAN -> "CALENDAR_REGION_INDONESIAN";
                    case IRANIAN -> "CALENDAR_REGION_IRANIAN";
                    case IRISH -> "CALENDAR_REGION_IRISH";
                    case ISLAMIC -> "CALENDAR_REGION_ISLAMIC";
                    case ITALIAN -> "CALENDAR_REGION_ITALIAN";
                    case JAPANESE -> "CALENDAR_REGION_JAPANESE";
                    case JEWISH -> "CALENDAR_REGION_JEWISH";
                    case MALAYSIA -> "CALENDAR_REGION_MALAYSIA";
                    case MEXICAN -> "CALENDAR_REGION_MEXICAN";
                    case NEW_ZEELAND -> "CALENDAR_REGION_NEW_ZEELAND";
                    case NORWEGIAN -> "CALENDAR_REGION_NORWEGIAN";
                    case PHILIPPINES -> "CALENDAR_REGION_PHILIPPINES";
                    case POLISH -> "CALENDAR_REGION_POLISH";
                    case PORTUGUESE -> "CALENDAR_REGION_PORTUGUESE";
                    case RUSSIAN -> "CALENDAR_REGION_RUSSIAN";
                    case SINGAPORE -> "CALENDAR_REGION_SINGAPORE";
                    case SOUTH_AFRICA -> "CALENDAR_REGION_SOUTH_AFRICA";
                    case SOUTH_KOREAN -> "CALENDAR_REGION_SOUTH_KOREAN";
                    case SPAIN -> "CALENDAR_REGION_SPAIN";
                    case SWEDISH -> "CALENDAR_REGION_SWEDISH";
                    case TAIWAN -> "CALENDAR_REGION_TAIWAN";
                    case THAI -> "CALENDAR_REGION_THAI";
                    case UK -> "CALENDAR_REGION_UK";
                    case US -> "CALENDAR_REGION_US";
                    case VIETNAMESE -> "CALENDAR_REGION_VIETNAMESE";
                };
        return PropertiesAccessor.getCalendarRegionsProperty(calendarRegionName, "NOT_FOUND");
    }
}