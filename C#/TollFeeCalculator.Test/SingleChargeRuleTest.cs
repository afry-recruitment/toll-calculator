namespace TollFeeCalculator.Test;

public class SingleChargeRuleTest
{
	readonly IVehicle vehicle;
	readonly TollCalculator calc;

	public SingleChargeRuleTest()
	{
		calc = new TollCalculator();
		vehicle = new Car();
	}

	[Fact]
	public void ShouldOnlyBeChargedOnceAnHour()
	{
		//Setup
		DateTime[] passingDates = [
			new DateTime(2025, 6, 12, 6, 0, 0),
			new DateTime(2025, 6, 12, 6, 29, 0),
			new DateTime(2025, 6, 12, 6, 59, 59),
		];

		//Act
		int tollFees = calc.GetTollFee(passingDates, vehicle);

		//Assert
		Assert.Equal(13, tollFees);
	}

	[Fact]
	public void ShouldOnlyBeChargedOnceAnHourOverThreePeriods()
	{
		//Setup
		DateTime[] passingDates = [
			new DateTime(2025, 6, 12, 14, 50, 0),
			new DateTime(2025, 6, 12, 15, 10, 0),
			new DateTime(2025, 6, 12, 15, 49, 59),
		];

		//Act
		int tollFees = calc.GetTollFee(passingDates, vehicle);

		//Assert
		Assert.Equal(18, tollFees);
	}


	[Fact]
	public void ShouldBeChargedEveryHour()
	{
		//Setup
		DateTime[] passingDates = [
			new DateTime(2025, 6, 12, 5, 0, 0),
			new DateTime(2025, 6, 12, 6, 30, 0),
			new DateTime(2025, 6, 12, 7, 31, 0),
			new DateTime(2025, 6, 12, 8, 32, 0),
		];

		//Act
		int tollFees = calc.GetTollFee(passingDates, vehicle);

		//Assert
		Assert.Equal(39, tollFees);
	}


	[Fact]
	public void ShouldBeChargedEveryHourForUnsortedDates()
	{
		//Setup
		DateTime[] passingDates = [
			new DateTime(2025, 6, 12, 8, 32, 0),
			new DateTime(2025, 6, 12, 6, 30, 0),
			new DateTime(2025, 6, 12, 5, 0, 0),
			new DateTime(2025, 6, 12, 7, 31, 0),
		];

		//Act
		int tollFees = calc.GetTollFee(passingDates, vehicle);

		//Assert
		Assert.Equal(39, tollFees);
	}
}