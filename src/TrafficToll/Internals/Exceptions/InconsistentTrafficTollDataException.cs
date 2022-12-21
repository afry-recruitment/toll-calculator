using System.Diagnostics.CodeAnalysis;

namespace TrafficToll.Internals.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class InconsistentTrafficTollDataException : TollCalculatorBaseException
{
    public InconsistentTrafficTollDataException() { }
    public InconsistentTrafficTollDataException(string message) : base(message) { }
    public InconsistentTrafficTollDataException(string message, Exception inner) : base(message, inner) { }
    private InconsistentTrafficTollDataException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
