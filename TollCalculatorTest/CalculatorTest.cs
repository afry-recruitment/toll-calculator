using System.Reflection;
using TollFeeCalculator;

namespace TollCalculatorTest;

[TestClass]
public class CalculatorTest
{
  [TestMethod]
  public void CallingWithVehicleNullShouldTrowExeption()
  {

    DateTime[] dates = Array.Empty<DateTime>();
    Assert.ThrowsException<ArgumentOutOfRangeException>(() => setup().TollFee(null, dates));
  }

  [TestMethod]
  public void CallingWithNegativeVehicleTypeIdhouldTrowExeption()
  {

    DateTime[] dates = Array.Empty<DateTime>();
    Assert.ThrowsException<ArgumentOutOfRangeException>(() => setup().TollFee(-1, dates));
  }

  [TestMethod]
  public void CallingWithAnArrayOfDidfferentDateshouldTrowExeption()
  {

    var dates = new DateTime[] {
         new DateTime(2003, 2, 12, 6, 23, 0),
         new DateTime(2003, 3, 12,23, 23, 0)
    };
    Assert.ThrowsException<ArgumentOutOfRangeException>(() => setup().TollFee(-1, dates));
  }

  [TestMethod]
  public void CallingWithTollFreeVehicleShouldReturnZero()
  {

    var dates = new DateTime[] {
         new DateTime(2003, 2, 12, 6, 23, 0),
         new DateTime(2003, 2, 12,23, 23, 0)
    };
    Assert.AreEqual(0, setup().TollFee(3, dates));
  }
  [TestMethod]
  public void AVehicleShouldOnlyBeChargedOnceAnHour()
  {

    var dates = new DateTime[] {
         new DateTime(2023, 2, 13, 6, 02, 0),
         new DateTime(2023, 2, 13,6, 23, 0)
    };
    Assert.AreEqual(8, setup().TollFee(6, dates));
  }

  [TestMethod]
  public void InCaseOfMultipleFeesInSamePeriodHighestRateShouldBeApplied()
  {

    var dates = new DateTime[] {
         new DateTime(2023, 2, 13, 6, 02, 0),
         new DateTime(2023, 2, 13,6, 32, 0)
    };
    Assert.AreEqual(13, setup().TollFee(6, dates));
  }

  [TestMethod]
  public void RateCannotGoAbove60()
  {

    var dates = new DateTime[] {
         new DateTime(2023, 2, 13, 6, 02, 0),
         new DateTime(2023, 2, 13,7, 3, 0),
         new DateTime(2023, 2, 13, 8, 4, 0),
         new DateTime(2023, 2, 13,9, 32, 0),
         new DateTime(2023, 2, 13, 15, 32, 0)

    };
    Assert.AreEqual(60, setup().TollFee(6, dates));
  }

  [TestMethod]
  public void EdgeCaseOneDate()
  {

    var dates = new DateTime[] {
         new DateTime(2023, 2, 13, 6, 02, 0),

    };
    Assert.AreEqual(8, setup().TollFee(6, dates));
  }

  [TestMethod]
  public void EdgeCaseMultiplePassesButAllWithinAnHour()
  {

    var dates = new DateTime[] {
         new DateTime(2023, 2, 13, 6, 02, 0),
         new DateTime(2023, 2, 13,7, 01, 0),
         new DateTime(2023, 2, 13, 8, 0, 0),
         new DateTime(2023, 2, 13,8, 59, 0),
         new DateTime(2023, 2, 13, 9, 58, 0)

    };
    Assert.AreEqual(18, setup().TollFee(6, dates));
  }






  private TollFeeCalculator.Calculator setup()
  {
    return new TollFeeCalculator.Calculator(
      new TollFeeCalculator.FeeService(),
      new TollFeeCalculator.TollFreeDateService());
  }

}