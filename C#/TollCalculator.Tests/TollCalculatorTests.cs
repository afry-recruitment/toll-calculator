namespace TollCalculator.Tests;

using TollFeeCalculator;
[TestFixture]
public class TollCalculatorTests
{

    ITollCalculator _sut = new TollCalculator();

    [Test]
    public void GetTollFee_Returns_0_for_saturday()
    {
        var date = DateTime.Parse("2022-09-10T06:29:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_Returns_0_for_cars_weekday_5_59()
    {
        var date = DateTime.Parse("2022-09-08T05:59:59+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_6_00()
    {
        var date = DateTime.Parse("2022-09-08T06:00:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(8));
    }


    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_6_29()
    {
        var date = DateTime.Parse("2022-09-08T06:29:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(8));
    }
    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_6_30()
    {
        var date = DateTime.Parse("2022-09-08T06:30:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(13));
    }
    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_6_59()
    {
        var date = DateTime.Parse("2022-09-08T06:59:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_Returns_18_for_cars_weekday_7_00()
    {
        var date = DateTime.Parse("2022-09-08T07:00:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(18));
    }
    [Test]
    public void GetTollFee_Returns_18_for_cars_weekday_7_59()
    {
        var date = DateTime.Parse("2022-09-08T07:59:59+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(18));
    }

    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_8_00()
    {
        var date = DateTime.Parse("2022-09-08T08:00:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_8_29()
    {
        var date = DateTime.Parse("2022-09-08T08:29:29+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_8_30()
    {
        var date = DateTime.Parse("2022-09-08T08:30:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(8));
    }

    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_9_00()
    {
        var date = DateTime.Parse("2022-09-08T09:00:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(8));
    }

    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_14_59()
    {
        var date = DateTime.Parse("2022-09-08T14:59:59+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(8));
    }

    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_15_00()
    {
        var date = DateTime.Parse("2022-09-08T15:00:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_15_29()
    {
        var date = DateTime.Parse("2022-09-08T15:29:59+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_Returns_18_for_cars_weekday_15_30()
    {
        var date = DateTime.Parse("2022-09-08T15:30:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(18));
    }

    [Test]
    public void GetTollFee_Returns_18_for_cars_weekday_16_59()
    {
        var date = DateTime.Parse("2022-09-08T16:59:59+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(18));
    }
    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_17_00()
    {
        var date = DateTime.Parse("2022-09-08T17:00:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(13));
    }
    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_17_59()
    {
        var date = DateTime.Parse("2022-09-08T17:59:59+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(13));
    }
    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_18_00()
    {
        var date = DateTime.Parse("2022-09-08T18:00:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(8));
    }
    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_18_29()
    {
        var date = DateTime.Parse("2022-09-08T18:29:29+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(8));
    }
    [Test]
    public void GetTollFee_Returns_0_for_cars_weekday_18_30()
    {
        var date = DateTime.Parse("2022-09-08T18:30:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_Throws_for_Unspecified_Date()
    {
        var date = DateTime.Parse("2022-09-08T18:30:00");
        var vehicle = new Car();

        Assert.Throws<InvalidOperationException>(() => _sut.GetTollFee(date, vehicle));
    }

    [Test]
    public void GetTollFee_Throws_for_UTC_Date()
    {
        var date = DateTime.SpecifyKind(DateTime.Parse("2022-09-08T18:30:00"), DateTimeKind.Utc);
        var vehicle = new Car();

        Assert.Throws<InvalidOperationException>(() => _sut.GetTollFee(date, vehicle));
    }

}