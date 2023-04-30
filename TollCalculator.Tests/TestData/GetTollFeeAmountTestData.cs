using System;
using System.Collections.Generic;
namespace TollCalculator.Tests.TestData
{
    public class GetTollFeeAmountTestData : TheoryData<DateTime, int>
    {
        public GetTollFeeAmountTestData()
        {
            // 6:00, 8
            // 6:30, 13
            // 7:00, 18
            // 8:00, 13
            // 8:30, 8
            // 9:00 - 14:00, 8
            // 15:00, 13
            // 15:30, 18
            // 16:00, 18
            // 17:00, 13
            // 18:00, 8
            // 18:30 - 06:00, 0

            Add(new DateTime(DateTime.Now.Year, 3, 1, hour: 6, 15, 0), 8);
            Add(new DateTime(DateTime.Now.Year, 3, 1, 6, 45, 0), 13);
            Add(new DateTime(DateTime.Now.Year, 3, 1, 7, 15, 0), 18);
            Add(new DateTime(DateTime.Now.Year, 3, 1, 8, 15, 0), 13);
            Add(new DateTime(DateTime.Now.Year, 3, 1, 8, 35, 0), 8);
            Add(new DateTime(DateTime.Now.Year, 3, 1, 10, 15, 0), 8);
            Add(new DateTime(DateTime.Now.Year, 3, 1, 15, 15, 0), 13);
            Add(new DateTime(DateTime.Now.Year, 3, 1, 15, 45, 0), 18);
            Add(new DateTime(DateTime.Now.Year, 3, 1, 16, 30, 0), 18);
            Add(new DateTime(DateTime.Now.Year, 3, 1, 17, 15, 0), 13);
            Add(new DateTime(DateTime.Now.Year, 3, 1, 18, 15, 0), 8);
            Add(new DateTime(DateTime.Now.Year, 3, 1, 20, 15, 0), 0);
        }
    }
}
