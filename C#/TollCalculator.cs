namespace TollFeeCalculator
{
    public static class TollCalculator
    {
        private const int lowFee = 8;
        private const int mediumFee = 13;
        private const int highFee = 18;

        // The fee is the first one not yet reached
        private static readonly TollFee[] fees = [
            new(06, 00, 0),
            new(06, 30, lowFee),
            new(07, 00, mediumFee),
            new(08, 00, highFee),
            new(08, 30, mediumFee),
            new(15, 00, lowFee),
            new(15, 30, mediumFee),
            new(17, 00, highFee),
            new(18, 00, mediumFee),
            new(18, 30, lowFee),
        ];


        /// <summary>
        /// Calculates the toll for a vehicle during one day
        /// </summary>
        /// <param name="vehicle">The vehicle to calculate the toll for</param>
        /// <param name="times">A sorted array of timestamps from the same day</param>
        /// <returns>The total fee incurred by the vehicle</returns>
        /// <exception cref="ArgumentException">Thrown if there are no times, or they are from different days, not in ascending order or duplicates.</exception>
        public static int GetTollFee(IVehicle vehicle, DateTime[] times)
        {
            ArgumentNullException.ThrowIfNull(vehicle);
            ArgumentNullException.ThrowIfNull(times);
            if (times.Length == 0)
                throw new ArgumentException("No times supplied", nameof(times));

            DateTime prev = times[0];
            for (int i = 1; i < times.Length; i++)
            {
                DateTime date = times[i];
                if (date.Year != prev.Year || date.Month != prev.Month || date.Day != prev.Day)
                    throw new ArgumentException("All times must be from the same day", nameof(times));
                else if (prev > date)
                    throw new ArgumentException("All times must be in ascending order", nameof(times));
                else if (prev == date)
                    throw new ArgumentException("Duplicate times found", nameof(times));
                prev = date;
            }

            if (vehicle.IsTollFree() || IsTollFreeDate(times[0]))
                return 0;

            // Calculate the total fee for the day using the ExtendingOnHigherFee variant (see comment further down for more info)
            return GetTotalFee(times, 60, TimeFrameVariant.ExtendingOnHigherFee);
        }
                
        private static bool IsTollFreeDate(DateTime date) => 
            date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday || HolidayHelper.IsSwedishBankHoliday(date);

        /*  From the requirements:
              A vehicle should only be charged once an hour
                  - In the case of multiple fees in the same hour period, the highest one applies.

            However, this can be interpreted in many different ways.

            To visualize this let's consider a vehicle incurring a fee every 40 minutes throughout the day
            and let's imagine the individual tolls would be the following:
            A: 8
            B: 8
            C: 13
            D: 18
            E: 13
            F: 8

            If we let the time frame extend (as in the TimeFrameVariant.Extending implementation) since only
            40 minutes has passed since the last fee, all fees would be considered to be within the same hour.
            This would result in the vehicle only being charged for D for a total of 18.

            If we instead set a fixed time frame (as in the TimeFrameVariant.Fixed implementation) the vehicle
            would be charged for multiple passings. Once the vehicle reaches C more than an hour has passed since
            A so a new fee is incurred. However since there's only 40 minutes to D that higher fee is used for
            this hourly period. The vehicle then incurrs a fee for E as well since more than an hour has passed
            since C where the period started. The vehicle will therefore be charged for A, D and E for a total
            of 39.

            The first approach lets the vehicle skip payment by merging all instances into a single "hour" but
            the second approach technically charged it for two tolls only 40 minutes apart. We can address this
            with another variant (as in the TimeFrameVariant.ExtendingOnHigherFee implementation) where we only
            consider the time since the actual charge. This would incurr payments at A, D and F for a total of
            34.

            While this third approach looks good given this example, it's possible to think up a scenario where
            the resulting fee doesn't seem to match "what one would expect". For instance, if the vehicle
            never appeared at A, we would only charge for D and F. This means B would be skipped despite being 
            80 minutes before the first actual charge. However, charging for B would go against the requirement
            since C is within the hour with a higher fee, but we can't charge for C since it's within 40 minutes
            of D.

            In conclusion, the requirement of "A vehicle should only be charged once an hour" is rather unclear.
        */
        private static int GetTotalFee(DateTime[] times, int max, TimeFrameVariant variant)
        {
            int total = 0;
            int prevFee = 0;
            DateTime? prevTime = null;

            foreach (DateTime time in times)
            {
                int fee = GetTollFee(time);
                if (fee == 0)
                    continue;

                // A new hour has started
                if (!prevTime.HasValue || (time - prevTime.Value).TotalHours > 1)
                {
                    total += fee;
                    prevFee = fee;
                    prevTime = time;
                }
                // Same hour as previous fee, and it should be replaced
                else if (fee > prevFee)
                {
                    total += fee - prevFee;
                    prevFee = fee;
                    if (variant != TimeFrameVariant.Fixed)
                        prevTime = time;
                }
                // Same hour as previous fee, but the previous fee was higher
                else if (variant == TimeFrameVariant.Extending)
                    prevTime = time;

                // No need to continue if the max is reached
                if (total >= max)
                    return max;
            }

            return total;
        }

        private static int GetTollFee(DateTime time)
        {
            foreach (TollFee fee in fees)
                if (fee.TryGetFee(time, out int v))
                    return v;

            return 0;
        }

        private enum TimeFrameVariant
        {
            Fixed,
            Extending,
            ExtendingOnHigherFee,
        }
    }
}