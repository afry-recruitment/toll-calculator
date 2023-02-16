namespace TollCalculatorJon
{
    public class TollCalculator
    {
        /**
     * Calculate the total toll that is needed to pay
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time
     * @return - the total toll
     */
        public int GetTotalTollFee(Vehicle vehicle, DateTime[] dates)
        {
            List<List<DateTime>> dailyTolls = new List<List<DateTime>>();
            DateTime currentDate = dates[0].Date;
            List<DateTime> currentTolls = new List<DateTime>();
            foreach (DateTime date in dates)
            {
                if (date.Date == currentDate)
                {
                    currentTolls.Add(date);
                }
                else
                {
                    dailyTolls.Add(currentTolls);
                    currentDate = date.Date;
                    currentTolls = new List<DateTime> { date };
                }
            }

            dailyTolls.Add(currentTolls);

            return dailyTolls.Sum(dailyToll => GetDailyTollFee(vehicle, dailyToll.ToArray()));
        }


        /**
     * Calculate the total toll of one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll, caps at 60
     */
        public int GetDailyTollFee(Vehicle vehicle, DateTime[] dates)
        {
            if (IsTollFreeDate(dates[0].Date) || IsTollFreeVehicle(vehicle)) return 0;

            TimeSpan startingHourTollTime = dates[0].TimeOfDay;
            int totalFee = 0;
            int lastFee = 0;
            foreach (DateTime date in dates)
            {
                if (date.TimeOfDay - startingHourTollTime < new TimeSpan(1, 0, 0))
                {
                    int tempFee = GetTollFee(date.TimeOfDay);
                    if (lastFee < tempFee)
                    {
                        lastFee = tempFee;
                    }
                }
                else
                {
                    totalFee += lastFee;
                    lastFee = GetTollFee(date.TimeOfDay);
                    startingHourTollTime = date.TimeOfDay;
                }
            }

            totalFee += lastFee;
            if (totalFee > 60) totalFee = 60;
            return totalFee;
        }

        /**
     *  Calculates if a vehicle is exempt from congestion tax
     * 
     *  @param vehicle - the vehicle
     *  @return - Boolean that shows if the vehicle was exempt
     */
        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            //Since the congestion tax only applies to cars, it does not need to check if the vehicle
            //is specifically a tractor, a motorbike and etc, since they all fall under the definition of not being a car

            return vehicle is not { Car: true, Emergency: false, Diplomat: false, Military: false };

            //As of 2020 foreign cars are no longer exempt from paying congestion tax
        }

        /**
     *  Calculates how much is needed to pay in tax depending on the time when the vehicle passed through the toll
     * 
     *  @param time - The time the vehicle passed the toll
     *  @return - The amount that the toll is worth at that time
     */
        public int GetTollFee(TimeSpan time)
        {
            if (time >= new TimeSpan(6, 0, 0) && time <= new TimeSpan(6, 29, 0))
                return 8;
            if (time >= new TimeSpan(6, 30, 0) && time <= new TimeSpan(6, 59, 0))
                return 13;
            if (time >= new TimeSpan(7, 0, 0) && time <= new TimeSpan(7, 59, 0))
                return 18;
            if (time >= new TimeSpan(8, 0, 0) && time <= new TimeSpan(8, 29, 0))
                return 13;
            if (time >= new TimeSpan(8, 30, 0) && time <= new TimeSpan(14, 59, 0))
                return 8;
            if (time >= new TimeSpan(15, 0, 0) && time <= new TimeSpan(15, 29, 0))
                return 13;
            if (time >= new TimeSpan(15, 30, 0) && time <= new TimeSpan(16, 59, 0))
                return 18;
            if (time >= new TimeSpan(17, 0, 0) && time <= new TimeSpan(17, 59, 0))
                return 13;
            if (time >= new TimeSpan(18, 0, 0) && time <= new TimeSpan(18, 29, 0))
                return 8;
            return 0;
        }


        /**
     *  Calculates if the current date is an toll free date like a holiday or the weekend
     * 
     *  @param date - The date the vehicle passed the toll
     *  @return - If the date is on a holiday or on the weekend
     */
        private Boolean IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            //Since earlier code has also included the day before a holiday, this has followed Stockholm laws and also
            //include the days before holidays with the exception of Långfredag (Good friday), Kristi himmelfärdsdag (Ascension day)
            //and Alla helgons dag (All saints' day).
            //The day before Första maj (International Workers' day) and Sveriges nationaldag (National day of Sweden)
            //is also not included, unless the holiday takes place on a saturday.
            //Laws followed can be read here: https://www.riksdagen.se/sv/dokument-lagar/dokument/svensk-forfattningssamling/lag-2004629-om-trangselskatt_sfs-2004-629


            List<DateTime> holidayList = new List<DateTime>();

            //Nyår (New Year)
            holidayList.Add(new DateTime(date.Year, 1, 1));

            //Trettondedag jul (Twelfth night)
            holidayList.Add(new DateTime(date.Year, 1, 5));
            holidayList.Add(new DateTime(date.Year, 1, 6));

            //Långfredagen (Good friday)
            holidayList.Add(EasterDate(date.Year).AddDays(-3));
            holidayList.Add(EasterDate(date.Year).AddDays(-2));

            //Annandag påsk (Easter Monday)
            holidayList.Add(EasterDate(date.Year).AddDays(+1));

            //Första maj (International Workers' Day)
            holidayList.Add(new DateTime(date.Year, 5, 1).DayOfWeek != DayOfWeek.Saturday
                ? new DateTime(date.Year, 5, 1)
                : new DateTime(date.Year, 4, 30));

            //Kristi himmelsfärdsdag (Ascension Day)
            holidayList.Add(EasterDate(date.Year).AddDays(+39));

            //Sveriges nationaldag (National Day of Sweden)
            holidayList.Add(new DateTime(date.Year, 6, 6).DayOfWeek != DayOfWeek.Saturday
                ? new DateTime(date.Year, 6, 6)
                : new DateTime(date.Year, 6, 5));

            //Midsommar (Midsummer)
            DateTime midsommerEve = new DateTime(date.Year, 6, 25);
            while (midsommerEve.DayOfWeek != DayOfWeek.Friday)
                midsommerEve = midsommerEve.AddDays(-1);
            holidayList.Add(midsommerEve);

            //Juldagen (Christmas day)
            holidayList.Add(new DateTime(date.Year, 12, 24));
            holidayList.Add(new DateTime(date.Year, 12, 25));

            //Annandag jul (Boxing day)
            holidayList.Add(new DateTime(date.Year, 12, 26));

            //Nyårsafton (New Years eve)
            holidayList.Add(new DateTime(date.Year, 12, 31));

            //Free toll days that would naturally always take place on a saturday or sunday are not included

            return holidayList.Contains(date);

		//TODO: Check with customer if July is also supposed to be tollfree
		//TODO: Double check with customer if holidays are correct, specificaly if the day before the holiday is tollfree
        }

        /**
     *  Calculates the date Easter takes place in a specific year
     * 
     *  @param year - The year that needs to be calculated
     *  @return - The date that easter takes place
     */
        private static DateTime EasterDate(int year)
        {
            int g = year % 19;
            int c = year / 100;
            int h = (c - (c / 4) - ((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (h / 28) * (1 - (h / 28) * (29 / (h + 1)) * ((21 - g) / 11));

            int day = i - ((year + (year / 4) + i + 2 - c + (c / 4)) % 7) + 28;
            int month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }
    }
}