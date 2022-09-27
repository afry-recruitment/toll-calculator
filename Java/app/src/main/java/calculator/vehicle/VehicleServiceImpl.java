package calculator.vehicle;

import lombok.extern.slf4j.Slf4j;
import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVRecord;

import java.io.FileReader;
import java.io.IOException;
import java.io.Reader;
import java.util.*;
import java.util.concurrent.atomic.AtomicBoolean;

@Slf4j
public class VehicleServiceImpl implements VehicleService
{
    private final Map<String, VehicleType> vehicleTypes;


    protected VehicleServiceImpl(String dataPath)
    {
        Map<String, VehicleType> vehicleTypes;
        try
        {
            vehicleTypes = readVehicleTypes(dataPath);
            if (vehicleTypes.size() > 0) log.info("Successfully parsed vehicles from " + dataPath);
            else log.warn("No values could be read from " + dataPath);
        } catch (IOException ex)
        {
            vehicleTypes = new HashMap<>();
            log.error("File " + dataPath + "  does not exist or can not be read. ");
        }

        this.vehicleTypes = vehicleTypes;
    }

    public VehicleType getVehicleType(String name)
    {
        return this.vehicleTypes.get(name);
    }

    public Set<String> listVehicleTypeNames()
    {
        return Set.copyOf(this.vehicleTypes.keySet());
    }

    public Set<VehicleType> listVehicleTypes()
    {
        return Set.copyOf(this.vehicleTypes.values());
    }

    public boolean isToolFree(String vehicleType)
    {
        return this.vehicleTypes.get(vehicleType)
                                .isTollFree();
    }

    private Map<String, VehicleType> readVehicleTypes(String csvPath) throws IOException
    {
        Map<String, VehicleType> vehicleTypes = new HashMap<>();
        Reader in = new FileReader(csvPath);
        Iterable<CSVRecord> records =
                CSVFormat.DEFAULT.builder().setIgnoreSurroundingSpaces(true).setCommentMarker('#').setHeader().setIgnoreEmptyLines(true)
                                                       .build()
                                                       .parse(in);
        final String typeHeader = "type";
        final String tollFreeHeader = "toll-free";
        AtomicBoolean parsedAtLeastOne = new AtomicBoolean(false);
        records.forEach(it ->
                        {
                            try
                            {
                                vehicleTypes.put(it.get(typeHeader),
                                                 new VehicleType(it.get(typeHeader),
                                                                 Boolean.parseBoolean(it.get(tollFreeHeader))));
                                if (!parsedAtLeastOne.get())
                                {
                                    parsedAtLeastOne.set(true);
                                }
                            } catch (IllegalArgumentException ex)
                            {
                                log.error("Could not correctly parse record: " + csvPath + " at line " +
                                          it.getRecordNumber() + ". " + System.lineSeparator() + "> " + it);
                            } catch (IllegalStateException ex)
                            {
                                log.error("Unexpected formatting of " + csvPath + ". CSV should contain " +
                                          "headers: " + typeHeader + ", " + tollFreeHeader);
                            }
                        });
        return vehicleTypes;
    }


}
