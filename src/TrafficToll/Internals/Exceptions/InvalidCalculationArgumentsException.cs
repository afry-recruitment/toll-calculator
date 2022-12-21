using System.Diagnostics.CodeAnalysis;

namespace TrafficToll.Internals.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class InvalidCalculationArgumentsException : TollCalculatorBaseException
{
    public InvalidCalculationArgumentsException() { }
    public InvalidCalculationArgumentsException(string message) : base(message) { }
    public InvalidCalculationArgumentsException(string message, Exception inner) : base(message, inner) { }
    private InvalidCalculationArgumentsException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}