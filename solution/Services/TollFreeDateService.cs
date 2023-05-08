using System.Globalization;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TollFeeCalculator
{

  public class TollFreeDateService : ITollFreeDateService
  {
    // Holidays are tricky, as one can easily be added further along in time,
    // plus they are culture specific. 
    // If I was to rely on third party library, 
    // I would have to make sure to take into account upgrading my reference 
    // when/if holidays change  
    // So, only importing some code for calculating easter.
    // orginal code counts whole of July as holiday. 
    // not sure if that  might change in the future or not
    // so keeping that in function called summerHoliday


    List<Holiday> staticHolidays = new List<Holiday> {
        new Holiday { month = 1, day = 1 }, //Nyårsdagen
        new Holiday {month = 1, day = 6},    //Trettondagsafton
        new Holiday {month = 4, day = 30},   // Valborg 
        new Holiday {month = 5, day = 1},    // 1 Maj
        new Holiday {month = 6, day = 6},    // Nationaldagen
        new Holiday { month = 12, day = 24},  // Julafton
        new Holiday {month = 12, day = 25},   // Juldag
        new Holiday {month = 12, day = 26},   //Annandag jul
        new Holiday { month = 12 , day = 31}  // Nyårsafton
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

    private bool isWeekend(DateTime date)
    {
      return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }
    private bool isSummerHoliday(DateTime date)
    {
      return date.Month == 7;
    }


    private List<Holiday> initializeVariableHolidays(DateTime easter)
    {
      var skartorsdag = easter.AddDays(-3);
      var longFriday = easter.AddDays(-2);
      var annandagPask = easter.AddDays(2);
      var kristiFlygare = easter.AddDays(39);
      // Midsommar == First friday between 6 and 19 juni
      var midsommarafton = findFriday(new DateTime(easter.Year, 6, 19));
      // Allhelgona == First friday from 30 oktober
      var allhelgona = findFriday(new DateTime(easter.Year, 10, 30));

      return new List<Holiday> {
        new Holiday { month = skartorsdag.Month, day= skartorsdag.Day},
        new Holiday { month = longFriday.Month, day = longFriday.Day},
        new Holiday { month = annandagPask.Month, day = annandagPask.Day},
        new Holiday { month = kristiFlygare.Month, day = kristiFlygare.Day},
        new Holiday { month = midsommarafton.Month, day = midsommarafton.Day},
        new Holiday { month = allhelgona.Month, day = allhelgona.Day}
      };
    }
    private bool isHoliday(DateTime date, List<Holiday> Holidays)
    {
      return Holidays.Any(candidate => (candidate.day == date.Day && candidate.month == date.Month));
    }


    private DateTime findFriday(DateTime candidate)
    {
      while (candidate.DayOfWeek != DayOfWeek.Friday)
      {
        candidate = candidate.AddDays(1);
      }
      return candidate;
    }


  }


}