package calculator.command.commands;

public class Default implements Command
{
    public Default(){
        System.out.println("No command given. Try >help");
    }
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
