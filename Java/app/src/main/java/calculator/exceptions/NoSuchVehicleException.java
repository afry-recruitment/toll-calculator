package calculator.exceptions;

public class NoSuchVehicleException extends Exception
{
    public NoSuchVehicleException(String message, Throwable throwable)
    {
        super(message, throwable);
    }

    public NoSuchVehicleException(String message)
    {
        super(message);
    }
}
