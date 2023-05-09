using System.Reflection;
using TollFeeCalculator;

namespace TollCalculatorTest;

[TestClass]
public class EasterServiceTest
{
  [TestMethod]
  public void TrySeveralEasterDates()
  {

    var res2011 = TollFeeCalculator.EasterService.EasterDateForYear(2011);
    var res2013 = TollFeeCalculator.EasterService.EasterDateForYear(2013);
    var res2012 = TollFeeCalculator.EasterService.EasterDateForYear(2012);
    var res2019 = TollFeeCalculator.EasterService.EasterDateForYear(2019);
    var res2020 = TollFeeCalculator.EasterService.EasterDateForYear(2020);
    var res2025 = TollFeeCalculator.EasterService.EasterDateForYear(2025);

    Assert.AreEqual(4, res2011.Month);
    Assert.AreEqual(24, res2011.Day);

    Assert.AreEqual(4, res2012.Month);
    Assert.AreEqual(8, res2012.Day);

    Assert.AreEqual(3, res2013.Month);
    Assert.AreEqual(31, res2013.Day);

    Assert.AreEqual(4, res2019.Month);
    Assert.AreEqual(21, res2019.Day);

    Assert.AreEqual(4, res2020.Month);
    Assert.AreEqual(12, res2020.Day);

    Assert.AreEqual(4, res2025.Month);
    Assert.AreEqual(20, res2025.Day);


  }
}