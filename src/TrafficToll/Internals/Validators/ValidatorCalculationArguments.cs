using TrafficToll.Internals.Enums;
using TrafficToll.Internals.Exceptions;

namespace TrafficToll.Internals.Validators
{
    internal static class ValidatorCalculationArguments
    {
        public static void EnsureArgumentsIsValid(IEnumerable<DateTime> passings, int vehicleType)
        {
            EnsurePassingsExist(passings);
            var vehicleTypeErrorMessage = ValidateVehicleType(vehicleType);
            var passingsErrorMessage = ValidatePassings(passings);
            EnsureNoErrorMessages(vehicleTypeErrorMessage + passingsErrorMessage);
        }

        private static string EnsurePassingsExist(IEnumerable<DateTime> passings)
        {
            if (passings == null)
                throw new InvalidCalculationArgumentsException("passings is null");
            if (!passings.Any())
                throw new InvalidCalculationArgumentsException("passings is empty");
            return string.Empty;
        }

        private static string ValidateVehicleType(int vehicleType)
        {
            var existingVehicles = Enum.GetValues<VehicleType>().OfType<int>();
            if (!existingVehicles.Contains(vehicleType))
                return $"Traffic type integer: {vehicleType} does not exist.\n";

            return string.Empty;
        }
        private static void EnsureNoErrorMessages(string errorMessages)
        {
            if (errorMessages.Length > 0)
                throw new InvalidCalculationArgumentsException(errorMessages);
        }

        private static string ValidatePassings(IEnumerable<DateTime> passings)
        {
            var allIsUniqueErrorMessage = ValidateAllIsUnique(passings);
            var validateToOldDatesErrorMessage = ValidateToOldDates(passings);
            var validateAllIsPastDateErrorMessage = ValidateAllIsPastDates(passings);
            
            return allIsUniqueErrorMessage + validateToOldDatesErrorMessage + validateAllIsPastDateErrorMessage;
        }

        private static string ValidateAllIsUnique(IEnumerable<DateTime> passings)
        {
            var distinct = passings.Distinct();
            if (distinct.Count() != passings.Count())
            {
                var duplicates = passings.Except(distinct).ToList();
                return $"Passings contains duplicates: {string.Join(';', duplicates)}.\n";
            }

            return string.Empty;
        }

        private static string ValidateToOldDates(IEnumerable<DateTime> passings)
        {
            var referenceStartDate = new DateTime(2000, 1, 1);
            var pastDates = passings.Where(x => x < referenceStartDate);
            if (pastDates.Any())
                return $"To old dates: {string.Join(';', pastDates)}";

            return string.Empty;
        }

        private static object ValidateAllIsPastDates(IEnumerable<DateTime> passings)
        {
            var futureDates = passings.Where(x => x > DateTime.Now);
            if (futureDates.Any())
                return $"Entered dates in the future: {string.Join(';', futureDates)}";

            return string.Empty;
        }
    }
}
