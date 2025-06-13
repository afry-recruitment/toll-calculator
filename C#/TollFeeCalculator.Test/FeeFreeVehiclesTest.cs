namespace TollFeeCalculator.Test;

public class FeeFreeVehiclesTest
{
	readonly TollCalculator calc;
	readonly DateTime passingDate;

	public FeeFreeVehiclesTest()
	{
		calc = new TollCalculator();
		passingDate = new DateTime(2025, 6, 12, 6, 10, 0);
	}

	[Fact]
	public void ShouldReturnZeroForMotorbike()
	{
		//Setup
		IVehicle vehicle = new Motorbike();

		//Act
		int calculateFee = calc.GetTollFee(passingDate, vehicle);

		//Assert
		Assert.Equal(0, calculateFee);
	}

	[Fact]
	public void ShouldReturnZeroForTractor()
	{
		//Setup
		IVehicle vehicle = new Tractor();

		//Act
		int calculateFee = calc.GetTollFee(passingDate, vehicle);

		//Assert
		Assert.Equal(0, calculateFee);
	}

	[Fact]
	public void ShouldReturnZeroForEmergency()
	{
		//Setup
		IVehicle vehicle = new Emergency();

		//Act
		int calculateFee = calc.GetTollFee(passingDate, vehicle);

		//Assert
		Assert.Equal(0, calculateFee);
	}

	[Fact]
	public void ShouldReturnZeroForDiplomat()
	{
		//Setup
		IVehicle vehicle = new Diplomat();

		//Act
		int calculateFee = calc.GetTollFee(passingDate, vehicle);

		//Assert
		Assert.Equal(0, calculateFee);
	}

	[Fact]
	public void ShouldReturnZeroForForeign()
	{
		//Setup
		IVehicle vehicle = new Foreign();

		//Act
		int calculateFee = calc.GetTollFee(passingDate, vehicle);

		//Assert
		Assert.Equal(0, calculateFee);
	}

	[Fact]
	public void ShouldReturnZeroForMilitary()
	{
		//Setup
		IVehicle vehicle = new Military();

		//Act
		int calculateFee = calc.GetTollFee(passingDate, vehicle);

		//Assert
		Assert.Equal(0, calculateFee);
	}

	[Fact]
	public void ShouldReturnNonZeroForCar()
	{
		//Setup
		IVehicle vehicle = new Car();

		//Act
		int calculateFee = calc.GetTollFee(passingDate, vehicle);

		//Assert
		Assert.Equal(8, calculateFee);
	}
}