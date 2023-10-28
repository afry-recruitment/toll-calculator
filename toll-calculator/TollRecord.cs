
public class TollRecord
{
    public Vehicle vehicle;
    public DateTime[] instances;

    public TollRecord(Vehicle v, DateTime[] t) 
    {
        vehicle = v;
        instances = t;
    }
}
