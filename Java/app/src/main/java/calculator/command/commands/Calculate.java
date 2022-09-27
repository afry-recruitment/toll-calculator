package calculator.command.commands;

import calculator.TollCalculator;
import calculator.TollPassesCsvParser;
import calculator.calendar.CalendarRegion;
import calculator.calendar.CalendarService;
import calculator.calendar.CalendarServiceImpl;
import calculator.exceptions.IllegalFileFormatException;
import calculator.fees.TollRateService;
import calculator.fees.TollRateServiceFactory;
import calculator.vehicle.VehicleService;
import calculator.vehicle.VehicleServiceFactory;
import lombok.extern.slf4j.Slf4j;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.time.LocalDate;
import java.time.LocalTime;
import java.util.List;

import static calculator.PropertiesService.*;

@Slf4j
public class Calculate implements Command
{
    public final static String FLAG_INPUT = "-in";
    public final static String FLAG_OUTPUT = "-out";
    public final static String FLAG_REGION = "-r";

    private String responseMessage;
    private boolean viable;

    private CalendarService calendarService;
    private VehicleService vehicleService;
    private TollRateService tollRateService;
    private TollCalculator tollCalculator;
    private TollPassesCsvParser csvParser;
    private String input;
    private String output;

    public Calculate(List<String> args)
    {
        this.viable = true;
        this.vehicleService = getVehicleService();
        this.tollRateService = getTollRateService();
        String input;
        String output;
        String calendarRegion;
        if (args == null || args.isEmpty())
        {
            calendarRegion = "SWEDISH";
            input = "data/toll-passes.csv";
            output = getSettingsProperty("REPORTS_FOLDER", "reports/") + "demo" + LocalDate.now() + "_" +
                     LocalTime.now()
                              .getHour() + "." + LocalTime.now()
                                                          .getMinute();
        }
        else if (checkInputOutputRegion(args))
        {
            input = Command.getArgument(FLAG_INPUT, args)
                           .orElseThrow();
            output = Command.getArgument(FLAG_OUTPUT, args)
                            .orElseThrow();
            calendarRegion = Command.getArgument(FLAG_REGION, args)
                                    .orElseThrow();
        }
        else if (checkInputRegion(args))
        {
            input = Command.getArgument(FLAG_INPUT, args)
                           .orElseThrow();
            output = getSettingsProperty("REPORTS_FOLDER", "reports/") + "report" + LocalDate.now() + "_" +
                     LocalTime.now()
                              .getHour() + "." + LocalTime.now()
                                                          .getMinute();
            calendarRegion = Command.getArgument(FLAG_REGION, args)
                                    .orElseThrow();
        }
        else if (checkInput(args))
        {
            input = Command.getArgument(FLAG_INPUT, args)
                           .orElseThrow();
            output = getSettingsProperty("REPORTS_FOLDER", "reports/") + "report" + LocalDate.now() + "_" +
                     LocalTime.now()
                              .getHour() + "." + LocalTime.now()
                                                          .getMinute();
            calendarRegion = "SWEDISH";
        }
        else if (checkInputOutput(args))
        {
            input = Command.getArgument(FLAG_INPUT, args)
                           .orElseThrow();
            output = getSettingsProperty("REPORTS_FOLDER", "reports/") + "report" + LocalDate.now() + "_" +
                     LocalTime.now()
                              .getHour() + "." + LocalTime.now()
                                                          .getMinute();
            calendarRegion = "SWEDISH";
        }
        else
        {
            this.responseMessage = "Could not parse argument. ";
            this.viable = false;
            return;
        }
        this.calendarService = getCalendarService(calendarRegion);
        this.tollCalculator =
                new TollCalculator(this.calendarService, this.vehicleService, this.tollRateService);
        this.input = input;

        this.output = output;

        try
        {
            this.csvParser = new TollPassesCsvParser(this.tollCalculator);
        } catch (IOException e)
        {
            e.printStackTrace();
        }

    }

    private CalendarServiceImpl getCalendarService(String region)
    {
        CalendarRegion calendarRegion = CalendarRegion.valueOf(region.toUpperCase());
        return new CalendarServiceImpl(calendarRegion);
    }

    private CalendarServiceImpl getCalendarService()
    {
        String region = getSettingsProperty("CALENDAR_REGION", "SWEDISH");
        CalendarRegion calendarRegion = CalendarRegion.valueOf(region.toUpperCase());
        return new CalendarServiceImpl(calendarRegion);
    }

    private VehicleService getVehicleService()
    {
        String vehicleTypePath = getSettingsProperty("VEHICLE_TYPES_PATH", "data/vehicle-types.csv");
        return VehicleServiceFactory.getVehicleService(vehicleTypePath);
    }

    private TollRateService getTollRateService()
    {
        String tollRatesPath = getSettingsProperty("RATES_PATH", "data/toll-rates.csv");
        return TollRateServiceFactory.getTollRateService(tollRatesPath);
    }

    private boolean checkInputOutputRegion(List<String> args)
    {
        boolean valid;
        valid = args.contains(FLAG_INPUT);
        valid &= args.size() > args.indexOf(FLAG_INPUT);
        valid &= args.contains(FLAG_REGION);
        valid &= args.size() > args.indexOf(FLAG_REGION);
        valid &= args.contains(FLAG_OUTPUT);
        valid &= args.size() > args.indexOf(FLAG_OUTPUT);
        valid &= args.size() == 6;
        return valid;
    }

    private boolean checkInput(List<String> args)
    {
        boolean valid;
        valid = args.contains(FLAG_INPUT);
        valid &= args.size() > args.indexOf(FLAG_INPUT);
        valid &= args.size() == 2;
        return valid;
    }

    private boolean checkInputOutput(List<String> args)
    {
        boolean valid;
        valid = args.contains(FLAG_INPUT);
        valid &= args.size() > args.indexOf(FLAG_INPUT);
        valid &= args.contains(FLAG_OUTPUT);
        valid &= args.size() > args.indexOf(FLAG_OUTPUT);
        valid &= args.size() == 4;
        return valid;
    }

    private boolean checkInputRegion(List<String> args)
    {
        boolean valid;
        valid = args.contains(FLAG_INPUT);
        valid &= args.size() > args.indexOf(FLAG_INPUT);
        valid &= args.contains(FLAG_REGION);
        valid &= args.size() > args.indexOf(FLAG_REGION);
        valid &= args.size() == 4;
        return valid;
    }

    @Override
    public boolean isViable()
    {
        return this.viable;
    }

    @Override
    public boolean execute()
    {
        try
        {
            if (isViable())
            {
                this.csvParser.parseCsv(this.input, this.output);
                System.out.println(
                        "Successfully parsed and calculated " + this.input + " report available " + "at: " +
                        this.output);
                return true;
            }
        } catch (IllegalFileFormatException e)
        {
            e.printStackTrace();
        } catch (FileNotFoundException e)
        {
            e.printStackTrace();
        }
        return false;
    }
}
