using System.Diagnostics.CodeAnalysis;

namespace toll_calculator.exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class TrafficTollDataRetrievalException : TollCalculatorBaseException
{
    public TrafficTollDataRetrievalException() { }
    public TrafficTollDataRetrievalException(string message) : base(message) { }
    public TrafficTollDataRetrievalException(string message, Exception inner) : base(message, inner) { }
    private TrafficTollDataRetrievalException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
