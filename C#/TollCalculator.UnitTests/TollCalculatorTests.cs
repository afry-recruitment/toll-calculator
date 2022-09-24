namespace TollCalculator.UnitTests;

using TollFeeCalculator;
[TestFixture]
public class TollCalculatorTests
{

    ITollCalculator tollCalculator = new TollCalculator();

    [Test]
    public void GetTollFee_Returns_0_for_sunday()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 25, 07, 00, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_Returns_0_for_cars_weekday_5_59()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 05, 59, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_6_00()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 06, 00, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(8));
    }


    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_6_29()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 06, 29, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(8));
    }
    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_6_30()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 06, 30, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(13));
    }
    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_6_59()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 06, 59, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_Returns_18_for_cars_weekday_7_00()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 07, 00, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(18));
    }
    [Test]
    public void GetTollFee_Returns_18_for_cars_weekday_7_59()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 07, 59, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(18));
    }

    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_8_00()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 08, 00, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_8_29()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 08, 29, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_8_30()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 08, 30, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(8));
    }

    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_9_00()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 09, 00, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(8));
    }

    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_14_59()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 14, 59, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(8));
    }

    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_15_00()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 15, 00, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_15_29()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 15, 29, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_Returns_18_for_cars_weekday_15_30()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 15, 30, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(18));
    }

    [Test]
    public void GetTollFee_Returns_18_for_cars_weekday_16_59()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 16, 59, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(18));
    }
    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_17_00()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 17, 00, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(13));
    }
    [Test]
    public void GetTollFee_Returns_13_for_cars_weekday_17_59()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 17, 59, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(13));
    }
    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_18_00()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 18, 00, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(8));
    }
    [Test]
    public void GetTollFee_Returns_8_for_cars_weekday_18_29()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 18, 29, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(8));
    }

    [Test]
    public void GetTollFee_Returns_0_for_cars_weekday_18_30()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 18, 30, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTotalTollFee_Returns_Max_60_for_one_day()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 08, 29, 29), new DateTime(2022, 09, 22, 09, 30, 29),
        new DateTime(2022, 09, 22, 10, 31, 29),
        new DateTime(2022, 09, 22, 11, 40, 29),
        new DateTime(2022, 09, 22, 15, 29, 29),
        new DateTime(2022, 09, 22, 16, 28, 29),
        new DateTime(2022, 09, 22, 17, 30, 29),
        new DateTime(2022, 09, 22, 07, 01, 29)};
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);

        Assert.That(actual, Is.EqualTo(60));
    }

    [Test]
    public void GetTotalTollFee_Returns_Max_60_for_one_day_and_out_of_order_times()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 08, 00, 29), new DateTime(2022, 09, 22, 09, 30, 29),
        new DateTime(2022, 09, 22, 10, 31, 29),
        new DateTime(2022, 09, 22, 11, 40, 29),
        new DateTime(2022, 09, 22, 15, 29, 29),
        new DateTime(2022, 09, 22, 16, 28, 29),
        new DateTime(2022, 09, 22, 17, 30, 29),
        new DateTime(2022, 09, 22, 07, 01, 29),
        new DateTime(2022, 09, 22, 20, 00, 00)};
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);

        Assert.That(actual, Is.EqualTo(60));
    }

    [Test]
    public void GetTotalTollFee_Returns_26_for_passage_6_00_and_07_01()
    {
        var dates = new DateTime[] {new DateTime(2022, 09, 22, 06, 00, 00),
        new DateTime(2022, 09, 22, 07, 01, 00)};
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);

        Assert.That(actual, Is.EqualTo(26));
    }

    [Test]
    public void GetTotalTollFee_Returns_8_for_passage_6_00()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 06, 00, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);

        Assert.That(actual, Is.EqualTo(8));
    }
    [Test]
    public void GetTotalTollFee_Returns_31_for_passage_6_00_and_6_45_and_7_01()
    {
        var date = DateOnly.Parse("2022-09-22");
        var times = new TimeOnly[]
        {
            TimeOnly.Parse("06:00"), // 8
            TimeOnly.Parse("06:45"), // 13
            TimeOnly.Parse("07:01"), // 18
        };
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 06, 00, 00),
        new DateTime(2022, 09, 22, 06, 45, 00),
        new DateTime(2022, 09, 22, 07, 01, 00)};
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);

        Assert.That(actual, Is.EqualTo(31));
    }

    [Test]
    public void GetTotalTollFee_Returns_13_for_passage_6_00_and_6_45()
    {
        var dates = new DateTime[] { new DateTime(2022, 09, 22, 06, 00, 00),
        new DateTime(2022, 09, 22, 06, 45, 00)};
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);

        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_Returns_0_for_provided_holiday()
    {
        ITollCalculator sut = new TollCalculator(new DateOnly[] { new DateOnly(2022, 1, 2) });
        var dates = new DateTime[] { new DateTime(2022, 01, 02, 06, 29, 00) };
        var vehicle = new Car();

        var actual = tollCalculator.GetTollFee(vehicle, dates);
        Assert.That(actual, Is.EqualTo(0));
    }

    [Test]
    public void GetTotalTollFee_Returns_0_for_provided_holiday()
    {
        ITollCalculator sut = new TollCalculator(new DateOnly[] { new DateOnly(2022, 1, 2) });

        var dates = new DateTime[] { new DateTime(2022, 01, 01, 06, 00, 00), new DateTime(2022, 01, 01, 06, 45, 00) };
        var vehicle = new Car();

        var actual = sut.GetTollFee(vehicle, dates);

        Assert.That(actual, Is.EqualTo(0));
    }
}