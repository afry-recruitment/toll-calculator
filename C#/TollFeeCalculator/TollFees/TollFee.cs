using System;
using System.Collections.Generic;
using System.Linq;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.TollFees
{
    public class TollFee
    {
        public static int GetTollFeeForVehicle(IVehicle vehicle, IList<DateTime> dates, ITollFeeRepository tollFeeRepository)
        {
            if(vehicle is null)
            {
                throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null");
            }

            if(tollFeeRepository is null)
            {
                throw new ArgumentNullException(nameof(tollFeeRepository), "TollRepository cannot be null!");
            }

            if(dates.Count == 0)
            {
                return 0;
            }

            var currentDay = dates.First();
            var intervalStart = currentDay;
            var currentMaxIntervalFee = GetTollFeeForTimeOfDay(intervalStart, vehicle, tollFeeRepository);
            var currentDayTotalFee = currentMaxIntervalFee;
            var totalFee = 0;

            for(var tripNo = 1; tripNo < dates.Count; tripNo++)
            {
                var date = dates[tripNo];
                if(currentDay.Date != date.Date)
                {
                    if(currentDayTotalFee > tollFeeRepository.MaxTollFeePerDay)
                    {
                        currentDayTotalFee = tollFeeRepository.MaxTollFeePerDay;
                    }

                    totalFee += currentDayTotalFee;
                    currentDayTotalFee = 0;
                    currentDay = date;
                }
                var timeDiff = date - intervalStart;

                var nextFee = GetTollFeeForTimeOfDay(date, vehicle, tollFeeRepository);

                if(timeDiff < tollFeeRepository.CombineTollFeeTimeSpan && nextFee > currentMaxIntervalFee)
                {
                    currentDayTotalFee -= currentMaxIntervalFee;
                    currentMaxIntervalFee = nextFee;
                    currentDayTotalFee += nextFee;
                }

                if(timeDiff >= tollFeeRepository.CombineTollFeeTimeSpan)
                {
                    currentDayTotalFee += nextFee;
                    currentMaxIntervalFee = nextFee;
                    intervalStart = date;
                }
            }

            if(currentDayTotalFee > tollFeeRepository.MaxTollFeePerDay)
            {
                currentDayTotalFee = tollFeeRepository.MaxTollFeePerDay;
            }

            totalFee += currentDayTotalFee;

            return totalFee;
        }

        public static int GetTollFeeForTimeOfDay(DateTime date, IVehicle vehicle, ITollFeeRepository tollFeeRepository)
        {
            if(vehicle is null)
            {
                throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null");
            }

            if(tollFeeRepository is null)
            {
                throw new ArgumentNullException(nameof(tollFeeRepository), "TollRepository cannot be null!");
            }

            if(IsDateTollFeeFree(date, tollFeeRepository) || IsVehicleTollFeeFree(vehicle, tollFeeRepository))
            {
                return 0;
            }

            var tollFee = tollFeeRepository.TollFeesForTimeIntervals
                .Where(x => x.IsInInterval(date))
                .Select(x => x.TollFee)
                .FirstOrDefault();

            return tollFee;
        }

        public static bool IsDateTollFeeFree(DateTime date, ITollFeeRepository tollFeeRepository)
        {
            if(tollFeeRepository is null)
            {
                throw new ArgumentNullException(nameof(tollFeeRepository), "TollRepository cannot be null!");
            }

            var tollFeeFree =
                tollFeeRepository.TollFeeFreeDates.Contains(DateOnly.FromDateTime(date)) ||
                tollFeeRepository.TollFeeFreeDaysOfWeek.Contains(date.DayOfWeek);
            return tollFeeFree;
        }

        public static bool IsVehicleTollFeeFree(IVehicle vehicle, ITollFeeRepository tollFeeRepository)
        {
            if(vehicle is null)
            {
                throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null");
            }

            if(tollFeeRepository is null)
            {
                throw new ArgumentNullException(nameof(tollFeeRepository), "TollRepository cannot be null!");
            }

            var tollFeeFree =
                tollFeeRepository.TollFeeFreeVehicleClassifications.Contains(vehicle.VehicleClassification) ||
                tollFeeRepository.TollFeeFreeVehicleTypes.Contains(vehicle.VehicleType);
            return tollFeeFree;
        }

    }
}
