package calculator.command.commands;

public class Unknown implements Command
{
    @Override
    public boolean isViable()
    {
        return false;
    }

    @Override
    public boolean execute()
    {
        return false;
    }
}
