namespace TollFeeCalculator.Services.Interfaces;

public interface ITollFreeService
{
    bool IsTollFreeVehicle(string registrationNumber);
    bool IsTollFreeDate(DateTime date);
}
