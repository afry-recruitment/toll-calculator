namespace TollCalculator.UnitTests;

using TollFeeCalculator;
[TestFixture]
public class TollFreeHelperTests
{

    [Test]
    public void IsTollFreeVehicle_Return_True_For_motorbikes()
    {
        var motorbike = new Motorbike();

        var actual = TollFreeHelper.IsTollFreeVehicle(motorbike);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsTollFreeVehicle_Return_True_For_Tractor()
    {
        var tractor = new Tractor();

        var actual = TollFreeHelper.IsTollFreeVehicle(tractor);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsTollFreeVehicle_Return_True_For_Diplomat()
    {
        var diplomat = new Diplomat();

        var actual = TollFreeHelper.IsTollFreeVehicle(diplomat);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsTollFreeVehicle_Return_True_For_Emergency()
    {
        var emergency = new Emergency();

        var actual = TollFreeHelper.IsTollFreeVehicle(emergency);
        Assert.That(actual, Is.EqualTo(true));
    }
    [Test]
    public void IsTollFreeVehicle_Return_True_For_Military()
    {
        var military = new Military();

        var actual = TollFreeHelper.IsTollFreeVehicle(military);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsTollFreeVehicle_Return_True_For_Foreign()
    {
        var foreign = new Foreign();

        var actual = TollFreeHelper.IsTollFreeVehicle(foreign);
        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void IsTollFreeVehicle_Return_True_For_Car()
    {
        var car = new Car();

        var actual = TollFreeHelper.IsTollFreeVehicle(car);
        Assert.That(actual, Is.EqualTo(false));
    }

    [Test]
    public void GetTotalTollFee_Returns_True_for_provided_holiday()
    {
        var holiday = new DateOnly[] { new DateOnly(2022, 1, 2) };

        var date = new DateTime(2022, 01, 01, 06, 00, 00);

        var actual = TollFreeHelper.IsTollFreeDate(date, holiday);

        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void GetTotalTollFee_Returns_True_for_Weekend()
    {
        var holiday = new DateOnly[0];

        var date = new DateTime(2022, 09, 24, 06, 00, 00);

        var actual = TollFreeHelper.IsTollFreeDate(date, holiday);

        Assert.That(actual, Is.EqualTo(true));
    }

    [Test]
    public void GetTotalTollFee_Returns_False_for_provided_holiday()
    {
        var holiday = new DateOnly[] { new DateOnly(2022, 1, 1) };

        var date = new DateTime(2022, 01, 03, 06, 00, 00);

        var actual = TollFreeHelper.IsTollFreeDate(date, holiday);

        Assert.That(actual, Is.EqualTo(false));
    }
}