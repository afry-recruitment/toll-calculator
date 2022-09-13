namespace TollCalculator.Tests;

using TollFeeCalculator;
[TestFixture]
public class TollFreeVehicleTests
{

    ITollCalculator _sut = new TollCalculator();
    [Test]
    public void Toll_weekday_6_30_is_0_for_motorbikes()
    {
        var date = DateTime.Parse("2022-09-8 6:30:00");
        var vehicle = new Motorbike();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void Toll_weekday_6_30_is_0_for_tractors()
    {
        var date = DateTime.Parse("2022-09-8 6:30:00");
        var vehicle = new Tractor();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void Toll_weekday_6_30_is_0_for_diplomats()
    {
        var date = DateTime.Parse("2022-09-8 6:30:00");
        var vehicle = new Diplomat();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void Toll_weekday_6_30_is_0_for_emergency()
    {
        var date = DateTime.Parse("2022-09-8 6:30:00");
        var vehicle = new Emergency();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void Toll_weekday_6_30_is_0_for_foreign()
    {
        var date = DateTime.Parse("2022-09-8 6:30:00");
        var vehicle = new Foreign();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void Toll_weekday_6_30_is_0_for_military()
    {
        var date = DateTime.Parse("2022-09-8 6:30:00");
        var vehicle = new Military();
        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(0));
    }
}