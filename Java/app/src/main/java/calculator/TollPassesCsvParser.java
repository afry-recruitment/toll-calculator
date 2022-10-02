package calculator;

import calculator.exceptions.IllegalFileFormatException;
import calculator.result.Result;
import calculator.report.ReportFactory;
import calculator.vehicle.Vehicle;
import calculator.vehicle.VehicleType;
import lombok.extern.slf4j.Slf4j;
import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVRecord;

import java.io.*;
import java.time.LocalDate;
import java.time.ZonedDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.LinkedBlockingQueue;
import java.util.concurrent.TimeUnit;
import java.util.stream.Collectors;

@Slf4j
public class TollPassesCsvParser
{
    private ExecutorService executor = null;
    private TollCalculator tollCalculator;
    private LinkedBlockingQueue<String> reports;
    private String reportsPath;

    public TollPassesCsvParser(TollCalculator tollCalculator) throws IOException
    {
        this.tollCalculator = tollCalculator;
        this.reportsPath = PropertiesService.getSettingsProperty("REPORTS_FOLDER", "reports");
        this.reports = new LinkedBlockingQueue<>();
    }

    public void parseCsv(String inputPath) throws IOException
    {
        readCsv(inputPath, reportsPath + File.separator);
    }

    public void parseCsv(String inputPath, String outputPath) throws
                                                              IllegalFileFormatException,
                                                              FileNotFoundException
    {
        try
        {
            readCsv(inputPath, outputPath);
        } catch (IOException e)
        {
            e.printStackTrace();
        }
    }

    public void readCsv(String inputPath, String outputPath) throws IOException
    {
        int noThreads = Integer.parseInt(PropertiesService.getSettingsProperty("NUMBER_OF_CALCULATOR_THREADS",
                                                                               String.valueOf(Runtime.getRuntime()
                                                                                                     .availableProcessors())));
        if (this.executor != null) this.executor.shutdownNow();
        this.executor = Executors.newFixedThreadPool(noThreads);
        // read reports from queue and writes them to outputPath
        File outputFile = buildCsvOutputFile(outputPath);
        this.executor.execute(new ReportWriterThread(outputFile));
        try (BufferedReader reader = new BufferedReader(new FileReader(inputPath), 1048576 * 10))
        {
            CSVFormat csvFormat = CSVFormat.DEFAULT.builder()
                                                   .setIgnoreSurroundingSpaces(true)
                                                   .setCommentMarker('#')
                                                   .setHeader()
                                                   .setIgnoreEmptyLines(true)
                                                   .build();
            Iterable<CSVRecord> records = csvFormat.parse(reader);
            for (CSVRecord record : records)
            {
                CalculatorThread calculatorThread = new CalculatorThread(record.get("vehicleId"),
                                                                         record.get("vehicleType"),
                                                                         record.get("passes"));
                log.info("Thread submitted. ");
                executor.execute(calculatorThread);
            }
        }
        try
        {
            this.executor.awaitTermination(1000, TimeUnit.MILLISECONDS);
        } catch (InterruptedException e)
        {
            log.warn("Interrupted while waiting. Shutting down thread: " + Thread.currentThread()
                                                                                 .getName());
            if (this.executor != null) this.executor.shutdownNow();
        }
    }


    /**
     * Takes a path and determines if the path is full or relative. If relative it will be relative to
     * working directory. It will generate a folder unique name for the file.
     *
     * @param outputPath path where report will be written to. Can be empty, relative or full.
     * @return empty file with unique name at outputPath or in the same parent directory.
     */
    private File buildCsvOutputFile(String outputPath)
    {
        if (outputPath == null) outputPath = "";
        String path;
        String fileName;
        if (outputPath.contains(File.separator) || outputPath.contains("/"))
        {
            // path and filename
            int lastFileSeparatorIndex = outputPath.lastIndexOf(File.separator);
            if (lastFileSeparatorIndex < 0) lastFileSeparatorIndex = outputPath.lastIndexOf("/");
            path = outputPath.substring(0, lastFileSeparatorIndex);
            fileName = outputPath.substring(lastFileSeparatorIndex);
        }
        else
        {
            path = reportsPath;
            fileName = outputPath;
        }
        if (!path.isEmpty()) new File(path).mkdirs();

        if (fileName.isEmpty() || fileName.equals(".csv")) fileName = "toll-report_" + LocalDate.now();
        if (!fileName.endsWith(".csv")) fileName += ".csv";

        String filePath = path + File.separator + fileName;
        log.info("Making output file: " + filePath);
        return makeFileWithUniqueName(filePath, ".csv", 1000);
    }

    private File makeFileWithUniqueName(String filePath, String fileEnding, int noTries)
    {
        File file = new File(filePath);
        if (file.exists())
        {
            String[] splitOnFileEnding = filePath.split(fileEnding);
            int i = 0;
            while (i++ < noTries && file.exists())
            {
                file = new File(splitOnFileEnding[0] + "(" + i + ")" + fileEnding);
                if (i == noTries - 1) throw new RuntimeException(
                        "Could not produce unique filename with " + noTries + " tries. ");
            }
        }
        return file;
    }


    private boolean writeReports(String reportLine, File outputFile)
    {
        log.info("New report line: " + reportLine);
        log.info("Output file: " + outputFile.getAbsolutePath());
        try (PrintWriter pw = new PrintWriter(outputFile))
        {
            pw.println(reportLine);
            return true;
        } catch (FileNotFoundException ex)
        {
            log.error("Could not write report file: " + outputFile);
            return false;
        }
    }

    private List<ZonedDateTime> parseSpaceSeparatedPasses(String passes)
    {
        passes = passes.replaceAll("[{}]", "");
        return Arrays.stream(passes.split(" "))
                     .parallel()
                     .map(ZonedDateTime::parse)
                     .collect(Collectors.toList());
    }

    //todo
    public boolean isRunning()
    {
        return !this.executor.isShutdown();
    }

    private class CalculatorThread implements Runnable
    {
        private Vehicle vehicle;
        private String passes;


        /**
         * @param vehicleId   vehicle registration id
         * @param vehicleType string representation of vehicle type
         * @param passes      space seperated list of toll passages format: 2011-12-03T10:15:30+01:00
         *                    (ISO-8601+offset id)
         */
        public CalculatorThread(String vehicleId, String vehicleType, String passes)
        {
            VehicleType type = tollCalculator.vehicleService()
                                             .getVehicleType(vehicleType);
            this.vehicle = new Vehicle(type, vehicleId);
            this.passes = passes;
        }

        @Override
        public void run()
        {
            List<ZonedDateTime> dateTimes = parseSpaceSeparatedPasses(this.passes);
            try
            {
                Result feeResult = tollCalculator.getTollFee(vehicle, dateTimes);

                reports.put(ReportFactory.getReport(ReportFactory.FULL_REPORT)
                                         .makeReport(feeResult));

            } catch (InterruptedException ex)
            {
                log.error(ex.getMessage());
            }
        }
    }

    /**
     * Polls reports of for reports to print to a given file path.
     */
    private class ReportWriterThread implements Runnable
    {
        private final File outputFile;

        public ReportWriterThread(File outPutFile)
        {
            super();
            this.outputFile = outPutFile;
        }

        /**
         * Wait for first element to be added to reports
         *
         * @param timeout
         * @return
         */
        private boolean waitForFirstReport(long timeout)
        {
            boolean noReports = true;
            boolean hasFailed = false;
            try
            {
                while (noReports && !hasFailed)
                {
                    Thread.sleep(10);
                    if (reports.isEmpty())
                    {
                        timeout -= 10;
                        if (timeout < 0) hasFailed = true;
                    }
                    else noReports = false;
                }
                if (hasFailed) log.warn(
                        "ReportWriterThread " + Thread.currentThread() + " timed out waiting" +
                        " for reports. ");
            } catch (InterruptedException e)
            {
                log.error("ReportWriterThread " + Thread.currentThread() + " was interrupted. ");
                hasFailed = true;
            }
            log.info("First report found: " + (!noReports && !hasFailed));
            return !noReports && !hasFailed;
        }

        /**
         * Wait for new elements to be added to reports
         *
         * @param timeout
         * @return
         */
        private boolean waitForNewReports(long timeout)
        {
            boolean interrupted = false;
            try
            {
                while (reports.isEmpty() && timeout > 0)
                {
                    Thread.sleep(10);
                    timeout -= 10;
                }
            } catch (InterruptedException e)
            {
                log.error("ReportWriterThread " + Thread.currentThread() + " was interrupted. ");
                interrupted = true;
            }
            log.info("Found new reports: " + (!reports.isEmpty() && !interrupted));
            return !reports.isEmpty() && !interrupted;
        }

        @Override
        public void run()
        {
            List<String> lines;
            boolean hasWork = true;
            // wait for 1 second until list is starting to populate
            if (!waitForFirstReport(1000)) return;
            log.info("Has waitForFirstReport");
            int it = 0;
            while (hasWork)
            {
                log.info("ReportWriterThread, iteration: " + it++);
                if (waitForNewReports(1000))
                {
                    log.info("Has waited for new reports. ");
                    lines = new ArrayList<>();
                    reports.drainTo(lines);
                    log.info("Writing " + lines.size() + " to reportFile. ");
                    writeReports(lines.stream()
                                      .reduce("", (a, b) -> a + b), outputFile);
                }
                else hasWork = false;
            }
            log.info("ReportWriterThread finished. ");
            log.info(reports.stream()
                            .toList()
                            .toString());
        }
    }
}