using FluentAssertions;
using TrafficToll.Internals.Services;
using Xunit;

namespace toll_calculator.tests;


public class UnitTests
{
    [Fact]
    public void List_group_time_spans_by_60_min_Expect_3()
    {
        IEnumerable<DateTime> spans = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0),
            new DateTime(2022,1,1,12,12,0),
            new DateTime(2022,1,1,14,0,0),
            new DateTime(2022,1,1,16,0,0),
            new DateTime(2022,1,1,16,2,0),
        };

        TollCalculatorCore.GroupByValidTollTime(spans, new TimeSpan(1, 0, 0)).Should().HaveCount(3);
    }

    [Fact]
    public void List_group_time_spans_by_60_min_Expect_1()
    {
        IEnumerable<DateTime> spans = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0)
        };

        TollCalculatorCore.GroupByValidTollTime(spans, new TimeSpan(1, 0, 0)).Should().HaveCount(1);
    }

    [Fact]
    public void List_many_group_time_spans_by_60_min_Expect_1()
    {
        IEnumerable<DateTime> spans = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0),
            new DateTime(2022,1,1,12,0,1),
            new DateTime(2022,1,1,12,0,2),
            new DateTime(2022,1,1,12,0,3),
            new DateTime(2022,1,1,12,0,4),
        };

        TollCalculatorCore.GroupByValidTollTime(spans, new TimeSpan(1, 0, 0)).Should().HaveCount(1);
    }

    [Fact]
    public void List_many_group_time_spans_by_60_min_Expect_6()
    {
        IEnumerable<DateTime> spans = new List<DateTime>()
        {
            new DateTime(2022,1,1,12,0,0),
            new DateTime(2022,1,1,13,0,0),
            new DateTime(2022,1,1,14,0,0),
            new DateTime(2022,1,1,15,0,0),
            new DateTime(2022,1,1,16,0,0),
            new DateTime(2022,1,1,17,0,0),
        };

        TollCalculatorCore.GroupByValidTollTime(spans, new TimeSpan(1, 0, 0)).Should().HaveCount(6);
    }
}
