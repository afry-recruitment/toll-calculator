using System;
using System.Linq;
using TollFeeCalculator;

public class TollCalculator
{
    /// <summary>
    /// Calculate the total toll fee for one day
    /// </summary>
    /// <param name="vehicle">the vehicle</param>
    /// <param name="dates">date and time of all passes on one day</param>
    /// <returns>the total toll fee for that day</returns>
    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        if (IsTollFreeVehicle(vehicle)) return 0;

        var totaltfee = 0;

        foreach (DateTime date in dates)
        {
            totaltfee += GetTollFee(date, vehicle);
        }
        return totaltfee;
    }

    /// <summary>
    /// the toll cost for a vehicle at a given time
    /// </summary>
    /// <param name="date">time of passing</param>
    /// <param name="vehicle">what vehicle</param>
    /// <returns>fee</returns>
    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        var loger = new Loger();
        loger.LogPassing(vehicle, date);

        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        using (var db = new TollFeeCalculator.utils.DataBaseContext())
        {
            var fee = GetFeeAtTime(date);

            var dbVehicle = db.Fees.First(x => x.Vehicle.LicensePlate == vehicle.LicensePlate);
            //Create new vehicle
            if (dbVehicle == null)
            {
                var newVechliceFee = new TollFeeCalculator.models.Fee { FeeAmount = fee, Vehicle = vehicle };
                newVechliceFee.FeeDay.Day = date;
                newVechliceFee.FeeDay.FeeAmount = fee;
                newVechliceFee.FeeHour.Time = date;
                newVechliceFee.FeeHour.FeeAmount = fee;
                db.Fees.Add(newVechliceFee);
            }
            else
            {
                if (dbVehicle.FeeDay.Day == date.Date)
                {
                    //Due to 60 being max amount add nothing
                    if (dbVehicle.FeeDay.FeeAmount == 60)
                    {
                        return 0;
                    }

                    var ts = date - dbVehicle.FeeHour.Time;
                    int feeToAdd;
                    if (ts.TotalHours == 0 && dbVehicle.FeeHour.FeeAmount > fee)
                    {
                        return 0;
                    }
                    else if (ts.TotalHours == 0)
                    {
                        feeToAdd = fee - dbVehicle.FeeHour.FeeAmount;
                        //If feeToAdd is less then 60 add full feeToAdd else add whats left
                        feeToAdd = dbVehicle.FeeAmount + feeToAdd < 60 ? feeToAdd : 60 - dbVehicle.FeeAmount;
                    }
                    else
                    {
                        dbVehicle.FeeHour.Time = date;
                        //If fee is less then 60 add full fee else add whats left
                        feeToAdd = dbVehicle.FeeAmount + fee < 60 ? fee : 60 - dbVehicle.FeeAmount;
                    }
                    dbVehicle.FeeAmount += feeToAdd;
                    dbVehicle.FeeHour.FeeAmount = fee;
                    db.SaveChanges();
                    return feeToAdd;
                }
                dbVehicle.FeeHour.Time = date;
                dbVehicle.FeeHour.FeeAmount = fee;
                dbVehicle.FeeAmount = fee;
            }

            db.SaveChanges();
            return fee;
        }
    }

    /// <summary>
    /// give fee for time
    /// </summary>
    /// <param name="date">time of passage</param>
    /// <returns>fee amount</returns>
    private static int GetFeeAtTime(DateTime date)
    {
        var hour = date.Hour;
        var minute = date.Minute;

        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if ((hour == 15 && minute >= 0) || (hour == 16 && minute <= 59)) return 18;
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        else return 0;
    }

    /// <summary>
    /// Checks of day is TollFree
    /// </summary>
    /// <param name="date">Date of passage</param>
    /// <returns>bool if is toll free or not</returns>
    private static bool IsTollFreeDate(DateTime date) => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || ApiHelper.GetPublicHoliday(date);


    /// <summary>
    /// checks if Vehicle is tollFree
    /// </summary>
    /// <param name="vehicle">the Vehicle</param>
    /// <returns>bool if free</returns>
    private static bool IsTollFreeVehicle(IVehicle vehicle)
    {
        if (vehicle == null) return false;
        var vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(nameof(TollFreeVehicles.Motorbike)) ||
               vehicleType.Equals(nameof(TollFreeVehicles.Tractor)) ||
               vehicleType.Equals(nameof(TollFreeVehicles.Emergency)) ||
               vehicleType.Equals(nameof(TollFreeVehicles.Diplomat)) ||
               vehicleType.Equals(nameof(TollFreeVehicles.Foreign)) ||
               vehicleType.Equals(nameof(TollFreeVehicles.Military));
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