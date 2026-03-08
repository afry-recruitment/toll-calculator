using System;
using System.Globalization;
using TollFeeCalculator;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        //Sorterar för att säkerställa att all passeringar behandlas i kronologisk ordning. Risk = 60min-regeln ger fel resultat.
        //Kontrollen undviker att programmet kraschar om dates (en array) är null eller tom.
        if (dates == null || dates.Length == 0)
        {
            return 0;
        }

        // dates [0] = första passeringen. Här måste vi fixa en metod som kan sortera tiderna så att algoritmerna fungerar korrekt. (edge cases)
        Array.Sort(dates);
        DateTime intervalStart = dates[0];

        int totalFee = 0;
        foreach (DateTime date in dates) 
        {
            int nextFee = GetFeeForTime(date, vehicle);  
            int tempFee = GetFeeForTime(intervalStart, vehicle); 

            //long diffInMillies = date.Millisecond - intervalStart.Millisecond;
            //long minutes = diffInMillies / 1000 / 60; 
            // date.Millisecond gör fel beräkning då returnerar millisekunden i "sekunden". Vi skapar "TimeSpan" för att få korrekt tidsdifferens i "minuter"
            TimeSpan diff = date - intervalStart;
            double minutes = diff.TotalMinutes;

            //Regel 4: A vehicle should only be charged once an hour.
            //Kontrollerar hur många gånger vi passerar under 1h (60 min)
            if (minutes <= 60) 
            {
                if (totalFee > 0) totalFee -= tempFee;
                // Regel 5: If multiple passes occur within one hour, highest fee applies 
                
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
                //Uppdaterar "intervalStart" vilket kommer göra att den INTE jämförs med den gamla tiden.
                intervalStart = date;
            }
        }
        // Regel 3: Maximum fee per day = 60 SEK
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    // Regel 6: Some vehicle types are fee free
    //Metoden kontrollerar vilken typ av fordon som passerar. Vissa fordon är avgiftsfritt
    //Design-lösning = Skapa en klass för varje fordon när programmet kört klart kan vi se vilken typ av fordon som har passerat.
    private bool IsTollFreeVehicle(Vehicle vehicle)
    { 
        if (vehicle == null) return false;
        string vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }


    //Regel 1: Fees will differ between 8 SEK and 18 SEK depending on time of day
    //Regel 2: Rush-hour will render the highest fee
    //Bytte namn på metoden så att det blir läsbart och enklare att följa med koden.
    public int GetFeeForTime(DateTime date, Vehicle vehicle)

    {
        
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0; 
        int hour = date.Hour;
        int minute = date.Minute;

        
        if ((hour == 6) && (minute >= 0) && (minute <= 29)) return 8;
        else if ((hour == 6) && (minute >= 30) && (minute <= 59)) return 13;
        else if ((hour == 7) && (minute >= 0) && (minute <= 59)) return 18;
        else if ((hour == 8) && (minute >= 0) && (minute <= 29)) return 13;
        else if ((hour >= 8) && (hour <= 14) && (minute >= 30) && (minute <= 59)) return 8;
        else if ((hour == 15) && (minute >= 0) && (minute <= 29)) return 13;
        //else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18; 
        //Reglerna säger avgiften 18 SEK gäller för 15:30 - 16:59 men koden säger 15:00 - 16:59.
        else if ((hour == 15 && minute >= 30) || (hour == 16 && minute <= 59)) return 18; 
        else if ((hour == 17) && (minute >= 0) && (minute <= 59)) return 13;
        else if ((hour == 18) && (minute >= 0) && (minute <= 29)) return 8;
        
        else return 0;

        // trängsel - regler:
        //06:00 - 06:29-- > 8

        //06:30 - 06:59-- > 13

        //07:00 - 07:59-- > 18

        //08:00 - 08:29-- > 13

        //08:30 - 14:59-- > 8

        //15:00 - 15:29-- > 13

        //15:30 - 16:59-- > 18

        //17:00 - 17:59-- > 13

        //18:00 - 18:29-- > 8

        


    }

    //private Boolean IsTollFreeDate(DateTime date)
    //Kontrollerar vilket år, månad och dag det är. Vi har villkor på helger och helgdagar. Dock är det hårdkodat till 2013. Kommer att passera helger men inte helgdagar för 2026.
    private bool IsTollFreeDate(DateTime date)
    { 
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        
        //Regel 7: Weekends and holidays are fee free
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (year == 2013) 
        {   
            if (month == 1 && day == 1 ||
                month == 3 && (day == 28 || day == 29) ||
                month == 4 && (day == 1 || day == 30) ||
                month == 5 && (day == 1 || day == 8 || day == 9) ||
                month == 6 && (day == 5 || day == 6 || day == 21) ||
                month == 7 ||
                month == 11 && day == 1 ||
                month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
        }
        return false;
    }

    
    private enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}