namespace TollFeeCalculator.Services.Interfaces;

public interface ITollFreeDataService
{
    List<string> LoadTollFreeVehicles(string configFilePath);
    List<DateTime> LoadTollFreeDates(string configFilePath);
}
