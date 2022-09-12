package calculator;

import calculator.vehicles.VehicleType;
import lombok.extern.slf4j.Slf4j;

import java.io.*;
import java.time.LocalDate;
import java.time.ZonedDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.concurrent.*;

@Slf4j
public class TollPassesCsvParser
{
    private TollPassesCsvParser()
    {
    }

    private static ExecutorService executor = null;

    /*
        How should we handle bigger jobs? batches,mt etc possible? Unordered? Vechicle id?
     */
    public static void parseCsv(String csvFile)
    {
        try
        {
            int noThreads = Integer.parseInt(PropertiesService.getSettingsProperty(
                    "NUMBER_OF_CALCULATOR_THREADS",
                    "8"));
            executor = Executors.newFixedThreadPool(noThreads);
            try (BufferedReader br = new BufferedReader(new FileReader(csvFile)))
            {
                String line;
                while ((line = br.readLine()) != null)
                {
                    CalculatorThread thread = new CalculatorThread(line);
                    // do not care for return
                    FutureTask<Void> task = new FutureTask<>(thread);
                    executor.execute(task);
                }

            } catch (IOException ex)
            {
                ex.printStackTrace();
            }
        } catch (NumberFormatException ex)
        {
            log.error("NUMBER_OF_CALCULATOR_THREADS in app-settings.properties has a non integer value. ");
        }
        if (executor != null)
        {
            try
            {
                executor.awaitTermination(30, TimeUnit.SECONDS);
                executor = null;
            } catch (InterruptedException e)
            {
                log.warn("Unexpected shutdown of CalculatorThread execution service. ");
                e.printStackTrace();
            }
        }
    }


/*
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
*/

    /* LinkedBlockingQueue<E> for data waiting to be written
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
        }*/
    private static String[] extractTollFeeDataFromCsvLine(String line) throws IllegalArgumentException
    {
        line = line.trim();
        // get regId and vehicleType as [0] and passes as [1]
        String[] splitOnList = line.split("[{}]"); // todo duoble check
        if (splitOnList.length != 3) throw new IllegalArgumentException(
                "Csv line either has too many lists or the passes list is not properly formatted. " + line);
        List<ZonedDateTime> dates = parseStringForZonedDateTimes(splitOnList[1]);
        // get regId as [0] and vehicleType as [1]
        String[] splitOnComma = splitOnList[0].split(",");
        if (splitOnComma.length != 2) throw new IllegalArgumentException(
                "Csv line does not follow expected " + "format:registrationId,vechicleType,passes" +
                " actual value: " + line);
        String regId = splitOnComma[0];
        VehicleType vehicleType = null;
        try
        {
            vehicleType = VehicleType.valueOf(splitOnComma[1]);
        } catch (IllegalArgumentException ex)
        {
            throw new IllegalArgumentException(
                    "Second field [" + splitOnComma[1] + "]could not be converted to a vehicle type. ", ex);
        }
        return null;
    }

    private static List<ZonedDateTime> parseStringForZonedDateTimes(String datesString)
    {
        String[] dates = datesString.split(",");
        return Arrays.stream(dates)
                     .map(ZonedDateTime::parse)
                     .toList();
    }

    private static class CalculatorThread implements Callable<Void>
    {
        public CalculatorThread(String line)
        {

        }

        @Override
        public Void call() throws Exception
        {
            return null;
        }
    }
}
