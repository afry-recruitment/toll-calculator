using System;
using System.Collections.Generic;

namespace TollFeeCalculator.Models
{
    public class TollStationPassage : IModel
    {
        public static List<TollStationPassage> AllTollStationPassages { get; private set; } = new();
        public int Id { get; init; } // Autoincrement in db.
        public TollStation? TollStation { get; private set; } // Allow null for testing.
        public Vehicle Vehicle { get; private set; }
        public DateTime Time { get; private set; } = DateTime.Now; // Default set to current time.
        public int TollFee { get; private set; }

        public TollStationPassage(TollStation tollStation, Vehicle vehicle)
        {
            this.TollStation = tollStation;
            this.Vehicle = vehicle;
            TollFee = vehicle.IsTollFree ? 0 : GetTollFee();
            AllTollStationPassages.Add(this);
        }

        public TollStationPassage(TollStation tollStation, Vehicle vehicle, DateTime time) // Used for testing with manual time of passage.
        {
            this.TollStation = tollStation;
            this.Vehicle = vehicle;
            this.Time = time;
            TollFee = vehicle.IsTollFree ? 0 : GetTollFee();
            AllTollStationPassages.Add(this);
        }

        public TollStationPassage(Vehicle vehicle, DateTime time) // Used for testing with manual time of passage.
        {
            this.Vehicle = vehicle;
            this.Time = time;
            TollFee = vehicle.IsTollFree ? 0 : GetTollFee();
            AllTollStationPassages.Add(this);
        }

        public void ModifyTollFeeForPassage(int newTollFee)
        {
            this.TollFee = newTollFee;
        }

        public int GetTollFee()
        {
            if (Time.DayOfWeek == DayOfWeek.Saturday || Time.DayOfWeek == DayOfWeek.Sunday || TollFreeDate.InList(TollFreeDate.AllTollFreeDates, Time))
            {
                return 0;
            }
            else if (FeeTimeInterval.InInterval(FeeTimeInterval.FeeLevel1Intervals, Time))
            {
                return (int)Utilities.TollFee.FeeLevel1;
            }
            else if (FeeTimeInterval.InInterval(FeeTimeInterval.FeeLevel2Intervals, Time))
            {
                return (int)Utilities.TollFee.FeeLevel2;
            }
            else if (FeeTimeInterval.InInterval(FeeTimeInterval.FeeLevel3Intervals, Time))
            {
                return (int)Utilities.TollFee.FeeLevel3;
            }
            else { return 0; }
        }
    }
}