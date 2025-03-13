namespace TollFeeCalculator.Models;

public record TollFeeCalculationResult(DateOnly Date, decimal TotalFee, string Currency);