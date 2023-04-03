using Newtonsoft.Json;

namespace TollCalculatorSana.Services;

public class JSONService : IJSONService
{    

    public List<T> GetJSON<T>(string file) where T : class
    {
        List<T> items = new List<T>();
        using (StreamReader r = new StreamReader(file))
        {
            string json = r.ReadToEnd();
            if (json != null)
            {
                items = JsonConvert.DeserializeObject<List<T>>(json);
            }
        }
        return items;
    }
}
