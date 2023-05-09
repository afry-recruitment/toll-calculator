using System.Reflection;
using TollFeeCalculator;

namespace TollCalculatorTest
{
  [TestClass]
  public class FeeServiceTest
  {
    [TestMethod]
    public void FeeForNightTimeShouldBeZero()
    {
      TollFeeCalculator.FeeService service = new FeeService();
      var retval = service.FeeForTime(new TimeOnly(2, 2));

      Assert.AreEqual(0, retval);
    }
    [TestMethod]
    public void FeeForRushHourShouldBeHigh()
    {
      TollFeeCalculator.FeeService service = new FeeService();
      var retval = service.FeeForTime(new TimeOnly(7, 12));

      Assert.AreEqual(18, retval);
    }
    [TestMethod]
    public void MillisecondsMatter()
    {
      TollFeeCalculator.FeeService service = new FeeService();
      var retval = service.FeeForTime(new TimeOnly(6, 29, 59, 5));

      Assert.AreEqual(8, retval);
    }
  }
}