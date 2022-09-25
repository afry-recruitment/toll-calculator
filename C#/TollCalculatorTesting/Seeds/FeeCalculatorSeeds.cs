using System.Collections.Generic;

public class FeeCalculatorSeeds
{
    public List<DateTime> GetLowTrafficPeriods()
    {
        return new List<DateTime>(){
            new DateTime(2022, 09, 26, 6, 15, 0),
            new DateTime(2022, 09, 26, 9, 15, 0),
            new DateTime(2022, 09, 26, 18, 15, 0)};
    }

    public List<DateTime> GetMediumTrafficPeriods()
    {
        return new List<DateTime>(){
            new DateTime(2022, 09, 26, 6, 45, 0),
            new DateTime(2022, 09, 26, 8, 15, 0),
            new DateTime(2022, 09, 26, 15, 15, 0),
            new DateTime(2022, 09, 26, 17, 15, 0)};
    }

    public List<DateTime> GetHighrafficPeriods()
    {
        return new List<DateTime>(){
            new DateTime(2022, 09, 26, 7, 15, 0),
            new DateTime(2022, 09, 26, 15, 45, 0)
        };
    }

    public List<DateTime> GetDublicatHoursPeriods()
    {
        return new List<DateTime>(){
            new DateTime(2022, 09, 26, 6, 15, 0),
            new DateTime(2022, 09, 26, 6, 35, 0),
            new DateTime(2022, 09, 26, 9, 15, 0),
            new DateTime(2022, 09, 26, 9, 35, 0),
            new DateTime(2022, 09, 26, 18, 15, 0)
        };
    }

    public List<DateTime> GetManyPeriods()
    {
        return new List<DateTime>(){
            new DateTime(2022, 09, 26, 7, 15, 0),
            new DateTime(2022, 09, 26, 8, 22, 0),
            new DateTime(2022, 09, 26, 9, 15, 0),
            new DateTime(2022, 09, 26, 9, 35, 0),
            new DateTime(2022, 09, 26, 15,45, 0),
            new DateTime(2022, 09, 26, 6, 15, 0),
            new DateTime(2022, 09, 26, 6, 35, 0),
            new DateTime(2022, 09, 26, 10, 15, 0),
            new DateTime(2022, 09, 26, 12, 35, 0),
            new DateTime(2022, 09, 26, 18,15, 0)
        };
    }

}