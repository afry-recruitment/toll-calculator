namespace TollCalculator.Tests;

using TollFeeCalculator;
[TestFixture]
public class TollCalculatorTests
{

    ITollCalculator _sut = new TollCalculator();

    [Test]
    public void Toll_for_saturday_is_zero()
    {
        var date = DateTime.Parse("2022-09-10");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void Toll_weekday_6_30_is_13_for_cars()
    {
        var date = DateTime.Parse("2022-09-8 6:30:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(13));
    }
}