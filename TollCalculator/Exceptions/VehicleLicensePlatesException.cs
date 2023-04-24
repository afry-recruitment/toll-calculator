namespace TollCalculator.Exceptions
{
    [Serializable]
    public class VehicleLicensePlateMismatchEx : ApplicationException
    {
        public VehicleLicensePlateMismatchEx() { }

        public VehicleLicensePlateMismatchEx(string message)
            : base(message) { }

        public VehicleLicensePlateMismatchEx(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
