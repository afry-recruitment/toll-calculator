package calculator.command;

import calculator.command.commands.*;

import java.util.List;

public class CommandInterpreter
{
    public final static String CALCULATE_COMMAND = "calculate";
    public final static String HELP_COMMAND = "help";
//    public final static String UNKNOWN_COMMAND = "help";

    public static Command interpret(List<String> args)
    {
        if (args == null || args.isEmpty()) return new Default();
        if (args.get(0)
                .equalsIgnoreCase(CALCULATE_COMMAND)) return new Calculate(args.subList(1, args.size()));
        if (args.get(0)
                .equalsIgnoreCase(HELP_COMMAND)) return new Help(args.subList(1, args.size()));
        else return new Unknown();
    }


}
