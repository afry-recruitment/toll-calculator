package calculator.command.commands;

import java.util.List;
import java.util.Optional;

public interface Command
{
    /*    private List<?> args;
        private boolean correctSyntax;
        private boolean missingDependencies;

        public Command(List<?> args, boolean correctSyntax, boolean missingDependencies)
        {
            this.args = args;
            this.correctSyntax = correctSyntax;
            this.missingDependencies = missingDependencies;
        }*/
    boolean isViable();

    boolean execute();

//    String getReason();

    static Optional<String> getArgument(String argKey, List<String> args)
    {
        String response = null;
        int argKeyIndex = args.indexOf(argKey);
        if (argKeyIndex >= 0)
        {
            if (args.size() > argKeyIndex)
            {
                response = args.get(argKeyIndex + 1);
            }
        }
        return Optional.ofNullable(response);
    }
}
