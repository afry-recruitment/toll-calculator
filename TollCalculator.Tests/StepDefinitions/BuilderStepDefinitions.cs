using System;
using Reqnroll;
using TollCalculator.Builder;
using TollCalculator.Config;
using TollCalculator.Vehicles;

namespace TollCalculator.Tests.StepDefinitions
{
    [Binding]
    public class BuilderStepDefinitions
    {
        private TollCalculatorBuilder? _builder;
        private VehicleType _vehicleType;
        private List<DateTime> _dates = [];
        private decimal? _result;

        [Given("A builder is created")]
        public void GivenABuilderIsCreated()
        {
            _builder = TollCalculatorBuilder.Create();
        }

        [Given("{string} dates are added")]
        public void GivenHolidayDatesAreAdded(string p0)
        {
            var holidays = p0.Split(';')
                .Where(p => !string.IsNullOrEmpty(p))
                .Select(d => DateOnly.Parse(d));

            _builder?.AddHolidays(holidays);
        }

        [Given("daily fee {decimal} set")]
        public void GivenDailyFeeSet(decimal p0)
        {
            _builder?.SetDailyLimit(p0);
        }

        [Given("{string} for times are added")]
        public void GivenForTimesAreAdded(string p0)
        {
            var zones = p0.Split(";")
                .Select(s => {
                    var parts = s.Split(":");
                    var hours = parts[0].Split("-");
                    return new KeyValuePair<FeeTimeZone, decimal>(new FeeTimeZone(
                        new TimeSpan(Int32.Parse(hours[0]), 0, 0),
                        new TimeSpan(Int32.Parse(hours[1]), 0, 0)),
                        decimal.Parse(parts[1]));
                }).ToDictionary();

            _builder?.AddTimeZones(zones);
        }


        [When("the {string} are set")]
        public void WhenTheAreSet(string p0)
        {
            _dates.AddRange(p0.Split(';')
                .Where(p => !string.IsNullOrEmpty(p))
                .Select(p => DateTime.Parse(p)));
        }

        [Given("{string} added")]
        public void GivenFreeVehiclesAdded(string p0)
        {
            foreach(var v in p0.Split(";").Where(p => !string.IsNullOrEmpty(p)))
            {
                if (!Enum.TryParse<VehicleType>(v, out var freeVehicle))
                {
                    throw new ArgumentException("invalid vehicle type");
                }

                _builder?.AddTollFreeVehicles([freeVehicle]);
            }
        }
        
        [When("the {string} is set")]
        public void WhenTheTypeIsSet(string p0)
        {
            Enum.TryParse<VehicleType>(p0, out var vehicleType);
            _vehicleType = vehicleType;
        }

        [When("a calculation is performed")]
        public void WhenACalculationIsPerformed()
        {
            var calculator = _builder?.Build();
            _result = calculator?.GetTollFee(_vehicleType, _dates);
        }

        [Then("the config is used and the result is {decimal}")]
        public void ThenTheConfigIsUsedAndTheResultIs(decimal p0)
        {
            Assert.AreEqual(p0, _result);
        }        
    }
}
