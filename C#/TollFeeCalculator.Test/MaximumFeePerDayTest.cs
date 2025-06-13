namespace TollFeeCalculator.Test;

public class MaximumFeePerDayTest
{
	readonly IVehicle vehicle;
	readonly TollCalculator calc;

	public MaximumFeePerDayTest()
	{
		calc = new TollCalculator();
		vehicle = new Car();
	}

	[Fact]
	public void ShouldNotBeChargedMoreThanMaximumAmountPerDay()
	{
		//Setup
		DateTime[] passingDates = [
			new DateTime(2025, 6, 12, 5, 0, 0),
			new DateTime(2025, 6, 12, 6, 30, 0),
			new DateTime(2025, 6, 12, 7, 31, 0),
			new DateTime(2025, 6, 12, 8, 32, 0),
			new DateTime(2025, 6, 12, 9, 33, 0),
			new DateTime(2025, 6, 12, 10, 34, 0),
			new DateTime(2025, 6, 12, 11, 35, 0),
			new DateTime(2025, 6, 12, 15, 00, 0),
			new DateTime(2025, 6, 12, 16, 01, 0),
		];

		//Act
		int tollFees = calc.GetTollFee(passingDates, vehicle);

		//Assert
		Assert.Equal(60, tollFees);
	}

}