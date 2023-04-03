namespace TollCalculatorSana.Classes;

public class Vehicle
{
    public string Type { get; set; } = string.Empty;
    public string RegNum { get; set; } = string.Empty;
    public Vehicle(string type, string regNum)
    {
        Type = type;
        RegNum = regNum;
    }
}
