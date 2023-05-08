namespace TollFeeCalculator
{
  public class FeeInterval
  {
    public TimeOnly from { get; set; }
    public TimeOnly to { get; set; }
    public int price { get; set; }
  }
}