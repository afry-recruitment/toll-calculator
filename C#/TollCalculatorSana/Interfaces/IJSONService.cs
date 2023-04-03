namespace TollCalculatorSana.Interfaces;

public interface IJSONService
{
    public List<T> GetJSON<T>(string file) where T : class;
}
