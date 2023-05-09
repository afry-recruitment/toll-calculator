namespace TollCalculatorTest
{
  [TestClass]
  public class TollFeeDateServiceTest
  {
    [TestMethod]
    public void NoFeeOnKristiFlygare()
    {
      TollFeeCalculator.TollFreeDateService service = new TollFeeCalculator.TollFreeDateService();
      Assert.AreEqual(true, service.IsTollFree(new DateTime(2023, 5, 18)));
    }

    [TestMethod]
    public void NoFeeInJuly()
    {
      TollFeeCalculator.TollFreeDateService service = new TollFeeCalculator.TollFreeDateService();
      Assert.AreEqual(true, service.IsTollFree(new DateTime(2023, 7, 12)));
    }

    [TestMethod]
    public void NoFeeOnSaturdays()
    {
      TollFeeCalculator.TollFreeDateService service = new TollFeeCalculator.TollFreeDateService();
      Assert.AreEqual(true, service.IsTollFree(new DateTime(2023, 9, 9)));
    }
    [TestMethod]
    public void NoFeeOnMaundyThursday()
    {
      TollFeeCalculator.TollFreeDateService service = new TollFeeCalculator.TollFreeDateService();
      Assert.AreEqual(true, service.IsTollFree(new DateTime(2023, 4, 6)));
    }

    [TestMethod]
    public void FeeOnOrdinaryDay()
    {
      TollFeeCalculator.TollFreeDateService service = new TollFeeCalculator.TollFreeDateService();
      Assert.AreEqual(false, service.IsTollFree(new DateTime(2023, 5, 9)));
    }

  }
}