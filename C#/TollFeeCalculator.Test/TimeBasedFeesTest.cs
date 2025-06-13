namespace TollFeeCalculator.Test;

public class TimeBasedFeesTest
{
	readonly IVehicle vehicle;
	readonly TollCalculator calc;

	public TimeBasedFeesTest()
	{
		calc = new TollCalculator();
		vehicle = new Car();
	}

	[Fact]
	public void ShouldReturnEightDuringFirstPeriod()
	{
		/* First period is 6:00 - 6:29 */

		//Setup
		var passingDateUpperEdge = new DateTime(2025, 6, 12, 6, 0, 0);
		var passingDateMiddle = new DateTime(2025, 6, 12, 6, 15, 0);
		var passingDateLowerEdge = new DateTime(2025, 6, 12, 6, 29, 59);

		//Act
		int highEdgeFee = calc.GetTollFee(passingDateUpperEdge, vehicle);
		int midTimeFee = calc.GetTollFee(passingDateMiddle, vehicle);
		int lowEdgeFee = calc.GetTollFee(passingDateLowerEdge, vehicle);

		//Assert
		Assert.Equal(8, highEdgeFee);
		Assert.Equal(8, midTimeFee);
		Assert.Equal(8, lowEdgeFee);
	}

	[Fact]
	public void ShouldReturnThirteenDuringSecondPeriod()
	{
		/* Second period is 6:30 - 6:59 */

		//Setup
		var passingDateUpperEdge = new DateTime(2025, 6, 12, 6, 30, 0);
		var passingDateMiddle = new DateTime(2025, 6, 12, 6, 45, 0);
		var passingDateLowerEdge = new DateTime(2025, 6, 12, 6, 59, 0);

		//Act
		int highEdgeFee = calc.GetTollFee(passingDateUpperEdge, vehicle);
		int midTimeFee = calc.GetTollFee(passingDateMiddle, vehicle);
		int lowEdgeFee = calc.GetTollFee(passingDateLowerEdge, vehicle);

		//Assert
		Assert.Equal(13, highEdgeFee);
		Assert.Equal(13, midTimeFee);
		Assert.Equal(13, lowEdgeFee);
	}

	[Fact]
	public void ShouldReturnEighteenDuringThirdPeriod()
	{
		/* Third period is 7:00 - 7:59 */

		//Setup
		var passingDateUpperEdge = new DateTime(2025, 6, 12, 7, 00, 0);
		var passingDateMiddle = new DateTime(2025, 6, 12, 7, 30, 0);
		var passingDateLowerEdge = new DateTime(2025, 6, 12, 7, 59, 59);

		//Act
		int highEdgeFee = calc.GetTollFee(passingDateUpperEdge, vehicle);
		int midTimeFee = calc.GetTollFee(passingDateMiddle, vehicle);
		int lowEdgeFee = calc.GetTollFee(passingDateLowerEdge, vehicle);

		//Assert
		Assert.Equal(18, highEdgeFee);
		Assert.Equal(18, midTimeFee);
		Assert.Equal(18, lowEdgeFee);
	}

	[Fact]
	public void ShouldReturnThirteenDuringFourthPeriod()
	{
		/* Fourth period is 8:00 - 8:29 */

		//Setup
		var passingDateUpperEdge = new DateTime(2025, 6, 12, 8, 0, 0);
		var passingDateMiddle = new DateTime(2025, 6, 12, 8, 15, 0);
		var passingDateLowerEdge = new DateTime(2025, 6, 12, 8, 29, 59);

		//Act
		int highEdgeFee = calc.GetTollFee(passingDateUpperEdge, vehicle);
		int midTimeFee = calc.GetTollFee(passingDateMiddle, vehicle);
		int lowEdgeFee = calc.GetTollFee(passingDateLowerEdge, vehicle);

		//Assert
		Assert.Equal(13, highEdgeFee);
		Assert.Equal(13, midTimeFee);
		Assert.Equal(13, lowEdgeFee);
	}

	[Fact]
	public void ShouldReturnEightDuringFifthPeriod()
	{
		/* Fifth period is 8:30 - 14:59 */

		//Setup
		var passingDateUpperEdge = new DateTime(2025, 6, 12, 8, 30, 0);
		var passingDateMiddle = new DateTime(2025, 6, 12, 9, 15, 0);
		var passingDateMiddleTwo = new DateTime(2025, 6, 12, 11, 45, 0);
		var passingDateLowerEdge = new DateTime(2025, 6, 12, 14, 59, 59);

		//Act
		int highEdgeFee = calc.GetTollFee(passingDateUpperEdge, vehicle);
		int midTimeFee = calc.GetTollFee(passingDateMiddle, vehicle);
		var midTimeTwoFee = calc.GetTollFee(passingDateMiddleTwo, vehicle);
		int lowEdgeFee = calc.GetTollFee(passingDateLowerEdge, vehicle);

		//Assert
		Assert.Equal(8, highEdgeFee);
		Assert.Equal(8, midTimeFee);
		Assert.Equal(8, midTimeTwoFee);
		Assert.Equal(8, lowEdgeFee);
	}

	[Fact]
	public void ShouldReturnThirteenDuringSixthPeriod()
	{
		/* Sixth period is 15:00 - 15:29 */

		//Setup
		var passingDateUpperEdge = new DateTime(2025, 6, 12, 15, 00, 0);
		var passingDateMiddle = new DateTime(2025, 6, 12, 15, 15, 0);
		var passingDateLowerEdge = new DateTime(2025, 6, 12, 15, 29, 59);

		//Act
		int highEdgeFee = calc.GetTollFee(passingDateUpperEdge, vehicle);
		int midTimeFee = calc.GetTollFee(passingDateMiddle, vehicle);
		int lowEdgeFee = calc.GetTollFee(passingDateLowerEdge, vehicle);

		//Assert
		Assert.Equal(13, highEdgeFee);
		Assert.Equal(13, midTimeFee);
		Assert.Equal(13, lowEdgeFee);
	}


	[Fact]
	public void ShouldReturnEighteenDuringSeventhPeriod()
	{
		/* Seventh period is 15:30 - 16:59  */

		//Setup
		var passingDateUpperEdge = new DateTime(2025, 6, 12, 15, 30, 0);
		var passingDateMiddle = new DateTime(2025, 6, 12, 16, 45, 0);
		var passingDateLowerEdge = new DateTime(2025, 6, 12, 16, 59, 59);

		//Act
		int highEdgeFee = calc.GetTollFee(passingDateUpperEdge, vehicle);
		int midTimeFee = calc.GetTollFee(passingDateMiddle, vehicle);
		int lowEdgeFee = calc.GetTollFee(passingDateLowerEdge, vehicle);

		//Assert
		Assert.Equal(18, highEdgeFee);
		Assert.Equal(18, midTimeFee);
		Assert.Equal(18, lowEdgeFee);
	}


	[Fact]
	public void ShouldReturnThirteenDuringEightsPeriod()
	{
		/* Eights period is 17:00 - 17:59 */

		//Setup
		var passingDateUpperEdge = new DateTime(2025, 6, 12, 17, 00, 0);
		var passingDateMiddle = new DateTime(2025, 6, 12, 17, 30, 0);
		var passingDateLowerEdge = new DateTime(2025, 6, 12, 17, 59, 59);

		//Act
		int highEdgeFee = calc.GetTollFee(passingDateUpperEdge, vehicle);
		int midTimeFee = calc.GetTollFee(passingDateMiddle, vehicle);
		int lowEdgeFee = calc.GetTollFee(passingDateLowerEdge, vehicle);

		//Assert
		Assert.Equal(13, highEdgeFee);
		Assert.Equal(13, midTimeFee);
		Assert.Equal(13, lowEdgeFee);
	}


	[Fact]
	public void ShouldReturnEightDuringNinthPeriod()
	{
		/* Ninth period is 18:00 - 18:29 */

		//Setup
		var passingDateUpperEdge = new DateTime(2025, 6, 12, 18, 0, 0);
		var passingDateMiddle = new DateTime(2025, 6, 12, 18, 15, 0);
		var passingDateLowerEdge = new DateTime(2025, 6, 12, 18, 29, 59);

		//Act
		int highEdgeFee = calc.GetTollFee(passingDateUpperEdge, vehicle);
		int midTimeFee = calc.GetTollFee(passingDateMiddle, vehicle);
		int lowEdgeFee = calc.GetTollFee(passingDateLowerEdge, vehicle);

		//Assert
		Assert.Equal(8, highEdgeFee);
		Assert.Equal(8, midTimeFee);
		Assert.Equal(8, lowEdgeFee);
	}

	[Fact]
	public void ShouldReturnZeroDuringTenthPeriod()
	{
		/* Tenth period is 18:30 - 05:59 */

		//Setup
		var passingDateUpperEdge = new DateTime(2025, 6, 12, 18, 30, 0);
		var passingDateMiddle = new DateTime(2025, 6, 12, 20, 45, 0);
		var passingDateMiddleTwo = new DateTime(2025, 6, 12, 03, 33, 0);
		var passingDateLowerEdge = new DateTime(2025, 6, 12, 5, 59, 59);

		//Act
		int highEdgeFee = calc.GetTollFee(passingDateUpperEdge, vehicle);
		int midTimeFee = calc.GetTollFee(passingDateMiddle, vehicle);
		int midTimeTwoFee = calc.GetTollFee(passingDateMiddleTwo, vehicle);
		int lowEdgeFee = calc.GetTollFee(passingDateLowerEdge, vehicle);

		//Assert
		Assert.Equal(0, highEdgeFee);
		Assert.Equal(0, midTimeFee);
		Assert.Equal(0, midTimeTwoFee);
		Assert.Equal(0, lowEdgeFee);
	}
}