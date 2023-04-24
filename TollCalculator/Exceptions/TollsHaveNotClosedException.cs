namespace TollCalculator.Exceptions
{
    [Serializable]
    public class TollsHaveNotClosedException : ApplicationException
    {
        public TollsHaveNotClosedException() { }

        public TollsHaveNotClosedException(string message)
            : base(message) { }

        public TollsHaveNotClosedException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
