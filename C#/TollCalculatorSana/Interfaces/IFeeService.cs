namespace TollCalculatorSana.Interfaces;

public interface IFeeService
{
    public int GetTollFee(string vehicleRegNum, string vehicleType, List<DateTime> dates);
}
