using Moq;
using TollFeeCalculator.TollFees;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculatorTests.TollFeesTests.TollFeeTests
{
    public class GetTollFeeForVehicleTests
    {
        private readonly Mock<ITollFeeRepository> _tollFeeRepositoryMock = new();
        private readonly Mock<IVehicle> _vehicleMock = new();

        public GetTollFeeForVehicleTests()
        {
            _tollFeeRepositoryMock.DefaultValue = DefaultValue.Mock;
            _vehicleMock.DefaultValue = DefaultValue.Mock;
            SetupTollFeeRepositoryMock();
            SetStandardCarVehicleMock();
        }

        [Fact]
        public void ShouldReturnZero_IfNoTripsHaveBeenMade()
        {
            const int expectedResult = 0;

            var actualResult = TollFee.GetTollFeeForVehicle(_vehicleMock.Object, new List<DateTime>(),
                _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnZero_IfVehicleClassificationIsTollFeeFree()
        {
            const int expectedResult = 0;
            var dates = new List<DateTime>() { new(2022, 12, 01, 12, 0, 0) };

            _vehicleMock.Setup(v => v.VehicleClassification)
                .Returns(VehicleClassification.Emergency);

            var actualResult = TollFee.GetTollFeeForVehicle(_vehicleMock.Object, dates, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnZero_IfVehicleTypeIsTollFeeFree()
        {
            const int expectedResult = 0;
            var dates = new List<DateTime>() { new(2022, 12, 01, 12, 0, 0) };

            _vehicleMock.Setup(v => v.VehicleType)
                .Returns(VehicleType.Motorbike);


            var actualResult = TollFee.GetTollFeeForVehicle(_vehicleMock.Object, dates, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnTollFee_IfVehicleIsNotTollFeeFree()
        {
            const int expectedResult = 8;
            var dates = new List<DateTime>() { new(2022, 12, 01, 12, 0, 0) };

            var actualResult = TollFee.GetTollFeeForVehicle(_vehicleMock.Object, dates, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(WithinOne60MinIntervalTestData))]
        public void ShouldReturnHighestTollFeeIn60MinInterval_IfNonTollFeeFreeVehicle(IList<DateTime> dates, int expectedResult)
        {
            var actualResult = TollFee.GetTollFeeForVehicle(_vehicleMock.Object, dates, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(TripsOverSeveral60MinIntervalsTestData))]
        public void ShouldReturnSumOfHighestTollFeesIn60MinIntervals_IfNonTollFeeFreeVehicle(IList<DateTime> dates, int expectedResult)
        {
            var actualResult = TollFee.GetTollFeeForVehicle(_vehicleMock.Object, dates, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnZero_IfDateIsTollFeeFree()
        {
            const int expectedResult = 0;
            var dates = new List<DateTime>()
            {
                new (2013, 1, 1, 12, 0, 0),
                new (2013, 1, 1, 15, 30, 0),
                new (2013, 6, 6, 9, 15, 0),
                new (2013, 12, 24, 15, 0, 0)
            };

            var actualResult = TollFee.GetTollFeeForVehicle(_vehicleMock.Object, dates, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnZero_IfWeekDayIsTollFeeFree()
        {
            const int expectedResult = 0;
            var dates = new List<DateTime>()
            {
                new(2022, 12, 3, 10, 9, 0),
                new(2022, 12, 4, 13, 54, 0)
            };

            var actualResult = TollFee.GetTollFeeForVehicle(_vehicleMock.Object, dates, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnCorrectTollFee_IfVehicleIsNotTollFeeFree()
        {
            const int expectedResult = 44;
            var dates = new List<DateTime>()
            {
                new (2013, 1, 1, 12, 0, 0),
                new (2013, 1, 1, 15, 30, 0),
                new (2013, 6, 6, 9, 15, 0),
                new (2013, 12, 24, 15, 0, 0),
                new (2022, 12, 01, 12, 25, 01),
                new (2022, 12, 01, 12, 50, 00),
                new (2022, 12, 01, 13, 25, 00),
                new(2022, 12, 3, 10, 9, 0),
                new(2022, 12, 4, 13, 54, 0),
                new (2022, 12, 8, 7, 50, 0),
                new (2022, 12, 8, 8, 20, 0),
                new (2022, 12, 8, 8, 29, 0),
                new (2022, 12, 8, 8, 40,0),
                new(2022, 12, 13, 4, 47, 0),
                new(2022, 12, 13, 7, 15, 0)
            };

            var actualResult = TollFee.GetTollFeeForVehicle(_vehicleMock.Object, dates, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotHaveATotalFeeLargerThenMaxTollFeePerDay()
        {
            const int expectedResult = 60;
            var dates = new List<DateTime>()
            {
                new (2022, 12, 01, 6, 0, 0),
                new (2022, 12, 01, 7, 0, 0),
                new (2022, 12, 01, 8, 0, 0),
                new (2022, 12, 01, 9, 0, 0),
                new (2022, 12, 01, 10, 0, 0),
                new (2022, 12, 01, 11, 0, 0),
                new (2022, 12, 01, 12, 0, 0),
                new (2022, 12, 01, 13, 0, 0),
                new (2022, 12, 01, 14, 0, 0),
                new (2022, 12, 01, 15, 0, 0),
                new (2022, 12, 01, 16, 0, 0),
                new (2022, 12, 01, 17, 0, 0),
                new (2022, 12, 01, 18, 0, 0),
            };

            var actualResult = TollFee.GetTollFeeForVehicle(_vehicleMock.Object, dates, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldAddOtherDaysTollFeesToTollFeesFromADayWithMaxDayTollFee()
        {
            const int expectedResult = 73;
            var dates = new List<DateTime>()
            {
                new (2022, 12, 01, 6, 0, 0),
                new (2022, 12, 01, 7, 0, 0),
                new (2022, 12, 01, 8, 0, 0),
                new (2022, 12, 01, 9, 0, 0),
                new (2022, 12, 01, 10, 0, 0),
                new (2022, 12, 01, 11, 0, 0),
                new (2022, 12, 01, 12, 0, 0),
                new (2022, 12, 01, 13, 0, 0),
                new (2022, 12, 01, 14, 0, 0),
                new (2022, 12, 01, 15, 0, 0),
                new (2022, 12, 01, 16, 0, 0),
                new (2022, 12, 01, 17, 0, 0),
                new (2022, 12, 01, 18, 0, 0),
                new (2022, 12, 02, 08, 0, 0),
            };

            var actualResult = TollFee.GetTollFeeForVehicle(_vehicleMock.Object, dates, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldThrowArgumentNullException_IfNoVehicleIsSpecified()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                TollFee.GetTollFeeForVehicle(null, new List<DateTime>(), _tollFeeRepositoryMock.Object));
            Assert.StartsWith("Vehicle cannot be null", exception.Message);
        }

        [Fact]
        public void ShouldThrowArgumentNullException_IfNoTollFeeRepositoryIsSpecified()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                TollFee.GetTollFeeForVehicle(_vehicleMock.Object, new List<DateTime>(), null));

            Assert.StartsWith("TollRepository cannot be null!", exception.Message);
        }


        public static IEnumerable<object[]> TripsOverSeveral60MinIntervalsTestData()
        {
            var expectedResult = 31;
            var dates = new List<DateTime>()
            {
                new(2022, 12, 01, 06, 15, 0),
                new(2022, 12, 01, 06, 45, 0),
                new(2022, 12, 01, 07, 35, 0)
            };
            yield return new object[] { dates, expectedResult };

            expectedResult = 26;
            dates = new List<DateTime>()
            {
                new(2022, 12, 01, 06, 59, 59),
                new(2022, 12, 01, 08, 00, 00)
            };
            yield return new object[] { dates, expectedResult };
        }


        public static IEnumerable<object[]> WithinOne60MinIntervalTestData()
        {
            var expectedResult = 18;
            var dates = new List<DateTime>()
            {
                new (2022, 12, 01, 14, 50, 0),
                new (2022, 12, 01, 15, 20, 0),
                new (2022, 12, 01, 15, 45, 0)
            };
            yield return new object[] { dates, expectedResult };

            expectedResult = 13;
            dates = new List<DateTime>()
            {
                new (2022, 12, 01, 5, 50, 0),
                new (2022, 12, 01, 6, 20, 0),
                new (2022, 12, 01, 6, 45, 0)
            };
            yield return new object[] { dates, expectedResult };

            expectedResult = 18;
            dates = new List<DateTime>()
            {
                new (2022, 12, 01, 7, 50, 0),
                new (2022, 12, 01, 8, 20, 0),
                new (2022, 12, 01, 8, 29, 0),
                new (2022, 12, 01, 8, 40,0)
            };
            yield return new object[] { dates, expectedResult };


            expectedResult = 8;
            dates = new List<DateTime>()
            {
                new (2022, 12, 01, 9, 50, 0),
                new (2022, 12, 01, 10, 20, 0),
                new (2022, 12, 01, 10, 45, 0)
            };
            yield return new object[] { dates, expectedResult };


            expectedResult = 13;
            dates = new List<DateTime>()
            {
                new (2022, 12, 01, 17, 50, 0),
                new (2022, 12, 01, 18, 20, 0),
                new (2022, 12, 01, 18, 45, 0)
            };
            yield return new object[] { dates, expectedResult };


            expectedResult = 8;
            dates = new List<DateTime>()
            {
                new (2022, 12, 01, 12, 25, 0),
                new (2022, 12, 01, 12, 50, 0),
                new (2022, 12, 01, 13, 24, 59)
            };
            yield return new object[] { dates, expectedResult };

            expectedResult = 8;
            dates = new List<DateTime>()
            {
                new (2022, 12, 01, 12, 25, 01),
                new (2022, 12, 01, 12, 50, 00),
                new (2022, 12, 01, 13, 25, 00)
            };
            yield return new object[] { dates, expectedResult };

            expectedResult = 13;
            dates = new List<DateTime>()
            {
                new (2022, 12, 01, 17, 50, 0),
                new (2022, 12, 01, 18, 25, 0),
                new (2022, 12, 01, 18, 35, 0)
            };
            yield return new object[] { dates, expectedResult };

        }


        private void SetStandardCarVehicleMock()
        {
            _vehicleMock.Setup(v => v.VehicleClassification).Returns(VehicleClassification.Standard);
            _vehicleMock.Setup(v => v.VehicleType).Returns(VehicleType.Car);
        }

        private void SetupTollFeeRepositoryMock()
        {
            _tollFeeRepositoryMock.Setup(v => v.TollFeesForTimeIntervals)
                .Returns(GetTollFeeTimeIntervals);

            _tollFeeRepositoryMock.Setup(v => v.TollFeeFreeVehicleClassifications)
                .Returns(GetTollFreeVehicleClassifications);

            _tollFeeRepositoryMock.Setup(v => v.TollFeeFreeVehicleTypes)
                .Returns(GetTollFreeVehicleTypes);

            _tollFeeRepositoryMock.Setup(v => v.TollFeeFreeDaysOfWeek)
                .Returns(GetTollFeeFreeDaysOfWeek);

            _tollFeeRepositoryMock.Setup(v => v.TollFeeFreeDates)
                .Returns(GetTollFreeDates);

            _tollFeeRepositoryMock.Setup(t => t.MaxTollFeePerDay)
                .Returns(GetMaxTollFeePerDay);

            _tollFeeRepositoryMock.Setup(v => v.CombineTollFeeTimeSpan)
                .Returns(GetCombineTollFeeTimeSpan);

        }

        private static IList<VehicleClassification> GetTollFreeVehicleClassifications()
        {
            return new List<VehicleClassification>()
            {
                VehicleClassification.Diplomat,
                VehicleClassification.Emergency,
                VehicleClassification.Foreign,
                VehicleClassification.Military
            };
        }

        private static IList<VehicleType> GetTollFreeVehicleTypes()
        {
            return new List<VehicleType>()
            {
                VehicleType.Motorbike,
                VehicleType.Tractor,
            };
        }

        private static IList<TollFeeTimeInterval> GetTollFeeTimeIntervals()
        {
            return new List<TollFeeTimeInterval>()
            {
                new(new TimeOnly(6, 0), new TimeOnly(6, 30), 8),
                new(new TimeOnly(6, 30), new TimeOnly(7, 0), 13),
                new(new TimeOnly(7, 0), new TimeOnly(8, 0), 18),
                new(new TimeOnly(8, 0), new TimeOnly(8, 30), 13),
                new(new TimeOnly(8, 30), new TimeOnly(14, 59), 8),
                new(new TimeOnly(15, 0), new TimeOnly(15, 30), 13),
                new(new TimeOnly(15, 30), new TimeOnly(17, 0), 18),
                new(new TimeOnly(17, 0), new TimeOnly(18, 0), 13),
                new(new TimeOnly(18, 0), new TimeOnly(18, 30), 8),
            };
        }

        private static IList<DateOnly> GetTollFreeDates()
        {
            List<DateOnly> tollFreeDates = new()
            {
                new DateOnly(2013, 1, 1),
                new DateOnly(2013, 3, 28),
                new DateOnly(2013, 3, 29),
                new DateOnly(2013, 4, 1),
                new DateOnly(2013, 4, 30),
                new DateOnly(2013, 5, 1),
                new DateOnly(2013, 5, 8),
                new DateOnly(2013, 5, 9),
                new DateOnly(2013, 6, 5),
                new DateOnly(2013, 6, 6),
                new DateOnly(2013, 6, 21),
                new DateOnly(2013, 11, 1),
                new DateOnly(2013, 12, 24),
                new DateOnly(2013, 12, 25),
                new DateOnly(2013, 12, 26),
                new DateOnly(2013, 12, 31)
            };

            for(var day = 1; day <= 31; day++)
            {
                tollFreeDates.Add(new DateOnly(2013, 7, day));
            }

            return tollFreeDates;
        }

        private static IList<DayOfWeek> GetTollFeeFreeDaysOfWeek()
        {
            return new List<DayOfWeek>()
            {
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };
        }

        private static int GetMaxTollFeePerDay()
        {
            return 60;
        }

        private static TimeSpan GetCombineTollFeeTimeSpan()
        {
            return new TimeSpan(1, 0, 0);
        }
    }
}
