namespace TollFeeCalculator.Test;

public class FeeFreeDaysTest
{
	readonly IVehicle car;
	readonly TollCalculator calc;

	public FeeFreeDaysTest()
	{
		calc = new TollCalculator();
		car = new Car();
	}

	[Fact]
	public void ShouldNotBeChargedOnSaturdays()
	{
		//Setup
		var passingDate = new DateTime(2025, 6, 14, 6, 10, 0);

		//Act
		int calculateFee = calc.GetTollFee(passingDate, car);

		//Assert
		Assert.Equal(0, calculateFee);
	}

	[Fact]
	public void ShouldNotBeChargedOnSundays()
	{
		//Setup
		var passingDate = new DateTime(2025, 6, 15, 6, 10, 0);

		//Act
		int calculateFee = calc.GetTollFee(passingDate, car);

		//Assert
		Assert.Equal(0, calculateFee);
	}

	[Fact]
	public void ShouldNotBeChargedOnAHoliday()
	{
		//Setup
		var passingDate = new DateTime(2025, 6, 6, 6, 10, 0);

		//Act
		int calculateFee = calc.GetTollFee(passingDate, car);

		//Assert
		Assert.Equal(0, calculateFee);
	}
}