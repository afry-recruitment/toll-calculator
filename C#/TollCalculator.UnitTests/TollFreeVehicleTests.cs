namespace TollCalculator.UnitTests;

using TollFeeCalculator;
[TestFixture]
public class TollFreeVehicleTests
{

    ITollCalculator tollCalculator = new TollCalculator();
    [Test]
    public void GetTollFee_Returns_0_for_motorbikes_weekday_6_30()
    {
        var date = DateTime.Parse("2022-09-08T06:29:00+02:00");
        var vehicle = new Motorbike();

        var actual = tollCalculator.GetTollFee(vehicle, new DateTime[] { date });
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_Returns_0_for_tractors_weekday_6_30()
    {
        var date = DateTime.Parse("2022-09-08T06:29:00+02:00");
        var vehicle = new Tractor();

        var actual = tollCalculator.GetTollFee(vehicle, new DateTime[] { date });
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_Returns_0_for_diplomats_weekday_6_30()
    {
        var date = DateTime.Parse("2022-09-08T06:29:00+02:00");
        var vehicle = new Diplomat();

        var actual = tollCalculator.GetTollFee(vehicle, new DateTime[] { date });
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_Returns_0_for_emergency_weekday_6_30()
    {
        var date = DateTime.Parse("2022-09-08T06:29:00+02:00");
        var vehicle = new Emergency();

        var actual = tollCalculator.GetTollFee(vehicle, new DateTime[] { date });
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_Returns_0_for_foreign_weekday_6_30()
    {
        var date = DateTime.Parse("2022-09-08T06:29:00+02:00");
        var vehicle = new Foreign();

        var actual = tollCalculator.GetTollFee(vehicle, new DateTime[] { date });
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_Returns_0_for_military_weekday_6_30()
    {
        var date = DateTime.Parse("2022-09-08T06:29:00+02:00");
        var vehicle = new Military();
        var actual = tollCalculator.GetTollFee(vehicle, new DateTime[] { date });
        Assert.That(actual, Is.EqualTo(0));
    }
}