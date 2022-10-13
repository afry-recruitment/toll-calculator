using System;
using System.Collections.Generic;
using System.Linq;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Utilities
{
    static class TollFeeCalculation
    {
        /**
        * Calculate the total toll fee for one day
        *
        * @param vehicle - the vehicle
        * @param dates   - date and time of all passes on one day
        * @return - the total toll fee for that day
        */
        public static int GetTollFeeForDate(TollStationPassage[] passages)
        {
            int total = 0;

            int highestFeeInHour = 0;
            DateTime startOfPeriod, endOfPeriod;

            for (int i = 0; i < passages.Length;)
            {
                startOfPeriod = passages[i].Time;
                endOfPeriod = startOfPeriod.AddHours(1);
                if (!passages.Any(passage => passage.Time >= startOfPeriod && passage.Time <= endOfPeriod && passage.Id != passages[i].Id))
                {
                    highestFeeInHour = passages[i].TollFee;
                    i++;
                }
                else
                {
                    highestFeeInHour = passages[i].TollFee;
                    for (int j = 1; j < passages.Length - i; j++)
                    {
                        if (passages[i + j].Time > endOfPeriod)
                        {
                            i += j;
                            break;
                        }
                        if (passages[i + j].TollFee > highestFeeInHour)
                        {
                            highestFeeInHour = passages[i + j].TollFee;
                        }
                    }
                }
                total += highestFeeInHour;
            }
            if (total > 60)
            {
                total = 60;
            }
            return total;
        }
        public static int GetTollFeeForDate(DateTime[] dates)
        {
            int totalFee = 0;
            int highestFeeInHour = 0;
            DateTime startOfPeriod, endOfPeriod;

            for (int i = 0; i < dates.Length;)
            {
                startOfPeriod = dates[i];
                endOfPeriod = startOfPeriod.AddHours(1);
                if (!dates.Any(passage => passage > startOfPeriod && passage <= endOfPeriod))
                {
                    highestFeeInHour = TollStationPassage.GetTollFee(dates[i]);
                    i++;
                }
                else
                {
                    highestFeeInHour = TollStationPassage.GetTollFee(dates[i]);
                    for (int j = 1; j < dates.Length - i; j++)
                    {
                        if (dates[i + j] > endOfPeriod)
                        {
                            i += j;
                            break;
                        }
                        if (TollStationPassage.GetTollFee(dates[i + j]) > highestFeeInHour)
                        {
                            highestFeeInHour = TollStationPassage.GetTollFee(dates[i + j]);
                        }
                    }
                }
                totalFee += highestFeeInHour;
            }
            if (totalFee > 60) totalFee = 60;
            return totalFee;
        }
        public static int CalculateTollFeeForDate(Vehicle vehicle, DateTime date)
        {
            int total = 0;

            if (!vehicle.IsTollFree)
            {
                var passages = TollStationPassage.AllTollStationPassages.Where(v => v.Vehicle == vehicle && v.Time.Date == date).ToArray();
                total = GetTollFeeForDate(passages);
            }
            return total;
        }
        public static int CalculateTollFeeForDate(Vehicle vehicle, TollStationPassage[] passages)
        {
            int total = 0;

            if (!vehicle.IsTollFree)
            {
                total = GetTollFeeForDate(passages);
            }
            return total;
        }
        public static int CalculateTollFeeForDate(Vehicle vehicle, DateTime[] dates)
        {
            int total = 0;

            if (!vehicle.IsTollFree)
            {
                total = GetTollFeeForDate(dates);
            }
            return total;
        }
        public static int CalculateTollFeeForDate(TollStationPassage[] passages)
        {
            int total = 0;

            int highestFeeInHour = 0;
            DateTime startOfPeriod, endOfPeriod;

            for (int i = 0; i < passages.Length;)
            {
                startOfPeriod = passages[i].Time;
                endOfPeriod = startOfPeriod.AddHours(1);
                if (!passages.Any(passage => passage.Time >= startOfPeriod && passage.Time <= endOfPeriod && passage.Id != passages[i].Id))
                {
                    highestFeeInHour = passages[i].TollFee;
                    i++;
                }
                else
                {
                    highestFeeInHour = passages[i].TollFee;
                    for (int j = 1; j < passages.Length - i; j++)
                    {
                        if (passages[i + j].Time > endOfPeriod)
                        {
                            i += j;
                            break;
                        }
                        if (passages[i + j].TollFee > highestFeeInHour)
                        {
                            highestFeeInHour = passages[i + j].TollFee;
                        }
                    }
                }
                total += highestFeeInHour;
            }
            if (total > 60)
            {
                total = 60;
            }
            return total;
        }
        public static Dictionary<DateTime, int> CalculateTotalTollFee(TollStationPassage[] passages)
        {
            Dictionary<DateTime, int> totalFees = new();
            int total, highestFeeInHour;
            DateTime startOfPeriod, endOfPeriod, currentDate;

            var grouped = passages.GroupBy(p => p.Time.Date);

            foreach (var date in grouped)
            {
                total = 0;
                highestFeeInHour = 0;
                currentDate = date.Key.Date;
                for (int i = 0; i < date.Count();)
                {
                    startOfPeriod = date.ElementAt(i).Time;
                    endOfPeriod = startOfPeriod.AddHours(1);
                    if (!date.Any(passage => passage.Time >= startOfPeriod && passage.Time <= endOfPeriod && passage.Id != date.ElementAt(i).Id))
                    {
                        highestFeeInHour = date.ElementAt(i).TollFee;
                        i++;
                    }
                    else
                    {
                        highestFeeInHour = date.ElementAt(i).TollFee;
                        for (int j = 1; j < date.Count() - i; j++)
                        {
                            if (date.ElementAt(i + j).Time > endOfPeriod)
                            {
                                i += j;
                                break;
                            }
                            if (date.ElementAt(i + j).TollFee > highestFeeInHour)
                            {
                                highestFeeInHour = date.ElementAt(i + j).TollFee;
                            }
                        }
                    }
                    total += highestFeeInHour;
                }
                if (total > 60)
                {
                    total = 60;
                }
                totalFees.Add(currentDate, total);
            }
            return totalFees;
        }
        public static Dictionary<DateTime, int> CalculateTotalTollFee(Vehicle vehicle, int year = 0, int month = 0)
        {
            Dictionary<DateTime, int> totalFees = new();
            int total, highestFeeInHour;
            DateTime startOfPeriod, endOfPeriod, currentDate;
            TollStationPassage[] passages;

            if (year == 0 && month == 0)
            {
                passages = TollStationPassage.AllTollStationPassages.Where(v => v.Vehicle == vehicle).ToArray();
            }
            else if (month == 0)
            {
                passages = TollStationPassage.AllTollStationPassages.Where(v => v.Vehicle == vehicle && v.Time.Year == year).ToArray();
            }
            else if (year == 0)
            {
                passages = TollStationPassage.AllTollStationPassages.Where(v => v.Vehicle == vehicle && v.Time.Month == month).ToArray();
            }
            else
            {
                passages = TollStationPassage.AllTollStationPassages.Where(v => v.Vehicle == vehicle && v.Time.Year == year && v.Time.Month == month).ToArray();
            }

            var grouped = passages.GroupBy(p => p.Time.Date);

            foreach (var date in grouped)
            {
                total = 0;
                highestFeeInHour = 0;
                currentDate = date.Key.Date;
                for (int i = 0; i < date.Count();)
                {
                    startOfPeriod = date.ElementAt(i).Time;
                    endOfPeriod = startOfPeriod.AddHours(1);
                    if (!date.Any(passage => passage.Time >= startOfPeriod && passage.Time <= endOfPeriod && passage.Id != date.ElementAt(i).Id))
                    {
                        highestFeeInHour = date.ElementAt(i).TollFee;
                        i++;
                    }
                    else
                    {
                        highestFeeInHour = date.ElementAt(i).TollFee;
                        for (int j = 1; j < date.Count() - i; j++)
                        {
                            if (date.ElementAt(i + j).Time > endOfPeriod)
                            {
                                i += j;
                                break;
                            }
                            if (date.ElementAt(i + j).TollFee > highestFeeInHour)
                            {
                                highestFeeInHour = date.ElementAt(i + j).TollFee;
                            }
                        }
                    }
                    total += highestFeeInHour;
                }
                if (total > 60)
                {
                    total = 60;
                }
                totalFees.Add(currentDate, total);
            }
            return totalFees;
        }
    }
}
