namespace TaxApp;

public interface ITaxService
{
    public int GetTax(IVehicle vehicle, List<DateTime> dates, int cityId = -1);
    public int GetFee(DateTime date, List<KeyValuePair<DateTime, int>> CityFees);
}
