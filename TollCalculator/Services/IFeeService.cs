namespace TollFeeCalculator
{
  public interface IFeeService
  {
    int FeeForTime(TimeOnly actual);
  }
}