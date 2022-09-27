package calculator.command.commands;

import calculator.calendar.CalendarRegion;
import lombok.extern.slf4j.Slf4j;

import java.util.Arrays;
import java.util.List;

@Slf4j
public class Help implements Command
{
    public final static String FLAG_REGION = "-r";
    private String response;
    private boolean viable;

    public Help(List<String> args)
    {
        this.viable = true;
        if (args == null || args.isEmpty())
        {
            this.response = "Example: " + System.lineSeparator() + "> -in <input csv> [-out" +
                            " <name of report file>] -r <holiday region> " + System.lineSeparator() + "For " +
                            "selection of possible regions call TollCalculator.jar with:" +
                            System.lineSeparator() + "> help regions";
        }
        else if (args.size() == 1 && args.contains(FLAG_REGION))
        {
            this.response = "Possible regions are: " + Arrays.toString(CalendarRegion.values());
        }
        else
        {
            this.viable = false;
            this.response = "Error. ";
        }
    }

    @Override
    public boolean isViable()
    {
        return this.viable;
    }

    @Override
    public boolean execute()
    {
        if (!isViable()) return false;
        log.info(this.response);
        return true;
    }

}
