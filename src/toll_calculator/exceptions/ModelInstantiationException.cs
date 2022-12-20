using System.Diagnostics.CodeAnalysis;

namespace toll_calculator.exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class ModelInstantiationException : TollCalculatorBaseException
{
    public override string Reason => "Configuration Setting Missing";
    public ModelInstantiationException() { }
    public ModelInstantiationException(string message) : base(message) { }
    public ModelInstantiationException(string message, Exception inner) : base(message, inner) { }
    private ModelInstantiationException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}