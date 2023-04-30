namespace TollCalculator.Exceptions
{
    [Serializable]
    public class VehicleLicensePlateException : ApplicationException
    {
        public VehicleLicensePlateException() { }

        public VehicleLicensePlateException(string message)
            : base(message) { }

        public VehicleLicensePlateException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
