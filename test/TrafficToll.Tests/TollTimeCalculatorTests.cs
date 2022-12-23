using FluentAssertions;
using TrafficToll.Internals.Services;
using Xunit;

namespace toll_calculator.tests;


public class TollTimeCalculatorTests
{

    [Fact]
    public void Group_integers()
    {
        // Arrange
        IEnumerable<int> ints = new List<int>()
        {
            0,
            1,
            2
        };

        // Act & Assert
        var group = ints.GroupBy(x => x)
            .Select(x => x.ToArray());

        group.Should().NotBeEmpty();
        group.ToArray()[0][0].Should().Be(0);
    }

    [Fact]
    public void Group_date_time()
    {
        // Arrange
        IEnumerable<DateTime> dateTimes = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0),
            new DateTime(2022,1,1,12,12,0),
            new DateTime(2022,1,1,14,0,0),
            new DateTime(2022,1,1,16,0,0),
            new DateTime(2022,1,1,16,2,0),
        };

        // Act & Assert
        var group = dateTimes.GroupBy(x => x)
            .Select(x => x.ToList());

        group.Should().NotBeEmpty();
        group.ToArray()[0][0].Should().Be(new DateTime(2022, 1, 1, 12, 0, 0));
    }

    [Fact]
    public void Group_date_time_by_day_of_year()
    {
        // Arrange
        IEnumerable<DateTime> dateTimes = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0),
            new DateTime(2022,1,1,12,12,0),
            new DateTime(2022,1,1,14,0,0),
            new DateTime(2022,1,2,16,0,0),
            new DateTime(2022,1,2,16,2,0),
        };

        // Act & Assert
        var group = dateTimes.GroupBy(x => x.DayOfYear)
            .Select(x => x.ToList());

        group.Should().HaveCount(2);
        group.ToArray()[0][0].Should().Be(new DateTime(2022, 1, 1, 12, 0, 0));
    }

    [Fact]
    public void Group_date_time_by_day_and_by_year()
    {
        // Arrange
        IEnumerable<DateTime> dateTimes = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0),
            new DateTime(2022,1,1,12,12,0),
            new DateTime(2023,1,1,14,0,0),
            new DateTime(2022,1,2,16,0,0),
            new DateTime(2022,1,2,16,2,0),
        };

        // Act & Assert
        var group = dateTimes.GroupBy(x => x.Year.ToString() + x.DayOfYear.ToString())
            .Select(x => x.ToList());

        group.Should().HaveCount(3);
        group.ToArray()[0][0].Should().Be(new DateTime(2022, 1, 1, 12, 0, 0));
    }

    [Fact]
    public void Group_1_date_time_by_day_and_by_year()
    {
        // Arrange
        IEnumerable<DateTime> dateTimes = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0),
        };

        // Act & Assert
        var group = dateTimes.GroupBy(x => x.Year.ToString() + x.DayOfYear.ToString())
            .Select(x => x.ToList());

        group.Should().HaveCount(1);
        group.ToArray()[0][0].Should().Be(new DateTime(2022, 1, 1, 12, 0, 0));
    }

    [Fact]
    public void List_group_time_dateTimes_by_60_min_Expect_1()
    {
        // Arrange
        IEnumerable<DateTime> dateTimes = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0)
        };

        // Act & Assert
        TollPassingsJoin.GroupPassingsByValidTollTime(dateTimes, new TimeSpan(1, 0, 0)).Should().HaveCount(1);
    }

    [Fact]
    public void List_group_time_dateTimes_by_60_min_Expect_3()
    {
        // Arrange
        IEnumerable<DateTime> dateTimes = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0),
            new DateTime(2022,1,1,12,12,0),
            new DateTime(2022,1,1,14,0,0),
            new DateTime(2022,1,1,16,0,0),
            new DateTime(2022,1,1,16,2,0),
        };

        // Act & Assert
        TollPassingsJoin.GroupPassingsByValidTollTime(dateTimes, new TimeSpan(1, 0, 0)).Should().HaveCount(3);
    }

    [Fact]
    public void List_many_group_time_dateTimes_by_60_min_Expect_1()
    {
        // Arrange
        IEnumerable<DateTime> dateTimes = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0),
            new DateTime(2022,1,1,12,0,1),
            new DateTime(2022,1,1,12,0,2),
            new DateTime(2022,1,1,12,0,3),
            new DateTime(2022,1,1,12,0,4),
        };

        // Act & Assert
        TollPassingsJoin.GroupPassingsByValidTollTime(dateTimes, new TimeSpan(1, 0, 0)).Should().HaveCount(1);
    }

    [Fact]
    public void List_many_group_time_dateTimes_by_60_min_Expect_6()
    {
        // Arrange
        IEnumerable<DateTime> dateTimes = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0),
            new DateTime(2022,1,1,13,0,0),
            new DateTime(2022,1,1,14,0,0),
            new DateTime(2022,1,1,15,0,0),
            new DateTime(2022,1,1,16,0,0),
            new DateTime(2022,1,1,17,0,0),
        };

        // Act & Assert
        TollPassingsJoin.GroupPassingsByValidTollTime(dateTimes, new TimeSpan(1, 0, 0)).Should().HaveCount(6);
    }

    [Fact]
    public void List_two_dates_expect_2_groups()
    {
        // Arrange
        IEnumerable<DateTime> dateTimes = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,7,0),
            new DateTime(2022,1,2,12,7,1),
        };

        // Act & Assert
        TollPassingsJoin.GroupPassingsByDayOfTheYearAndYear(dateTimes).Should().HaveCount(2);
    }

    [Fact]
    public void List_two_dates_same_day_expect_1_groups()
    {
        // Arrange
        IEnumerable<DateTime> dateTimes = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,7,0),
            new DateTime(2022,1,1,12,7,1),
        };

        // Act & Assert
        TollPassingsJoin.GroupPassingsByDayOfTheYearAndYear(dateTimes).Should().HaveCount(1);
    }

    [Fact]
    public void List_same_date_different_year_expect_2_groups()
    {
        // Arrange
        IEnumerable<DateTime> dateTimes = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,7,0),
            new DateTime(2023,1,1,12,7,1),
        };

        // Act & Assert
        TollPassingsJoin.GroupPassingsByDayOfTheYearAndYear(dateTimes).Should().HaveCount(2);
    }

    [Fact]
    public void Group_by_60_minutes()
    {
        // Arrange
        var date0 = new DateTime(2022, 1, 1, 12, 0, 0);
        var date1 = new DateTime(2022, 1, 1, 12, 59, 0);

        var addedDate = date0 + new TimeSpan(0, 59, 0);
        addedDate.Should().Be(date1);

        var maxDate = date0 + new TimeSpan(1, 0, 0);
        (date1 < maxDate).Should().BeTrue();
        (date1 > date0).Should().BeTrue();

        //var startTime = timeSpans.First();
        //var group = timeSpans.Where(x => x >= startTime && x < startTime + validTollTime);
        //groupList.Add(group);
    }
}
