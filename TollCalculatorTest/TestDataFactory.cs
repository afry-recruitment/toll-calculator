using Moq;
using toll_calculator_jakob.Models;

namespace TollCalculatorTest
{
    public static class TestDataFactory
    {
        public static List<DateTime> GenerateTestDates()
        {
            return
            [
                new DateTime(2024, 11, 14, 6, 15, 0, 100),
                new DateTime(2024, 11, 14, 6, 45, 59, 250),
                new DateTime(2024, 11, 14, 7, 15, 0, 500),
                new DateTime(2024, 11, 14, 15, 10, 0, 750),
                new DateTime(2024, 11, 14, 18, 10, 59, 999),
            ];
        }

        public static List<DateTime> GenerateTestDatesOverLimit()
        {
            return
            [
                new DateTime(2024, 11, 14, 6, 15, 0, 100),
                new DateTime(2024, 11, 14, 6, 45, 0, 250),
                new DateTime(2024, 11, 14, 7, 15, 0, 500),
                new DateTime(2024, 11, 14, 8, 15, 0, 500),
                new DateTime(2024, 11, 14, 15, 10, 0, 750),
                new DateTime(2024, 11, 14, 16, 30, 0, 999),
                new DateTime(2024, 11, 14, 18, 10, 59, 999)
            ];
        }

        public static List<DateTime> GenerateTestDatesHoliday()
        {
            return
            [
                new DateTime(2024, 12, 24, 6, 15, 0, 100),
                new DateTime(2024, 12, 24, 6, 45, 0, 250),
                new DateTime(2024, 12, 24, 7, 15, 0, 0),
                new DateTime(2024, 12, 24, 15, 10, 0, 750),
                new DateTime(2024, 12, 24, 18, 10, 0, 999),
            ];
        }

        public static IVehicle CreateTestVehicle(bool isTollFree = false)
        {
            var mockVehicle = new Mock<IVehicle>();
            mockVehicle.Setup(v => v.IsTollFree).Returns(isTollFree);

            return mockVehicle.Object;
        }
    }
}
