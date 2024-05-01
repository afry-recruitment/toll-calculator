namespace Tollculator.Calculator;

public interface ITollCalculator
{
    Task<int> GetTollFee(DateOnly date, string registrationNumber, IList<TimeOnly> times);
}
