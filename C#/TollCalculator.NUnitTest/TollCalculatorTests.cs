using NUnit.Framework;
using TollCalculator.DataAccess;
using TollCalculator.Models;

namespace TollCalculator;

[TestFixture]
public class TollCalculatorTests
{

    [Test]
    [TestCase("2023-03-02 08:15:00", "2023-03-02 12:10:00")]
    public void GetTollFee_EmptyVehicle_ThrowException(DateTime first, DateTime last)
    {
        Repo repo = new();
        Car car = new();
        TollCalculator tollCalc = new(repo);
        DateTime[] dates = new DateTime[] { first, last };

        var exceptionDetails = Assert.Throws<ArgumentNullException>(() => tollCalc.GetTollFee(null, dates));
        Assert.AreEqual("Value cannot be null. (Parameter 'vehicle')", exceptionDetails.Message);
    }

    [Test]
    public void GetTollFee_EmptyDateArray_ThrowException()
    {
        Repo repo = new();
        Car car = new();
        TollCalculator tollCalc = new(repo);
        DateTime[] dates = new DateTime[0];

        var exceptionDetails = Assert.Throws<ArgumentException>(() => tollCalc.GetTollFee(car, dates));
        Assert.AreEqual("Unvalid dates", exceptionDetails.Message);
    }

    [Test]
    [TestCase("2023-03-02 08:15:00", "2023-03-02 12:10:00", ExpectedResult = 21)]
    public int GetTollFee_Return21(DateTime first, DateTime last)
    {
        Repo repo = new();
        Car car = new();
        TollCalculator tollCalc = new(repo);
        DateTime[] dates = new DateTime[] { first, last };

        return tollCalc.GetTollFee(car, dates);
    }

    [Test]
    [TestCase("2023-12-31")]
    public void IsTollFreeDate_ReturnTrue(DateTime date)
    {
        Repo repo = new();
        TollCalculator tollCalc = new(repo);
        var res = tollCalc.IsTollFreeDate(date);

        Assert.IsTrue(res);
    }

    [Test]
    [TestCase("2023-12-27")]
    public void IsTollFreeDate_ReturnFalse(DateTime date)
    {
        Repo repo = new();
        TollCalculator tollCalc = new(repo);
        var res = tollCalc.IsTollFreeDate(date);

        Assert.IsFalse(res);
    }

    [Test]
    public void IsTollFreeVehicle_ReturnFalse()
    {
        Repo repo = new();
        Car car = new();
        TollCalculator tollCalc = new(repo);
        var res = tollCalc.IsTollFreeVehicle(car);
        Assert.IsFalse(res);
    }

    [Test]
    public void IsTollFreeVehicle_ReturnTrue()
    {
        Repo repo = new();
        Motorbike motorbike = new();
        TollCalculator tollCalc = new(repo);
        var res = tollCalc.IsTollFreeVehicle(motorbike);
        Assert.IsTrue(res);
    }
}