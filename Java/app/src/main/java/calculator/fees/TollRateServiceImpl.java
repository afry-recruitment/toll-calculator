package calculator.fees;

import calculator.exceptions.IncompleteTollRateTableException;
import lombok.extern.slf4j.Slf4j;
import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVRecord;

import java.io.FileReader;
import java.io.IOException;
import java.io.Reader;
import java.time.LocalTime;
import java.time.format.DateTimeParseException;
import java.time.temporal.ChronoUnit;
import java.util.HashSet;
import java.util.Set;

@Slf4j
public class TollRateServiceImpl implements TollRateService
{
    private final Set<IntervalTollRate> tollRates;

    public TollRateServiceImpl(String dataPath)
    {
        Set<IntervalTollRate> tollRates = null;
        try
        {
            tollRates = readTollRates(dataPath);
            if (tollRates.size() > 0) log.info("Successfully parsed toll-rates from " + dataPath);
            else log.warn("No values could be read from " + dataPath);
        } catch (IOException ex)
        {
            tollRates = new HashSet<>();
            log.error(ex.getMessage());
            log.error("File " + dataPath + "  does not exist or can not be read. ");
        }
        try
        {
            validateTollRateTable(tollRates);
        } catch (IncompleteTollRateTableException ex)
        {
            log.error(ex.getMessage());
        }
        this.tollRates = tollRates;
    }

    private static Set<IntervalTollRate> readTollRates(String csvPath) throws IOException
    {
        Set<IntervalTollRate> rates = new HashSet<>();
        Reader in = new FileReader(csvPath);
        Iterable<CSVRecord> records = CSVFormat.DEFAULT.builder()
                                                       .setHeader()
                                                       .setCommentMarker('#')
                                                       .setIgnoreEmptyLines(true)
                                                       .setIgnoreSurroundingSpaces(true)
                                                       .build()
                                                       .parse(in);
        records.forEach(it ->
                        {
                            try
                            {
                                rates.add(new IntervalTollRate(LocalTime.parse(it.get("start")),
                                                               LocalTime.parse(it.get("end")),
                                                               Integer.parseInt(it.get("rate"))));
                            } catch (DateTimeParseException ex)
                            {
                                log.error("Could not parse date in " + csvPath + " at line " +
                                          it.getRecordNumber() + ". " + System.lineSeparator() + "> " + it);
                            } catch (NumberFormatException ex)
                            {
                                log.error("Could not parse integer rate in " + csvPath + " at line " +
                                          it.getRecordNumber() + ". " + System.lineSeparator() + "> " + it);

                            }
                        });

        return rates;
    }

    @Override
    public int getTollFeeAtTime(final LocalTime localTime)
    {
        return this.tollRates.parallelStream()
                             .filter(it -> it.encompasses(LocalTime.from(localTime)))
                             .findFirst()
                             .orElseThrow(() -> new RuntimeException(
                                     "Toll-rate table does not fully cover a day. " + localTime))
                             .getRate();
    }

    /*
        Looks for holes in toll rate intervals by testing each minute in a day.
     */
    private static void validateTollRateTable(Set<IntervalTollRate> tollRates) throws
                                                                               IncompleteTollRateTableException
    {
        ChronoUnit timeUnit = ChronoUnit.MINUTES;
        // start since the last minute won't be counted
        long coveredTime = 1;
        long timeUnitsPerDay = ChronoUnit.DAYS.getDuration()
                                              .dividedBy(timeUnit.getDuration());
        for (IntervalTollRate tollRate : tollRates)
        {
            coveredTime += tollRate.getStart()
                                   .until(tollRate.getEnd(), ChronoUnit.MINUTES);
        }
        if (coveredTime != timeUnitsPerDay) throw new IncompleteTollRateTableException(
                " Toll-rate table does not fully cover a day. " + "Covered time: " + coveredTime);
        log.info("Toll-rate table is valid, it encompasses a whole day and nothing more.  Covered time: " +
                 coveredTime);
    }
}