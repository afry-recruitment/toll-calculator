package calculator.calendar;

import calculator.PropertiesService;
import lombok.extern.slf4j.Slf4j;

import java.io.*;
import java.time.LocalDate;
import java.time.format.DateTimeParseException;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;

import java.util.List;

@Slf4j
public class CalendarService
{
    private List<LocalDate> holidays;
    private final String HOLIDAY_DATES_FILE_NAME = "data/holidays";

    public CalendarService(CalendarRegion calendarRegion)
    {
        loadHolidays(calendarRegion);
    }

    private void loadHolidays(CalendarRegion calendarRegion)
    {
        String lastUpdatedHolidays =
                PropertiesService.getSettingsProperty("LAST_UPDATED_HOLIDAYS", "1970-01-01");
        // if the holiday list has not been checked in the last 7
        if (LocalDate.now()
                     .plus(7, ChronoUnit.DAYS)
                     .isBefore(LocalDate.parse(lastUpdatedHolidays)) ||
            !new File(HOLIDAY_DATES_FILE_NAME).exists())
        {
            log.info("List of holidays is out of date or file does not exist. Checking API. ");
            CalenderHandler calenderHandler = getCalenderHandler();
            this.holidays = calenderHandler.getHolidays(calendarRegion);
            if (writeHolidays(this.holidays)) PropertiesService.setSettingsProperty("LAST_UPDATED_HOLIDAYS",
                                                                                    LocalDate.now()
                                                                                             .toString());
        }
        // if the holiday list is up-to-date use the stored one
        else
        {
            try
            {
                this.holidays = readHolidays();
            } catch (IOException e)
            {
                log.info("Could not find any holidays. Creating new file. ");
                if (writeHolidays(this.holidays)) PropertiesService.setSettingsProperty(
                        "LAST_UPDATED_HOLIDAYS",
                        LocalDate.now()
                                 .toString());
            }
        }
    }

    /**
     * Returns a copy of holidays from the region which it was instantiated with.
     *
     * @return list holidays as LocalDate
     */
    public List<LocalDate> getHolidays()
    {
        return new ArrayList<>(holidays);
    }

    public boolean isHoliday(LocalDate date)
    {
        return holidays.stream()
                       .anyMatch(h -> h.equals(date));
    }

    private static CalenderHandler getCalenderHandler()
    {
        return new GoogleCalendarHandler();
    }


    private List<LocalDate> readHolidays() throws IOException
    {
        List<LocalDate> dates = new ArrayList<>();
        try (BufferedReader br = new BufferedReader(new FileReader(HOLIDAY_DATES_FILE_NAME)))
        {
            String line;
            while ((line = br.readLine()) != null)
            {
                try
                {
                    dates.add(LocalDate.parse(line));
                } catch (DateTimeParseException ex)
                {
                    log.error("Could not parse line: \n" + line + "\n as a date. ");
                }
            }
            log.info("Parsed holidays file successfully. ");
            return dates;
        }
    }

    private boolean writeHolidays(List<LocalDate> dates)
    {

        File outputFile = new File(HOLIDAY_DATES_FILE_NAME);
        outputFile.delete();
        try (PrintWriter pw = new PrintWriter(outputFile))
        {
            dates.forEach(pw::println);
            log.info("Updated " + HOLIDAY_DATES_FILE_NAME);
            return true;
        } catch (FileNotFoundException ex)
        {
            log.error(
                    "Could not create file: " + HOLIDAY_DATES_FILE_NAME + " with error: " + ex.getMessage());
            return false;
        }
    }

    public void setHolidays(List<LocalDate> holidays)
    {
        this.holidays = holidays;
    }
}
