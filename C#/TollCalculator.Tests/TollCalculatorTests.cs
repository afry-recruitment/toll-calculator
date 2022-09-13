namespace TollCalculator.Tests;

using TollFeeCalculator;
[TestFixture]
public class TollCalculatorTests
{

    ITollCalculator _sut = new TollCalculator();

    // [SetUp]
    // public void Setup()
    // {
    // }

    [Test]
    public void Toll_for_saturday_is_zero()
    {
        var date = DateTime.Parse("2022-09-10");
        var vehicle = new Car();

        Assert.That(_sut.GetTollFee(date, vehicle), Is.EqualTo(0));
    }

    [Test]
    public void Toll_weekday_6_30_is_13()
    {
        var date = DateTime.Parse("2022-09-8 6:30:00");
        var vehicle = new Car();

        Assert.That(_sut.GetTollFee(date, vehicle), Is.EqualTo(13));
    }
}