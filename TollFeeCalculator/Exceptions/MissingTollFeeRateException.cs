namespace TollFeeCalculator.Exceptions;

public class MissingTollFeeRateException : Exception
{
    public MissingTollFeeRateException() { }
    public MissingTollFeeRateException(string message) : base(message) { }
    public MissingTollFeeRateException(TimeOnly passageTime) : base($"Missing toll fee rate for passage time: {passageTime}") { }
}