using congestion.calculator;
using System;

public interface ICongestionTaxCalculator
{
    int GetTax(string vehicle, DateTime[] dates);
}