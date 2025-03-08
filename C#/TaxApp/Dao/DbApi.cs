
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace TaxApp;
/*
    This class is responsible for fetching data from the database.
*/
public class DbApi : IDbApi
{
    private readonly IConfiguration _configuration;
    public DbApi(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public List<KeyValuePair<DateTime, int>> GetDBFees(int cityId)
    {
        List<KeyValuePair<DateTime, int>> fees = new List<KeyValuePair<DateTime, int>>();
        
        var connectionString = string.Concat($"Data Source={Environment.CurrentDirectory}/",  _configuration.GetSection("DatabaseName").Value);

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM TollFee Where CityId = $id";
        var cmd = connection.CreateCommand();
        cmd.CommandText = sql;

        cmd.Parameters.AddWithValue("id", cityId);

        var r = cmd.ExecuteReader();
        while (r.Read())
        {
            fees.Add(new KeyValuePair<DateTime, int>(r.GetDateTime(2), r.GetInt32(3)));
        }
            

        return fees;
    }

}
