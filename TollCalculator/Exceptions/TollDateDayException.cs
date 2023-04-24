namespace TollCalculator.Exceptions
{
    [Serializable]
    public class TollDateDayException : ApplicationException
    {
        public TollDateDayException() { }

        public TollDateDayException(string message)
            : base(message) { }

        public TollDateDayException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
