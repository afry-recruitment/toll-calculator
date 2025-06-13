namespace TollFeeCalculator;

public static class Utils
{
	/**
	* Calculates holidays without fixed date
	*
	* @param year   - the year to calculate easter for
	* @return - hashset<DateTime> of holidays and the days before a holiday that are not weekends
	*/
	public static HashSet<DateTime> GetDynamicHolidays(int year)
	{
		return [.. GetEasterBasedHolidays(year), GetMidsommarEve(year), GetDayBeforeAllSaintsDay(year)];
	}

	/**
	* Calculates holidays without fixed date
	*
	* @param year   - the year to calculate easter for
	* @return - hashset<(int month, int day)> of holidays and the days before a holiday that are not weekends
	*/

	public static HashSet<(int month, int day)> GetDynamicHolidaysMonthDay(int year)
	{
		return GetDynamicHolidays(year)
			.Select(h => (h.Month, h.Day))
			.ToHashSet(); ;
	}

	/**
     * Calculates easter day based on the year
     * which is calculated based on Meeus/Jones/Butcher" algorithm
     * See https://en.wikipedia.org/wiki/Date_of_Easter#Anonymous_Gregorian_algorithm
	 *
     * @param year   - the year to calculate easter for
     * @return - the easter day datetime object
     */
	private static DateTime GetEasterDay(int year)
	{
		int a = year % 19;
		int b = year / 100;
		int c = year % 100;
		int d = b / 4;
		int e = b % 4;
		int f = (b + 8) / 25;
		int g = (b - f + 1) / 3;
		int h = (19 * a + b - d - g + 15) % 30;
		int i = c / 4;
		int k = c % 4;
		int l = (32 + 2 * e + 2 * i - h - k) % 7;
		int m = (a + 11 * h + 22 * l) / 451;
		int month = (h + l - 7 * m + 114) / 31;
		int day = ((h + l - 7 * m + 114) % 31) + 1;

		return new DateTime(year, month, day);
	}

	private static HashSet<DateTime> GetEasterBasedHolidays(int year)
	{
		DateTime easterSunday = GetEasterDay(year);
		DateTime goodFriday = easterSunday.AddDays(-2);
		DateTime dayBeforeGoodFriday = goodFriday.AddDays(-1);
		DateTime easterMonday = easterSunday.AddDays(1);
		DateTime ascensionDay = easterSunday.AddDays(39);
		DateTime dayBeforeAscensionDay = ascensionDay.AddDays(-1);
		return [dayBeforeGoodFriday, goodFriday, easterMonday, dayBeforeAscensionDay, ascensionDay];
	}

	private static DateTime GetMidsommarEve(int year)
	{
		return Enumerable.Range(0, 7)
			.Select(offset => new DateTime(year, 6, 20).AddDays(offset))
			.First(d => d.DayOfWeek == DayOfWeek.Friday);
	}

	private static DateTime GetDayBeforeAllSaintsDay(int year)
	{
		return Enumerable.Range(0, 6)
			.Select(offset => new DateTime(year, 10, 31).AddDays(offset))
			.First(d => d.DayOfWeek == DayOfWeek.Friday);
	}
}