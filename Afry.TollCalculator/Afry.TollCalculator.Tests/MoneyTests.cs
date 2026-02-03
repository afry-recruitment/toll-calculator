using Afry.TollCalculator.Models;

namespace Afry.TollCalculator.Tests;

public sealed class MoneyTests
{
    [Fact]
    public void Add_SameCurrency_AddsMinorUnits()
    {
        // Arrange
        var a = new Money(1500, "SEK");
        var b = new Money(250, "SEK");

        // Act
        var sum = a + b;

        // Assert
        Assert.Equal(1750, sum.MinorUnit);
        Assert.Equal("SEK", sum.Currency);
    }

    [Fact]
    public void Add_DifferentCurrencies_Throws()
    {
        // Arrange
        var a = new Money(100, "SEK");
        var b = new Money(100, "NOK");

        // Act + Assert
        Assert.Throws<InvalidOperationException>(() => _ = a + b);
    }

    [Fact]
    public void Compare_DifferentCurrencies_Throws()
    {
        // Arrange
        var a = new Money(100, "SEK");
        var b = new Money(200, "NOK");

        // Act + Assert
        Assert.Throws<InvalidOperationException>(() => _ = a < b);
    }

    [Fact]
    public void ToString_FormatsMajorAndMinorUnits()
    {
        // Arrange
        var money = new Money(12345, "SEK");

        // Act
        var text = money.ToString();

        // Assert
        Assert.Equal("123.45 SEK", text);
    }
}
