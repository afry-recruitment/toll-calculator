package calculator;

import calculator.calendar.CalendarService;
import calculator.vehicles.VehicleType;
import lombok.SneakyThrows;
import lombok.extern.slf4j.Slf4j;

import java.io.*;
import java.time.LocalDate;
import java.time.ZonedDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.concurrent.*;

@Slf4j
public class TollPassesCsvParser
{
    private ExecutorService executor = null;
    private TollCalculator tollCalculator = null;

    public TollPassesCsvParser(CalendarService calendarService)
    {
        this.tollCalculator = new TollCalculator(calendarService);
    }

    public void parseCsv(String csvFile)
    {
        try
        {
            int noThreads = Integer.parseInt(PropertiesService.getSettingsProperty(
                    "NUMBER_OF_CALCULATOR_THREADS",
                    "8"));
            this.reports = new LinkedBlockingQueue<>();
            executor = Executors.newFixedThreadPool(noThreads);

            FeederThread feederThread = new FeederThread();
/*            try
            {*/
                executor.execute(feederThread);
/*            } catch (InterruptedException | ExecutionException ex)
            {
                log.error(ex.getMessage());
//                e.printStackTrace();
            }*/
            // throw first line
            boolean firstLine = true;
            try (BufferedReader br = new BufferedReader(new FileReader(csvFile)))
            {
                String line;
                while ((line = br.readLine()) != null)
                {
                    log.info(line);
                    if (firstLine)
                    {
                        firstLine = false;
                        continue;
                    }
                    CalculatorThread calculatorThread = new CalculatorThread(line);
                    // do not care for return
                    //                    FutureTask task = new FutureTask(thread);
                    log.info("Thread submitted. ");
                    executor.execute(calculatorThread)
                      /*      .get()*/;
                }
            } catch (IOException ex)
            {
                log.error(ex.getMessage());
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
                executor.awaitTermination(5, TimeUnit.SECONDS);
                executor = null;
                log.info("ExecutorService: " + executor + " has finished. ");
            } catch (InterruptedException e)
            {
                log.warn("Unexpected shutdown of CalculatorThread execution service. ");
                e.printStackTrace();
            }
        }
    }

    private LinkedBlockingQueue<String> reports;

    private boolean writeReports(String s)
    {

        LocalDate now = LocalDate.now();
        File outputFile = new File("data/report_" + now + ".csv");
        int i = 0;
        while (i++ < 1000 && outputFile.exists())
        {

            outputFile = new File("data/report_" + now + "(" + i + ").csv");
        }
        try (PrintWriter pw = new PrintWriter(outputFile))
        {
            pw.println(s);
            //            reports.forEach(pw::println);
            //            log.info("Updated " + HOLIDAY_DATES_FILE_NAME);
            return true;
        } catch (FileNotFoundException ex)
        {
            //            log.error(
            //                    "Could not create file: " + HOLIDAY_DATES_FILE_NAME + " with error: " + ex.getMessage());
            return false;
        }
    }

    private String extractTollFeeDataFromCsvLine(String line) throws IllegalArgumentException
    {
        // get regId and vehicleType as [0] and passes as [1]
        String[] splitOnList = line.trim()
                                   .split("[{}]"); // todo double check
        if (splitOnList.length != 2) throw new IllegalArgumentException(
                "Csv line either has too many lists or the passes list is not properly formatted. " + line);
        List<ZonedDateTime> dates = parseStringForZonedDateTimes(splitOnList[1]);
        // get regId as [0] and vehicleType as [1]
        String[] splitOnComma = splitOnList[0].split(",");
        if (splitOnComma.length != 2) throw new IllegalArgumentException(
                "Csv line does not follow expected " + "format:registrationId,vehicleType,passes" +
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
        int fee = this.tollCalculator.getTollFee(vehicleType, dates);
        return TollReportService.buildCsvReportLine(regId, dates.size(), fee);
    }

    private static List<ZonedDateTime> parseStringForZonedDateTimes(String datesString)
    {
        String[] dates = datesString.split(",");
        return Arrays.stream(dates)
                     .map(ZonedDateTime::parse)
                     .toList();
    }

    private class CalculatorThread implements Runnable
    {
        private String line;

        public CalculatorThread(String line)
        {
            this.line = line;
        }

        @Override
        public void run()
        {
            try
            {
                reports.put(extractTollFeeDataFromCsvLine(line));
            } catch (InterruptedException ex)
            {
                log.error(ex.getMessage());
                //                e.printStackTrace();
            }
        }
    }

    private class FeederThread implements Runnable
    {
        @Override
        public void run()
        {
            List<String> lines = new ArrayList<>();
            //            boolean reportsLeft = true;
            while (true)
            {
                if (reports.isEmpty())
                {
                    try
                    {
                        Thread.sleep(75);
                        if (reports.isEmpty())
                        {
                            break;
                        }
                    } catch (InterruptedException e)
                    {
                        e.printStackTrace();
                        break;
                    }
                }
                reports.drainTo(lines);
                writeReports(lines.stream()
                                  .reduce("", (a, b) -> a + b));
            }
        }
    }
}
