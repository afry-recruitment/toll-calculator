namespace Afry.TollCalculator.Models;

/// <summary>
/// Represents a monetary amount stored in minor currency units (e.g., öre for SEK).
/// </summary>
public readonly struct Money(long minorUnit, string currency = "SEK")
{
    /// <summary>
    /// The amount in minor units (e.g., öre).
    /// </summary>
    public long MinorUnit { get; } = minorUnit;

    /// <summary>
    /// The ISO currency code (e.g., SEK).
    /// </summary>
    public string Currency { get; } = currency;

    private const string DefaultCurrency = "SEK";

    /// <summary>
    /// The major unit component (e.g., SEK) without the fractional minor remainder.
    /// </summary>
    public long MajorUnit => MinorUnit / 100;

    /// <summary>
    /// The minor-unit remainder (0-99) for formatting.
    /// </summary>
    public int MinorRemainder => (int)Math.Abs(MinorUnit % 100);

    /// <summary>
    /// Returns a zero amount in the given currency.
    /// </summary>
    public static Money Zero(string currency = DefaultCurrency) => new(0, currency);

    public static Money operator +(Money a, Money b)
    {
        if (!string.Equals(a.Currency, b.Currency, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Cannot add money with different currencies: '{a.Currency}' and '{b.Currency}'.");
        }

        return new Money(a.MinorUnit + b.MinorUnit, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        if (!string.Equals(a.Currency, b.Currency, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Cannot subtract money with different currencies: '{a.Currency}' and '{b.Currency}'.");
        }

        return new Money(a.MinorUnit - b.MinorUnit, a.Currency);
    }

    public static bool operator >(Money a, Money b)
    {
        if (!string.Equals(a.Currency, b.Currency, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Cannot compare money with different currencies: '{a.Currency}' and '{b.Currency}'.");
        }

        return a.MinorUnit > b.MinorUnit;
    }

    public static bool operator <(Money a, Money b)
    {
        if (!string.Equals(a.Currency, b.Currency, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Cannot compare money with different currencies: '{a.Currency}' and '{b.Currency}'.");
        }

        return a.MinorUnit < b.MinorUnit;
    }

    public static bool operator >=(Money a, Money b) => !(a < b);

    public static bool operator <=(Money a, Money b) => !(a > b);

    public override string ToString() => $"{MajorUnit}.{MinorRemainder:00} {Currency}";
}
