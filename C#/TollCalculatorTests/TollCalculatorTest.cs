namespace TollCalculatorTests;
using TollFeeCalculator;
public class TollCalculatorTest
{
    [Test]
    public void TollFee_ShouldBe_8()
    {
        //arrange
        var lowTrafficTime=new DateTime(2022, 10, 3, 6, 15, 0);
        var car= new Car();
        //act
        var result = new TollCalculator().GetTollFee(lowTrafficTime,car);

        //assert
        Assert.AreEqual(8,result);
        
    }

    [Test]
    public void TollFee_ShouldBe_13()
    {
        //arrange
        var mediumTrafficTime=new DateTime(2022, 10, 3, 6, 45, 0);
        var car= new Car();
        //act
        var result = new TollCalculator().GetTollFee(mediumTrafficTime,car);

        //assert
        Assert.AreEqual(13,result);
        
    }

    [Test]
    public void TollFee_ShouldBe_18()
    {
        //arrange
        var highTrafficTime=new DateTime(2022, 10, 3, 15, 35, 0);
        var car= new Car();
        //act
        var result = new TollCalculator().GetTollFee(highTrafficTime,car);

        //assert
        Assert.AreEqual(18,result);
        
    }

    [Test]
    public void TollFee_ShouldBe_zero()
    {
        //arrange
        var nightTrafficTime=new DateTime(2022, 10, 3, 20, 35, 0);
        var car= new Car();
        //act
        var result = new TollCalculator().GetTollFee(nightTrafficTime,car);

        //assert
        Assert.AreEqual(0,result);
        
    }

    [Test]
    public void TollFee_ShouldBe_Max8()
    {
        //arrange
        var lowTrafficSameHour=new List<DateTime>(){
            new DateTime(2022, 10, 4, 6, 10, 0),
            new DateTime(2022, 10, 4, 6, 15, 0),
            new DateTime(2022, 10, 4, 6, 29, 0)};
        var car= new Car();
        //act
        var result = new TollCalculator().GetTollFee(car,lowTrafficSameHour);

        //assert
        Assert.AreEqual(8,result);
        
    }

     [Test]
    public void TollFee_ShouldBe_Max39()
    {
        //arrange
        var lowTrafficSameHour=new List<DateTime>(){
            new DateTime(2022, 10, 4, 6, 10, 0),
            new DateTime(2022, 10, 4, 8, 15, 0),
            new DateTime(2022, 10, 4, 15, 35, 0)};
        var car= new Car();
        //act
        var result = new TollCalculator().GetTollFee(car,lowTrafficSameHour);

        //assert
        Assert.AreEqual(39,result);
        
    }

    [Test]
    public void TollFee_ShouldBe_Max60()
    {
        //arrange
        var tollTimes=new List<DateTime>(){
            new DateTime(2022, 10, 3, 6, 15, 0),
            new DateTime(2022, 10, 3, 7, 45, 0),
            new DateTime(2022, 10, 3, 9, 20, 0),
            new DateTime(2022, 10, 3, 10, 15, 0),
            new DateTime(2022, 10, 3, 15,45, 0),
            new DateTime(2022, 10, 4, 6, 15, 0),
            new DateTime(2022, 10, 4, 6, 35, 0),
            new DateTime(2022, 10, 4, 10, 15, 0),
            new DateTime(2022, 10, 5, 12, 35, 0),
            new DateTime(2022, 10, 5, 18,15, 0)};
        var car= new Car();
        //act
        var result = new TollCalculator().GetTollFee(car,tollTimes);

        //assert
        Assert.AreEqual(60,result);
        
    }


    [Test]
    public void NonFreeVehicleShouldNotBeTollFree()
    {
        //arrange
        var vehicle = new Car();

        //act
        var result = new TollCalculator().IsTollFreeVehicle(vehicle);

        //assert
        Assert.IsFalse(result);
        
    }

    [Test]
    public void NonFreeVehicleShouldBeTollFree()
    {
        //arrange
        var vehicle = new Motorbike();

        //act
        var result = new TollCalculator().IsTollFreeVehicle(vehicle);

        //assert
        Assert.IsTrue(result);
        
    }

    [Test]
    public void SaturdayShouldBeTollFree()
    {
        //arrange
        var saturday = new DateTime(2022,10,1);

        //act
        var result = new TollCalculator().IsTollFreeDate(saturday);

        //assert
        Assert.IsTrue(result);
        
    }

     [Test]
    public void SundayShouldBeTollFree()
    {
        //arrange
        var sunday = new DateTime(2022,10,2);

        //act
        var result = new TollCalculator().IsTollFreeDate(sunday);

        //assert
        Assert.IsTrue(result);
        
    }
    [Test]
    public void FridayShouldNotBeTollFree()
    {
        //arrange
        var friday = new DateTime(2022,09,30);

        //act
        var result = new TollCalculator().IsTollFreeDate(friday);

        //assert
        Assert.IsFalse(result);
        
    }

    [Test]
    public void MondayShouldNotBeTollFree()
    {
        //arrange
        var monday = new DateTime(2022,10,3);

        //act
        var result = new TollCalculator().IsTollFreeDate(monday);

        //assert
        Assert.IsFalse(result);
        
    }

    [Test]
    public void HolidayShouldBeTollFree()
    {
        //arrange
        var holiday = new DateTime(2022,12,24);

        //act
        var result = new TollCalculator().IsTollFreeDate(holiday);

        //assert
        Assert.IsTrue(result);
        
    }


}