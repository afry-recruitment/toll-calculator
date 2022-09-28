namespace TollCalculator.UnitTests;

using TollFeeCalculator;
[TestFixture]
public class TollFeeCalculatorHelperTest
{
    [Test]
    public void IsRushHour_Return_true_for_7_00()
    {
        var date = new DateTime(2022, 09, 25, 07, 00, 00);

        var actual = TollFeeCalculatorHelper.IsRushHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsRushHour_Return_true_for_15_31()
    {
        var date = new DateTime(2022, 09, 25, 15, 31, 00);

        var actual = TollFeeCalculatorHelper.IsRushHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsRushHour_Return_true_for_16_00()
    {
        var date = new DateTime(2022, 09, 25, 16, 00, 00);

        var actual = TollFeeCalculatorHelper.IsRushHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsRushHour_Return_False_for_17_10()
    {
        var date = new DateTime(2022, 09, 25, 17, 10, 00);

        var actual = TollFeeCalculatorHelper.IsRushHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(false));
    }

    [Test]
    public void IsMinimumFeeHours_Return_true_for_06_10()
    {
        var date = new DateTime(2022, 09, 25, 06, 10, 00);

        var actual = TollFeeCalculatorHelper.IsMinimumFeeHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsMinimumFeeHours_Return_true_for_08_10()
    {
        var date = new DateTime(2022, 09, 25, 08, 10, 00);

        var actual = TollFeeCalculatorHelper.IsMinimumFeeHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsMinimumFeeHours_Return_true_for_09_14()
    {
        var date = new DateTime(2022, 09, 25, 09, 14, 00);

        var actual = TollFeeCalculatorHelper.IsMinimumFeeHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsMinimumFeeHours_Return_true_for_18_29()
    {
        var date = new DateTime(2022, 09, 25, 18, 29, 00);

        var actual = TollFeeCalculatorHelper.IsMinimumFeeHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsMinimumFeeHours_Return_False_For_18_30()
    {
        var date = new DateTime(2022, 09, 25, 18, 30, 00);

        var actual = TollFeeCalculatorHelper.IsMinimumFeeHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(false));
    }

    [Test]
    public void IsAverageFeeHours_Return_true_for_06_30()
    {
        var date = new DateTime(2022, 09, 25, 06, 30, 00);

        var actual = TollFeeCalculatorHelper.IsAverageFeeHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsAverageFeeHours_Return_true_for_06_59()
    {
        var date = new DateTime(2022, 09, 25, 06, 59, 00);

        var actual = TollFeeCalculatorHelper.IsAverageFeeHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsAverageFeeHours_Return_true_for_08_20()
    {
        var date = new DateTime(2022, 09, 25, 08, 20, 00);

        var actual = TollFeeCalculatorHelper.IsAverageFeeHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsAverageFeeHours_Return_true_for_15_20()
    {
        var date = new DateTime(2022, 09, 25, 15, 20, 00);

        var actual = TollFeeCalculatorHelper.IsAverageFeeHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsAverageFeeHours_Return_true_for_17_00()
    {
        var date = new DateTime(2022, 09, 25, 17, 00, 00);

        var actual = TollFeeCalculatorHelper.IsAverageFeeHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsAverageFeeHours_Return_False_for_18_01()
    {
        var date = new DateTime(2022, 09, 25, 18, 01, 00);

        var actual = TollFeeCalculatorHelper.IsAverageFeeHours(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(false));
    }

    [Test]
    public void TollChargesPerPass_Return_8_for_18_29()
    {
        var date = new DateTime(2022, 09, 25, 18, 29, 00);

        var actual = TollFeeCalculatorHelper.TollChargesPerPass(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(8));
    }

    [Test]
    public void TollChargesPerPass_Return_13_for_17_00()
    {
        var date = new DateTime(2022, 09, 25, 17, 00, 00);

        var actual = TollFeeCalculatorHelper.TollChargesPerPass(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(13));
    }

    [Test]
    public void TollChargesPerPass_Return_18_for_07_00()
    {
        var date = new DateTime(2022, 09, 25, 07, 00, 00);

        var actual = TollFeeCalculatorHelper.TollChargesPerPass(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(18));
    }

    [Test]
    public void TollChargesPerPass_Return_0_for_05_00()
    {
        var date = new DateTime(2022, 09, 25, 05, 00, 00);

        var actual = TollFeeCalculatorHelper.TollChargesPerPass(date.Hour, date.Minute);
        Assert.That(actual, Is.EqualTo(0));
    }
}