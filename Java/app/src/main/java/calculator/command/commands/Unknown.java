package calculator.command.commands;

import lombok.extern.slf4j.Slf4j;

@Slf4j
public class Unknown implements Command
{
public Unknown(){
    System.out.println("Unknown command. Try >help");
}
    @Override
    public boolean isViable()
    {
        return false;
    }

    @Override
    public boolean execute()
    {
        log.warn("Unknown command can not execute. ");
        return false;
    }
}
