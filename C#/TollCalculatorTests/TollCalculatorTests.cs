#nullable disable
using AutoFixture;
using FakeItEasy;
using FluentAssertions;
using TollCalculator.Models;
using TollCalculator.Services;

namespace TollCalculatorTests
{
    [TestClass]
    public class TollCalculatorTests
    {
        private ITollCalculator _sut;
        private Fixture _fixture;
        private Fake<IDateService> _dateService;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _dateService = new Fake<IDateService>();
            _sut = new TollCalculator.Services.TollCalculator(_dateService.FakedObject);
        }

        [TestMethod]
        public void CalculateTollFee_NoDates_Throws()
        {
            // Arrange
            var vehicle = _fixture.Create<Car>();
            var passageTimes = new List<DateTime>();

            // Act
            var actualAction = () => _sut.CalculateTollFee(vehicle, passageTimes);

            // Assert
            actualAction.Should().Throw<ArgumentException>();
        }


        [DataTestMethod]
        [DataRow("2023-01-01T08:00:00", 13)]
        [DataRow("2023-01-01T18:03:00", 8)]
        [DataRow("2023-01-01T05:43:00", 0)]
        public void CalculateTollFee_OneCarPassageOnNonHoliday_ShouldReturnCorrectToll(string passageDateTimeString, int fee)
        {
            // Arrange
            var vehicle = _fixture.Create<Car>();
            var passage = DateTime.Parse(passageDateTimeString);
            var passageTimes = new List<DateTime>() { passage };
            _dateService.CallsTo(x => x.IsHoliday(A<DateTime>._)).Returns(false);
            var expected = (Decimal) fee;

            // Act
            var actual = _sut.CalculateTollFee(vehicle, passageTimes);

            // Assert
            actual.Should().Be(expected);
        }


        [TestMethod]
        public void CalculateTollFee_OneCarPassageOnHoliday_ShouldReturnZero()
        {
            // Arrange
            var vehicle = _fixture.Create<Car>();
            var passage = _fixture.Create<DateTime>();
            var passageTimes = new List<DateTime>() { passage };
            _dateService.CallsTo(x => x.IsHoliday(passage.Date)).Returns(true);
            var expected = 0;

            // Act
            var actual = _sut.CalculateTollFee(vehicle, passageTimes);

            // Assert
            actual.Should().Be(expected);
        }

    }
}
