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

    [Test]
    public void GetTotalTollFee_Returns_Max_60_for_one_day()
    {
        var date = DateOnly.Parse("2022-09-08");
        var times = new TimeOnly[]
        {
            TimeOnly.Parse("06:00"), // 8
            TimeOnly.Parse("07:01"), // + 18 = 26
            TimeOnly.Parse("08:02"), // + 13 = 39
            TimeOnly.Parse("09:33"), // + 8 = 47
            TimeOnly.Parse("10:34"), // + 8 = 56
            TimeOnly.Parse("11:35"), // + 8 = 64
        };
        var vehicle = new Car();

        var actual = _sut.GetTotalTollFee(vehicle, date, times);

        Assert.That(actual, Is.EqualTo(60));
    }

    [Test]
    public void GetTotalTollFee_Returns_Max_60_for_one_day_and_out_of_order_times()
    {
        var date = DateOnly.Parse("2022-09-08");
        var times = new TimeOnly[]
        {
            TimeOnly.Parse("06:00"), // 8
            TimeOnly.Parse("11:35"), // + 8 = 64
            TimeOnly.Parse("07:01"), // + 18 = 26
            TimeOnly.Parse("10:34"), // + 8 = 56
            TimeOnly.Parse("08:02"), // + 13 = 39
            TimeOnly.Parse("09:33"), // + 8 = 47
        };
        var vehicle = new Car();

        var actual = _sut.GetTotalTollFee(vehicle, date, times);

        Assert.That(actual, Is.EqualTo(60));
    }

    [Test]
    public void GetTotalTollFee_Returns_26_for_passage_6_00_and_07_01()
    {
        var date = DateOnly.Parse("2022-09-08");
        var times = new TimeOnly[]
        {
            TimeOnly.Parse("06:00"), // 8
            TimeOnly.Parse("07:01"), // + 18 = 26
        };
        var vehicle = new Car();

        var actual = _sut.GetTotalTollFee(vehicle, date, times);

        Assert.That(actual, Is.EqualTo(26));
    }

    [Test]
    public void GetTotalTollFee_Returns_8_for_passage_6_00()
    {
        var date = DateOnly.Parse("2022-09-08");
        var times = new TimeOnly[]
        {
            TimeOnly.Parse("06:00"), // 8
        };
        var vehicle = new Car();

        var actual = _sut.GetTotalTollFee(vehicle, date, times);

        Assert.That(actual, Is.EqualTo(8));
    }
    [Test]
    public void GetTotalTollFee_Returns_31_for_passage_6_00_and_6_45_and_7_01()
    {
        var date = DateOnly.Parse("2022-09-08");
        var times = new TimeOnly[]
        {
            TimeOnly.Parse("06:00"), // 8
            TimeOnly.Parse("06:45"), // 13
            TimeOnly.Parse("07:01"), // 18
        };
        var vehicle = new Car();

        var actual = _sut.GetTotalTollFee(vehicle, date, times);

        Assert.That(actual, Is.EqualTo(31));
    }

    [Test]
    public void GetTotalTollFee_Returns_13_for_passage_6_00_and_6_45()
    {
        var date = DateOnly.Parse("2022-09-08");
        var times = new TimeOnly[]
        {
            TimeOnly.Parse("06:00"), // 8
            TimeOnly.Parse("06:45"), // 13
        };
        var vehicle = new Car();

        var actual = _sut.GetTotalTollFee(vehicle, date, times);

        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTotalTollFee_NullValue_for_times_throws_ArgumentNullException()
    {
        var date = DateOnly.Parse("2022-09-08");
        TimeOnly[] times = null!;
        var vehicle = new Car();

        Assert.Throws<ArgumentNullException>(() => _sut.GetTotalTollFee(vehicle, date, times));
    }

    [Test]
    public void GetTollFee_Returns_0_for_provided_holiday()
    {
        ITollCalculator sut = new TollCalculator(new DateOnly[] { new DateOnly(2022, 1, 2) });
        var date = DateTime.Parse("2022-01-2T06:29:00+02:00");
        var vehicle = new Car();

        var actual = _sut.GetTollFee(date, vehicle);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTotalTollFee_Returns_0_for_provided_holiday()
    {
        ITollCalculator sut = new TollCalculator(new DateOnly[] { new DateOnly(2022, 1, 2) });
        var date = DateOnly.Parse("2022-01-01");
        var times = new TimeOnly[]
        {
            TimeOnly.Parse("06:00"), // 8
            TimeOnly.Parse("06:45"), // 13
        };
        var vehicle = new Car();

        var actual = sut.GetTotalTollFee(vehicle, date, times);

        Assert.That(actual, Is.EqualTo(0));
    }
}