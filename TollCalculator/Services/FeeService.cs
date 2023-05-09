
namespace TollFeeCalculator
{
  public class FeeService : IFeeService
  {
    List<FeeInterval> fees = new List<FeeInterval>
      {
      new FeeInterval { From = new TimeOnly(0, 0, 0), To = new TimeOnly(5, 59, 59,999), Price = 0 },
      new FeeInterval { From = new TimeOnly(6, 0, 0), To = new TimeOnly(6, 29, 59,999), Price = 8 },
      new FeeInterval { From = new TimeOnly(6, 30, 0), To = new TimeOnly(6, 59, 59,999), Price = 13 },
      new FeeInterval { From = new TimeOnly(7, 0, 0), To = new TimeOnly(7, 59, 59,999), Price = 18 },
      new FeeInterval { From = new TimeOnly(8, 0, 0), To = new TimeOnly(8, 29, 59,999), Price = 13 },
      new FeeInterval { From = new TimeOnly(8, 30, 0), To = new TimeOnly(14, 59, 59,999), Price = 8 },
      new FeeInterval { From = new TimeOnly(15, 0, 0), To = new TimeOnly(15, 29, 59,999), Price = 13 },
      new FeeInterval { From = new TimeOnly(15, 30, 0), To = new TimeOnly(16, 59, 59,999), Price = 18 },
      new FeeInterval { From = new TimeOnly(17, 0, 0), To = new TimeOnly(17, 59, 59,999), Price = 13 },
      new FeeInterval { From = new TimeOnly(18, 0, 0), To = new TimeOnly(18, 29, 59,999), Price = 8 },
      new FeeInterval { From = new TimeOnly(18, 30, 0), To = new TimeOnly(23, 59, 59,999), Price = 0 }

    };

    public int FeeForTime(TimeOnly actual)
    {
      return fees.First(candidate => actual >= candidate.From && actual <= candidate.To).Price;
    }
  }

}