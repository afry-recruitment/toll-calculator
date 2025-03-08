namespace TaxApp;

public interface IDbApi
{
    List<KeyValuePair<DateTime, int>> GetDBFees(int cityId);
}
