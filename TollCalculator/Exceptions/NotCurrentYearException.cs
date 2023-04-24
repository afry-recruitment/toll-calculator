namespace TollCalculator.Exceptions
{
    [Serializable]
    public class NotCurrentYearException : ApplicationException
    {
        public NotCurrentYearException() { }

        public NotCurrentYearException(string message)
            : base(message) { }

        public NotCurrentYearException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
