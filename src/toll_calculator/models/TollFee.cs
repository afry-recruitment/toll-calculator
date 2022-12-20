namespace toll_calculator.models;

internal sealed class TollFee
{
    public TollFee(string name, int prize)
    {
        Name = name;
        Prize = prize;
    }

    public string Name { get; }
    public int Prize { get; }
    //public int LowTraffic { get; } = 8;
    //public int MidTraffic { get; } = 13;
    //public int RushHourTraffic { get; } = 18;
}

