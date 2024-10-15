namespace TollFeeCalculator.Models;

public class Vehicle
{
    public string RegistrationNumber { get; private set; }
    public bool IsDailyCapped => DailyToll.Item1 >= 60;
    public (int, DateTime) DailyToll { get; private set; }
    public List<DateTime> Passages { get; private set; }

    public Vehicle(string registrationNumber)
    {
        RegistrationNumber = registrationNumber;
        Passages = new List<DateTime>();
        DailyToll = (0, DateTime.Today);
    }

    public void RecordPassage(DateTime passageTime)
    {
        Passages.Add(passageTime);
    }

    public void DailyTollSetter((int, DateTime) dailyToll)
    {
        //In case we want to adjust some rule for what namespace can have access to this method so we don't expose setters out
        DailyToll = dailyToll;
    }
}
