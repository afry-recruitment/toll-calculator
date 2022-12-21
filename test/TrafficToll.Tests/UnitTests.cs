//using FluentAssertions;
//using Xunit;

//namespace toll_calculator.tests;


//public class UnitTests
//{
//    [Fact]
//    public void List_group_time_spans_by_60_min_Expect_3()
//    {
//        IEnumerable<TimeSpan> spans = new List<TimeSpan>()
//        {
//            new TimeSpan(12,0,0),
//            new TimeSpan(12,12,0),
//            new TimeSpan(14,0,0),
//            new TimeSpan(16,0,0),
//            new TimeSpan(16,2,0),
//        };

//        TollCalculator.GroupByMaxValidTollTime(spans, new TimeSpan(1,0,0)).Should().HaveCount(3);
//    }

//    [Fact]
//    public void List_group_time_spans_by_60_min_Expect_1()
//    {
//        IEnumerable<TimeSpan> spans = new List<TimeSpan>()
//        {
//            new TimeSpan(12,0,0)
//        };

//        TollCalculator.GroupByMaxValidTollTime(spans, new TimeSpan(1, 0, 0)).Should().HaveCount(1);
//    }

//    [Fact]
//    public void List_many_group_time_spans_by_60_min_Expect_1()
//    {
//        IEnumerable<TimeSpan> spans = new List<TimeSpan>()
//        {
//            new TimeSpan(12,0,0),
//            new TimeSpan(12,0,1),
//            new TimeSpan(12,0,2),
//            new TimeSpan(12,0,3),
//            new TimeSpan(12,0,4),
//        };

//        TollCalculator.GroupByMaxValidTollTime(spans, new TimeSpan(1, 0, 0)).Should().HaveCount(1);
//    }

//    [Fact]
//    public void List_many_group_time_spans_by_60_min_Expect_6()
//    {
//        IEnumerable<TimeSpan> spans = new List<TimeSpan>()
//        {
//            new TimeSpan(12,0,0),
//            new TimeSpan(13,0,0),
//            new TimeSpan(14,0,0),
//            new TimeSpan(15,0,0),
//            new TimeSpan(16,0,0),
//            new TimeSpan(17,0,0),
//        };

//        TollCalculator.GroupByMaxValidTollTime(spans, new TimeSpan(1, 0, 0)).Should().HaveCount(6);
//    }
//}
