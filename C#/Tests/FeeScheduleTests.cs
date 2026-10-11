namespace TollFeeCalculator.Tests;

/// <summary>
/// Rule: "Fees will differ between 8 SEK and 18 SEK, depending on the time of
/// day." Every schedule window, at its boundaries and just outside, is checked.
/// </summary>
public class FeeScheduleTests
{
    private readonly TollCalculator _calculator = new();

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(5, 59, 0)]
    [InlineData(6, 0, 8)]
    [InlineData(6, 29, 8)]
    [InlineData(6, 30, 13)]
    [InlineData(6, 59, 13)]
    [InlineData(7, 0, 18)]
    [InlineData(7, 59, 18)]
    [InlineData(8, 0, 13)]
    [InlineData(8, 29, 13)]
    [InlineData(8, 30, 8)]
    [InlineData(9, 0, 8)]
    [InlineData(12, 0, 8)]
    [InlineData(14, 29, 8)]
    [InlineData(14, 59, 8)]
    [InlineData(15, 0, 13)]
    [InlineData(15, 29, 13)]
    [InlineData(15, 30, 18)]
    [InlineData(16, 0, 18)]
    [InlineData(16, 59, 18)]
    [InlineData(17, 0, 13)]
    [InlineData(17, 59, 13)]
    [InlineData(18, 0, 8)]
    [InlineData(18, 29, 8)]
    [InlineData(18, 30, 0)]
    [InlineData(23, 59, 0)]
    public void Single_pass_is_charged_the_expected_fee(int hour, int minute, int expected)
    {
        int actual = _calculator.GetTollFee(new Car(), new[] { Days.Tue(hour, minute) });

        Assert.Equal(expected, actual);
    }
}
