package calculator.exceptions;

public class IllegalFileFormatException extends Exception
{
    public IllegalFileFormatException(String message, Throwable throwable)
    {
        super(message, throwable);
    }
    public IllegalFileFormatException(String message)
    {
        super(message);
    }
}
