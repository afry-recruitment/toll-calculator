using TollCalculator.Builder;
using TollCalculator.Vehicles;

namespace TollCalculator.Tests.StepDefinitions
{
    [Binding]
    public class GetTollFeeStepDefinitions
    {
        private VehicleType _vehicle;
        private List<DateTime> _dates = [];
        private decimal _result;
       
        [Given("the vehicle is {string}")]
        public void GivenTheVehicleIs(string p0)
        {
            if (Enum.TryParse<VehicleType>(p0, out var vehicle))
            {
                _vehicle = vehicle;
            }
            else
            {
                throw new ArgumentException("Invalid vehicle type");
            }
        }

        [Given("the dates are {string}")]
        public void GivenTheDatesAre(string p0)
        {
            _dates.AddRange(p0.Split(';').Select(p => DateTime.Parse(p)));
        }

        [When("the toll is calculated")]
        public void WhenTheTollIsCalculated()
        {
            var calc = new TollCalculator();
            _result = calc.GetTollFee(_vehicle, _dates);
        }

        [Then("the result should be {int}")]
        public void ThenTheResultShouldBe(decimal p0)
        {
            Assert.AreEqual(p0, _result);
        }
    }
}
