using TollCalculator.Services.Policies;

namespace TollCalculator.Tests.Policies;

public class HolidayPolicy2013Tests
{
    private readonly HolidayPolicy2013 _sut = new();

    [Fact]
    public void IsTollFree_Saturday_ReturnsTrue()
    {
        Assert.True(_sut.IsTollFree(new DateTime(2013, 1, 5)));
    }

    [Fact]
    public void IsTollFree_Sunday_ReturnsTrue()
    {
        Assert.True(_sut.IsTollFree(new DateTime(2013, 1, 6)));
    }

    [Fact]
    public void IsTollFree_KnownHoliday2013_ReturnsTrue()
    {
        Assert.True(_sut.IsTollFree(new DateTime(2013, 12, 25)));
    }

    [Fact]
    public void IsTollFree_NormalWeekday2013_ReturnsFalse()
    {
        Assert.False(_sut.IsTollFree(new DateTime(2013, 2, 13)));
    }

    [Fact]
    public void IsTollFree_Non2013Weekday_ReturnsFalse()
    {
        Assert.False(_sut.IsTollFree(new DateTime(2014, 12, 25)));
    }
}