using System.IO.Enumeration;
using System.Security.AccessControl;
using System;
using System.Linq;
namespace TollFeeCalculator
{
  public class FeeService : IFeeService
  {
    List<FeeInterval> fees = new List<FeeInterval>
      {
      new FeeInterval { from = new TimeOnly(0, 0, 0), to = new TimeOnly(5, 59, 59), price = 0 },
      new FeeInterval { from = new TimeOnly(6, 0, 0), to = new TimeOnly(6, 29, 59), price = 8 },
      new FeeInterval { from = new TimeOnly(6, 30, 0), to = new TimeOnly(6, 59, 59), price = 13 },
      new FeeInterval { from = new TimeOnly(7, 0, 0), to = new TimeOnly(7, 59, 59), price = 18 },
      new FeeInterval { from = new TimeOnly(8, 0, 0), to = new TimeOnly(8, 29, 59), price = 13 },
      new FeeInterval { from = new TimeOnly(8, 30, 0), to = new TimeOnly(14, 59, 59), price = 8 },
      new FeeInterval { from = new TimeOnly(15, 0, 0), to = new TimeOnly(15, 29, 59), price = 13 },
      new FeeInterval { from = new TimeOnly(15, 30, 0), to = new TimeOnly(16, 59, 59), price = 18 },
      new FeeInterval { from = new TimeOnly(17, 0, 0), to = new TimeOnly(17, 59, 59), price = 13 },
      new FeeInterval { from = new TimeOnly(18, 0, 0), to = new TimeOnly(18, 29, 59), price = 8 },
      new FeeInterval { from = new TimeOnly(18, 30, 0), to = new TimeOnly(23, 59, 59), price = 0 }

    };

    public int FeeForTime(TimeOnly actual)
    {
      return fees.First(candidate => candidate.from >= actual && candidate.to <= actual).price;
    }
  }

}