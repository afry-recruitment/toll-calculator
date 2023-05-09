namespace TollFeeCalculator
{
  public class FeeInterval
  {
    public TimeOnly From { get; set; }
    public TimeOnly To { get; set; }
    public int Price { get; set; }
  }
}