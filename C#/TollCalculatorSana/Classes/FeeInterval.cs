namespace TollCalculatorSana.Classes;

public class FeeInterval
{
    public int Id { get; set; }
    public int FeeValue { get; set; }
    public TimeOnly From { get; set; }
    public TimeOnly To { get; set; }
}
