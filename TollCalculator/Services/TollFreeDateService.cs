
namespace TollFeeCalculator
{
  // Holidays are tricky, as one can easily be added further along in time,
  // plus they are culture specific. 
  // If I was to rely on third party library, 
  // I would have to make sure to take into account upgrading my reference 
  // when/if holidays change  
  // I have nicked some code for calculating easter.
  // and chosen to calculate the rest myself
  // orginal code counts whole of July as holiday. 
  // not sure if that  might change in the future or not
  // so keeping that in function called summerHoliday

  public class TollFreeDateService : ITollFreeDateService
  {

    List<Holiday> staticHolidays = new List<Holiday> {
        new Holiday { Month = 1, Day = 1 }, //Nyårsdagen
        new Holiday {Month = 1, Day = 6},    //Trettondagsafton
        new Holiday {Month = 4, Day = 30},   // Valborg 
        new Holiday {Month = 5, Day = 1},    // 1 Maj
        new Holiday {Month = 6, Day = 6},    // Nationaldagen
        new Holiday { Month = 12, Day = 24},  // Julafton
        new Holiday {Month = 12, Day = 25},   // Juldag
        new Holiday {Month = 12, Day = 26},   //Annandag jul
        new Holiday { Month = 12 , Day = 31}  // Nyårsafton
    };
    public TollFreeDateService()
    {
      EasterService variableEaster = new EasterService();
    }

    public bool IsTollFree(DateTime date)
    {

      if (isWeekend(date) || isSummerHoliday(date) || isHoliday(date, staticHolidays))
      {
        return true;
      }
      else
      {
        return isHoliday(date, initializeVariableHolidays(EasterService.EasterDateForYear(date.Year)));
      }
    }

    private DateTime findFriday(DateTime candidate)
    {
      while (candidate.DayOfWeek != DayOfWeek.Friday)
      {
        candidate = candidate.AddDays(1);
      }
      return candidate;
    }
    private List<Holiday> initializeVariableHolidays(DateTime easter)
    {
      var maundyThursday = easter.AddDays(-3);
      var longFriday = easter.AddDays(-2);
      var annandagPask = easter.AddDays(2);
      var kristiFlygare = easter.AddDays(39);
      // Midsommar == First friday between 6 and 19 juni
      var midsommarafton = findFriday(new DateTime(easter.Year, 6, 19));
      // Allhelgona == First friday from 30 oktober
      var allhelgona = findFriday(new DateTime(easter.Year, 10, 30));

      return new List<Holiday> {
        new Holiday { Month = maundyThursday.Month, Day= maundyThursday.Day},
        new Holiday { Month = longFriday.Month, Day = longFriday.Day},
        new Holiday { Month = annandagPask.Month, Day = annandagPask.Day},
        new Holiday { Month = kristiFlygare.Month, Day = kristiFlygare.Day},
        new Holiday { Month = midsommarafton.Month, Day = midsommarafton.Day},
        new Holiday { Month = allhelgona.Month, Day = allhelgona.Day}
      };
    }
    private bool isHoliday(DateTime date, List<Holiday> Holidays)
    {
      return Holidays.Any(candidate => (candidate.Day == date.Day && candidate.Month == date.Month));
    }

    private bool isSummerHoliday(DateTime date)
    {
      return date.Month == 7;
    }
    private bool isWeekend(DateTime date)
    {
      return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }
  }
}