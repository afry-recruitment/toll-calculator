namespace Afry.TollCalculator.Infrastructure;

/// <summary>
/// Represents the outcome of an operation.
/// </summary>
/// <typeparam name="T">The success value type.</typeparam>
public readonly record struct Result<T>(T? Value, string? Error)
{
    /// <summary>
    /// Gets whether the result represents a successful outcome.
    /// </summary>
    public bool IsSuccess => Error is null;

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static Result<T> Success(T value) => new(value, null);

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    public static Result<T> Failure(string error) => new(default, error);
}
