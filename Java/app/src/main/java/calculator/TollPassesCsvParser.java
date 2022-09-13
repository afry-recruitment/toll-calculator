package calculator;

import calculator.Reports.CsvReport;
import calculator.calendar.CalendarService;
import calculator.exceptions.IllegalFileFormatException;
import calculator.vehicles.VehicleType;
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
    private LinkedBlockingQueue<String> reports;
    private File inputFile = null;
    private File outputFile = null;
    private String reportsPath;

    public TollPassesCsvParser(CalendarService calendarService)
    {
        this.reportsPath = PropertiesService.getSettingsProperty("REPORTS_FOLDER", "reports");
        this.tollCalculator = new TollCalculator(calendarService);
    }

    public void parseCsv(String inputPath) throws IllegalFileFormatException, FileNotFoundException
    {
        parseCsv(inputPath, reportsPath + File.separator);
    }

    public void parseCsv(String inputPath, String outputPath) throws
                                                              IllegalFileFormatException,
                                                              FileNotFoundException
    {
        loadCsvInputFile(inputPath);
        loadCsvOutputFile(outputPath);
        try
        {
            int noThreads = Integer.parseInt(PropertiesService.getSettingsProperty(
                    "NUMBER_OF_CALCULATOR_THREADS",
                    "8"));
            this.reports = new LinkedBlockingQueue<>();
            executor = Executors.newFixedThreadPool(noThreads);
            // read reports from queue and writes them to outputPath
            ReportWriterThread reportWriterThread = new ReportWriterThread();
            executor.execute(reportWriterThread);
            // throw first line
            boolean firstLine = true;
            try (BufferedReader br = new BufferedReader(new FileReader(this.inputFile)))
            {
                String line;
                while ((line = br.readLine()) != null)
                {
                    log.info(line);
                    // skip headers on first line
                    if (firstLine)
                    {
                        firstLine = false;
                        continue;
                    }
                    CalculatorThread calculatorThread = new CalculatorThread(line);
                    log.info("Thread submitted. ");
                    executor.execute(calculatorThread);
                }
            } catch (IOException ex)
            {
                log.error(ex.getMessage());
                ex.printStackTrace();
            }
        } catch (NumberFormatException ex)
        {
            log.error("Property NUMBER_OF_CALCULATOR_THREADS in app-settings.properties has a non integer " +
                      "value. ");
        }
        if (executor != null)
        {
            try
            {
                executor.awaitTermination(5, TimeUnit.SECONDS);
                executor.shutdown();
                log.info("ExecutorService: " + executor + "is shutdown: " + executor.isShutdown());
            } catch (InterruptedException e)
            {
                log.warn("Unexpected shutdown of CalculatorThread execution service. ");
                e.printStackTrace();
            }
        }
    }

    //todo
    public boolean isRunning()
    {
        return !this.executor.isShutdown();
    }

    private void loadCsvInputFile(String inputPath) throws FileNotFoundException, IllegalFileFormatException
    {
        File tmpInputFile = new File(inputPath);
        if (!tmpInputFile.exists()) throw new FileNotFoundException("Could not locate " + inputPath);
        if (!inputPath.endsWith(".csv"))
            throw new IllegalFileFormatException("File has to be of typ CSV and end with the .csv suffix. ");
        this.inputFile = tmpInputFile;
    }

    private void loadCsvOutputFile(String outputPath)
    {
        String path;
        String fileName;
        if (outputPath.contains(File.separator))
        {
            // path and filename
            int lastFileSeparatorIndex = outputPath.lastIndexOf(File.separator);
            path = outputPath.substring(0, lastFileSeparatorIndex);
            fileName = outputPath.substring(lastFileSeparatorIndex);

        }else{
            path = reportsPath;
            fileName = outputPath;
        }
        if (!path.isEmpty()) new File(path).mkdirs();
        if(fileName.isEmpty()) fileName = "toll-report_" + LocalDate.now();
        if(!fileName.endsWith(".csv")) fileName += ".csv";
        this.outputFile = makeFileWithUniqueName(path+File.separator+fileName, ".csv", 1000);

/*        if (outputPath.endsWith(File.separator)) outputPath += "toll-report_" + LocalDate.now();
        if (!outputPath.endsWith(".csv")) outputPath += ".csv";

        // simple but dull check to see if file tree specified
        // if not put report in /reports
        if (outputPath.contains(File.separator))
        {
            if (outputPath.endsWith(File.separator + ".csv"))
            {

            }
            this.outputFile = makeFileWithUniqueName(outputPath, ".csv", 1000);
        }
        else new File(reportsPath).mkdir();
        {
            if (outputPath.equals(".csv")) outputPath = LocalDate.now() + outputPath;
            this.outputFile = makeFileWithUniqueName(reportsPath + outputPath, ".csv", 1000);
        }*/
    }

    public File makeFileWithUniqueName(String filePath, String fileEnding, int noTries)
    {
        File file = new File(filePath);
        if (file.exists())
        {
            String[] splitOnFileEnding = filePath.split(fileEnding);
            int i = 0;
            while (i++ < noTries && file.exists())
            {
                file = new File(splitOnFileEnding[0] + "(" + i + ")" + splitOnFileEnding[1]);
                if (i == noTries - 1) throw new RuntimeException(
                        "Could not produce unique filename with " + noTries + " tries. ");
            }
        }
        return file;
    }


    private boolean writeReports(String reportLine)
    {
        try (PrintWriter pw = new PrintWriter(this.outputFile))
        {
            pw.println(reportLine);
            return true;
        } catch (FileNotFoundException ex)
        {
            log.error("Could not write report file: " + this.outputFile);
            return false;
        }
    }

    private CsvReport extractTollFeeDataFromCsvLine(String line) throws IllegalArgumentException
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
                    "Second field [" + splitOnComma[1] + "] could not be converted to a vehicle type. ", ex);
        }
        int fee = this.tollCalculator.getTollFee(vehicleType, dates);
        return new CsvReport(regId, dates.size(), fee);
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
                reports.put(extractTollFeeDataFromCsvLine(line).toString());
            } catch (InterruptedException ex)
            {
                log.error(ex.getMessage());
            }
        }
    }

    /**
     * Polls outer reports of type LinkedBlockingQueue for reports to print to a given file path.
     */
    private class ReportWriterThread implements Runnable
    {
        @Override
        public void run()
        {
            List<String> lines = new ArrayList<>();
            boolean hasWork = true;
            boolean hasWaited = false;
            int noPolls = 10;
            while (hasWork)
            {
                if (reports.isEmpty())
                {
                    try
                    {
                        Thread.sleep(10);
                        hasWaited = true;
                        --noPolls;
                        if (noPolls == 0) hasWork = false;
                    } catch (InterruptedException e)
                    {
                        log.error("ReportWriterThread got interrupted in its execution. ");
                        e.printStackTrace();
                        break;
                    }
                }
                else
                {
                    if (hasWaited)
                    {
                        noPolls = 10;
                        hasWaited = false;
                    }
                    reports.drainTo(lines);
                    log.info("Writing " + lines.size() + " to reportFile. ");
                    writeReports(lines.stream()
                                      .reduce("", (a, b) -> a + b));
                }
            }
        }
    }
}
