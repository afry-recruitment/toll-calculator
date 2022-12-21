using System.Diagnostics.CodeAnalysis;

namespace TrafficToll.Internals.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class InvalidClientInputException : TollCalculatorBaseException
{
    public InvalidClientInputException() { }
    public InvalidClientInputException(string message) : base(message) { }
    public InvalidClientInputException(string message, Exception inner) : base(message, inner) { }
    private InvalidClientInputException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}