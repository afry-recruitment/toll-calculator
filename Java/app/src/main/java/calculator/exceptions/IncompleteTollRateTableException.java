package calculator.exceptions;

public class IncompleteTollRateTableException extends Exception
{
    public IncompleteTollRateTableException(String message, Throwable throwable)
    {
        super(message, throwable);
    }

    public IncompleteTollRateTableException(String message)
    {
        super(message);
    }
}
