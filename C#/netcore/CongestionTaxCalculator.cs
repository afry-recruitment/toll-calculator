using System;
using System.Collections.Generic;
using congestion.calculator;
using System.Linq;
using congestion_tax_api.Models;

public class CongestionTaxCalculator : ICongestionTaxCalculator
{
    /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total congestion tax for that day
         */

    private static readonly HashSet<string> vehiclesTollfree = Enum.GetNames(typeof(TollFreeVehicles))
            .Select(x => x.ToLower())
            .ToHashSet();

    public int GetTax(string vehicle, DateTime[] dates)
    {
        if (string.IsNullOrWhiteSpace(vehicle))
        {
            return 0;
        }
        var _rates = TaxRates.lstTaxRates;
        int totalFee = 0;
        DateTime intervalStart = dates[0];
        foreach (DateTime date in dates)
        {
            var nextFee = GetTollFee(date, vehicle, _rates);
            var diffInMinutes = date.Subtract(intervalStart).TotalMinutes;
            var feeToAdd = nextFee;

            if (diffInMinutes <= 60 && Math.Sign(totalFee) == +1)
            {
                var tempFee = GetTollFee(intervalStart, vehicle, _rates);
                totalFee -= tempFee;
                feeToAdd = Math.Max(tempFee, nextFee);
            }

            totalFee += feeToAdd;
            intervalStart = date;
        }

        return Math.Min(totalFee, 60);
    }

    private bool IsTollFreeVehicle(string vehicle)
    {
        return vehiclesTollfree.Contains(vehicle.ToLower());
    }

    public int GetTollFee(DateTime date, string vehicle, IEnumerable<TaxRateCard> rates)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
        {
            return 0;
        }

        return rates
            .FirstOrDefault(rate => IsEligibleforTollFee(date, rate))?
            .amount ?? 0;
    }

    private bool IsEligibleforTollFee(DateTime date, TaxRateCard taxRateCard)
    {
        var start = new TimeSpan(taxRateCard.startHour, taxRateCard.startMinute, 0);
        var end = new TimeSpan(taxRateCard.endHour, taxRateCard.endMinute, 0);
        var currentTime = date.TimeOfDay;
        return start <= currentTime && currentTime <= end;
    }

    private bool IsTollFreeDate(DateTime date)
    {
        if (date.IsWeekend())
        {
            return true;
        }

        if (date.Year != 2013)
        {
            return false;
        }

        return 
            date.IsHoliday() ||
            date.Month == 7;
    }

    private enum TollFreeVehicles
    {
        Motorcycle = 1,
        Tractor = 2,
        Emergency = 3,
        Diplomat = 4,
        Foreign = 5,
        Military = 6
    }
}
