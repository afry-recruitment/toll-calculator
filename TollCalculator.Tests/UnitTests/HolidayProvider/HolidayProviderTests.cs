using PublicHoliday;
using TollCalculator.Holidays;

namespace TollCalculator.Tests.UnitTests.HolidayProvider;

[TestFixture]
public class HolidayProviderTests
{
    private static readonly IHolidayProvider ExternalHolidayProvider = new ExternalHolidayProvider(new SwedenPublicHoliday());

    private static IEnumerable<DateTime> HolidayDates2013()
    {
        // January
        yield return new DateTime(2013, 1, 1);

        // March
        // yield return new DateTime(2013, 3, 28);
        yield return new DateTime(2013, 3, 29);

        // April
        yield return new DateTime(2013, 4, 1);
        // yield return new DateTime(2013, 4, 30);

        // May
        yield return new DateTime(2013, 5, 1);
        // yield return new DateTime(2013, 5, 8);
        yield return new DateTime(2013, 5, 9);

        // June
        // yield return new DateTime(2013, 6, 5);
        yield return new DateTime(2013, 6, 6);
        yield return new DateTime(2013, 6, 21);

        // July
        // for (int day = 1; day <= 31; day++)
        //     yield return new DateTime(2013, 7, day);

        // November
        // yield return new DateTime(2013, 11, 1);

        // December
        yield return new DateTime(2013, 12, 24);
        yield return new DateTime(2013, 12, 25);
        yield return new DateTime(2013, 12, 26);
        yield return new DateTime(2013, 12, 31);
    }

    [Test]
    [TestCaseSource(nameof(HolidayDates2013))]
    public void IsHoliday_ShouldReturnTrue_ForAllConfiguredDates(DateTime date)
    {
        Assert.That(ExternalHolidayProvider.IsHoliday(date), Is.True);
    } 
}