namespace TollFeeCalculator.Test;

public class HolidaysTest
{

	[Fact]
	public void ShouldReturnCorrectHashSetOfHolidays()
	{
		//Setup
		/*
		* Taken from riksbank https://www.riksbank.se/en-gb/press-and-published/calendar/holidays-2025/
		* Including days before a holiday and skipping days that are known to be holidays(e.g., easter sunday)
		*/
		var dynamicHolidaysOf2025 = new HashSet<(int month, int day)>()
		{
			(4,17),
			(4,18),
			(4,21),
			(5,28),
			(5,29),
			(6,20),
			(10,31),
		};

		//Act
		HashSet<(int month, int day)> dynamicHolidays = Utils.GetDynamicHolidaysMonthDay(2025);

		//Assert
		Assert.Equal(dynamicHolidaysOf2025, dynamicHolidays);
	}

}