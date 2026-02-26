namespace TollFeeCalculator.Interfaces;

public interface ITollFeeCalculator
{
    public int CalculateFee(IEnumerable<DateTime> timeStamps);
    
}