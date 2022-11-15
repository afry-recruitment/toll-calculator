using TollCalculator.Models;

namespace TollCalculator.ViewModels
{
    public class TollVehicleViewModel
    {
        private IVehicle _vehicle;

        public IVehicle Vehicle
        {
            get { return _vehicle; }
            set
            {
                if (value is null)
                    throw new ArgumentNullException("Provided vehicle was was invalid.");

                _vehicle = value;
            }
        }

        private DateTime[] _tollPassesDuringDay;

        public DateTime[] TollPassesDuringDay
        {
            get => _tollPassesDuringDay;
            set
            {
                if (value is null)
                    throw new ArgumentNullException("Tollpasses for vehicle provided was invalid.");

                _tollPassesDuringDay = value;
            }
        }

        public string GetProvidedVehicleAndTollPasses()
        {
            string tollDateOutput = "";

            foreach (DateTime tollDate in _tollPassesDuringDay)
                tollDateOutput += ",";

            return $"{nameof(_vehicle)} {tollDateOutput}.";
        }
    }
}
