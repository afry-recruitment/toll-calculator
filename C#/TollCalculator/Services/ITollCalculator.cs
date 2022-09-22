namespace TollFeeCalculator;
public interface ITollCalculator
{
    public int GetTollFee(IVehicle vehicle, DateTime[] date);
}