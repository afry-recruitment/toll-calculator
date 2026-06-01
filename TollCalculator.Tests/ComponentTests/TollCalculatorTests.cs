using PublicHoliday;
using TollCalculator.Holidays;
using TollCalculator.Vehicles;

namespace TollCalculator.Tests.ComponentTests;

public class TollCalculatorTests
{
    private static TollCalculator CreateSut() => new(new TollFeeSchedule(new ExternalHolidayProvider(new SwedenPublicHoliday())));
    
    [Test]
    public void GetTollFee_Highest_toll_within_the_hour_is_applied()
    {
        var car = new Car();

        DateTime[] dates =
        [
            new(2026, 5, 29, 6, 0, 0),
            new(2026, 5, 29, 6, 30, 0),
            new(2026, 5, 29, 6, 59, 0),
        ];
        
        var sut = CreateSut();
        
        var tollFee = sut.GetTollFee(car, dates);
        
        Assert.That(tollFee, Is.EqualTo(13));
    }
    
    [TestCase("Car")]
    public void GetTollFee_Maximum_toll_fee_per_day_60(string vehicleType)
    {
        var vehicle = VehicleFactory.Create(vehicleType);

        DateTime[] dates =
        [
            new(2026, 5, 29, 6, 0, 0),
            new(2026, 5, 29, 6, 30, 0),
            new(2026, 5, 29, 7, 0, 0),
            new(2026, 5, 29, 7, 0, 0),
            new(2026, 5, 29, 7, 30, 0),
            new(2026, 5, 29, 8, 0, 0),
            new(2026, 5, 29, 8, 0, 0),
            new(2026, 5, 29, 8, 30, 0),
            new(2026, 5, 29, 9, 0, 0),
            new(2026, 5, 29, 9, 0, 0),
            new(2026, 5, 29, 9, 30, 0),
            new(2026, 5, 29, 10, 0, 0),
            new(2026, 5, 29, 10, 0, 0),
            new(2026, 5, 29, 10, 30, 0),
            new(2026, 5, 29, 11, 0, 0),
        ];

        var tollCalculator = CreateSut();
        
        var tollFee = tollCalculator.GetTollFee(vehicle, dates);
        
        Assert.That(tollFee, Is.EqualTo(60));
    }

    [TestCase("Motorbike")]
    [TestCase("Tractor")]
    [TestCase("EmergencyVehicle")]
    [TestCase("DiplomaticVehicle")]
    [TestCase("ForeignVehicle")]
    [TestCase("MilitaryVehicle")]
    public void GetTollFee_free_vehicles(string vehicleName)
    {
        var vehicle = VehicleFactory.Create(vehicleName);

        DateTime[] dates =
        [
            new(2026, 5, 29, 6, 0, 0),
            new(2026, 5, 29, 6, 30, 0),
            new(2026, 5, 29, 7, 0, 0),
            new(2026, 5, 29, 10, 0, 0),
            new(2026, 5, 29, 10, 0, 0),
            new(2026, 5, 29, 10, 30, 0),
            new(2026, 5, 29, 11, 0, 0),
        ];
        
        var sut = CreateSut();
        
        var tollFee = sut.GetTollFee(vehicle, dates);
        
        Assert.That(tollFee, Is.EqualTo(0));
    }
    
    [TestCase("Car")]
    [TestCase("Tractor")]
    public void GetTollFee_free_on_weekends(string vehicleName)
    {
        var vehicle = VehicleFactory.Create(vehicleName);

        DateTime[] dates =
        [
            new(2026, 5, 30, 6, 0, 0), //Saturday 
            new(2026, 5, 30, 6, 30, 0),
            new(2026, 5, 30, 7, 0, 0),
            new(2026, 5, 30, 10, 0, 0),
            new(2026, 5, 30, 10, 0, 0),
            new(2026, 5, 30, 10, 30, 0),
            new(2026, 5, 30, 11, 0, 0),
        ];
        
        var sut = CreateSut();
        
        var tollFee = sut.GetTollFee(vehicle, dates);
        
        Assert.That(tollFee, Is.EqualTo(0));
    }
    
    [Test]
    public void GetTollFee_outside_interval_hour()
    {
        var car = new Car();

        DateTime[] dates =
        [
            new(2026, 5, 29, 6, 0, 0),
            new(2026, 5, 29, 6, 30, 0),
            new(2026, 5, 29, 7, 30, 0),
        ];

        var sut = CreateSut();

        var tollFee = sut.GetTollFee(car, dates);

        Assert.That(tollFee, Is.EqualTo(31));
    }
    
    [Test]
    public void GetTollFee_within_hour_then_take_highest_fee()
    {
        var car = new Car();

        DateTime[] dates =
        [
            new(2026, 5, 29, 6, 30, 0),
            new(2026, 5, 29, 7, 29, 0),
        ];

        var sut = CreateSut();

        var tollFee = sut.GetTollFee(car, dates);

        Assert.That(tollFee, Is.EqualTo(18));
    }

    [Test]
    public void GetTollFee_Multiple_interval_groups_should_be_within_the_same_hour()
    {
        var car = new Car();

        DateTime[] dates =
        [
            new(2026, 5, 29, 6, 0, 0),
            new(2026, 5, 29, 6, 30, 0),
            new(2026, 5, 29, 7, 30, 0),
            new(2026, 5, 29, 7, 45, 0)
        ];

        var sut = CreateSut();

        var tollFee = sut.GetTollFee(car, dates);

        Assert.That(tollFee, Is.EqualTo(31));
    }

    [Test]
    public void GetTollFee_EmptyDates()
    {
        var sut = CreateSut();
        
        var tollFee = sut.GetTollFee(new Car(), []);
        
        Assert.That(tollFee, Is.EqualTo(0));
    }
    
    [Test]
    public void GetTollFee_Dates_in_weird_order_should_give_same_fee_as_sorted()
    {
        var car = new Car();

        DateTime[] sorted =
        [
            new(2026, 5, 29, 6, 0, 0),
            new(2026, 5, 29, 6, 30, 0),
            new(2026, 5, 29, 7, 30, 0),
            new(2026, 5, 29, 8, 0, 0),
        ];

        DateTime[] shuffled =
        [
            new(2026, 5, 29, 7, 30, 0),
            new(2026, 5, 29, 6, 0, 0),
            new(2026, 5, 29, 8, 0, 0),
            new(2026, 5, 29, 6, 30, 0),
        ];

        var sut = CreateSut();

        var sortedFee = sut.GetTollFee(car, sorted);
        var shuffledFee = sut.GetTollFee(car, shuffled);

        Assert.That(shuffledFee, Is.EqualTo(sortedFee));
    }
    
    [Test]
    public void GetTollFee_MultipleDays()
    {
        var car = new Car();

        DateTime[] dates =
        [
            new(2026, 5, 28, 6, 0, 0),
            new(2026, 5, 28, 6, 30, 0),
            new(2026, 5, 28, 7, 0, 0),
            new(2026, 5, 28, 7, 0, 0),
            new(2026, 5, 28, 7, 30, 0),
            new(2026, 5, 28, 8, 0, 0),
            new(2026, 5, 28, 8, 0, 0),
            new(2026, 5, 28, 8, 30, 0),
            new(2026, 5, 28, 9, 0, 0),
            new(2026, 5, 28, 9, 0, 0),
            new(2026, 5, 28, 9, 30, 0),
            new(2026, 5, 28, 10, 0, 0),
            new(2026, 5, 28, 10, 0, 0),
            new(2026, 5, 28, 10, 30, 0),
            new(2026, 5, 28, 11, 0, 0),
            
            new(2026, 5, 29, 6, 0, 0),
            new(2026, 5, 29, 6, 30, 0),
            new(2026, 5, 29, 6, 59, 59),
        ];
        var sut = CreateSut();
        
        var tollFee = sut.GetTollFee(car, dates);
        
        Assert.That(tollFee, Is.EqualTo(73));
    }

    [Test]
    public void GetTollFee_single_date_should_be_toll()
    {
        var car = new Car();
        var date = new DateTime(2026, 5, 27, 8, 0, 0);
        var sut = CreateSut();

        var tollFee = sut.GetTollFee(car, date);

        Assert.That(tollFee, Is.EqualTo(13));
    }

    [Test]
    public void GetTollFee_single_date_on_weekend()
    {
        var car = new Car();
        var date = new DateTime(2026, 5, 31, 6, 0, 0);
        var sut = CreateSut();
        
        var tollFee = sut.GetTollFee(car, date);
        
        Assert.That(tollFee, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_interval_starts_on_toll_free_time_still_charges_highest_in_window()
    {
        var car = new Car();

        DateTime[] dates =
        [
            new(2026, 5, 29, 5, 30, 0),
            new(2026, 5, 29, 6, 0, 0),
            new(2026, 5, 29, 6, 15, 0),
        ];

        var sut = CreateSut();

        var tollFee = sut.GetTollFee(car, dates);

        Assert.That(tollFee, Is.EqualTo(8));
    }

    [TestCase("Motorbike")]
    [TestCase("Tractor")]
    [TestCase("EmergencyVehicle")]
    [TestCase("DiplomaticVehicle")]
    [TestCase("ForeignVehicle")]
    [TestCase("MilitaryVehicle")]
    public void GetTollFee_single_date_toll_free_vehicles(string vehicleName)
    {
        var vehicle = VehicleFactory.Create(vehicleName);
        var date = new DateTime(2026, 5, 29, 6, 0, 0);
        var sut = CreateSut();

        var tollFee = sut.GetTollFee(vehicle, date);

        Assert.That(tollFee, Is.EqualTo(0));
    }
}