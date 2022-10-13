using System;
using System.Collections.Generic;

namespace TollFeeCalculator.Models
{

    public class FeeTimeInterval : IModel
    {
        private static readonly List<FeeTimeInterval> AllIntervals = new();
        public static List<FeeTimeInterval> NoFeeIntervals { get; private set; } = new();
        public static List<FeeTimeInterval> FeeLevel1Intervals { get; private set; } = new();
        public static List<FeeTimeInterval> FeeLevel2Intervals { get; private set; } = new();
        public static List<FeeTimeInterval> FeeLevel3Intervals { get; private set; } = new();

        public int Id { get; init; }
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;

        public FeeTimeInterval(DateTime startTime, DateTime endTime)
        {
            this._startTime = startTime;
            this._endTime = endTime;
            AllIntervals.Add(this);
            AddToFeeIntervalsList(this);
        }


        public static bool InInterval(List<FeeTimeInterval> intervals, DateTime time)
        {
            foreach (var interval in intervals)
            {
                if (time.TimeOfDay >= interval._startTime.TimeOfDay && time.TimeOfDay < interval._endTime.TimeOfDay)
                {
                    return true;
                }
            }
            return false;
        }

        private static void AddToFeeIntervalsList(FeeTimeInterval interval)
        {
            int startHour = interval._startTime.Hour;
            int startMin = interval._startTime.Minute;


            // Mer överskådlig if-sats.
            if (startHour < 6 && startHour > 18)
            {
                NoFeeIntervals.Add(interval);
            }
            else
            {
                if (startHour == 6)
                {
                    if (startMin == 0)
                    {
                        FeeLevel1Intervals.Add(interval);
                    }
                    else if (startMin == 30)
                    {
                        FeeLevel2Intervals.Add(interval);
                    }
                }
                else if (startHour == 7)
                {
                    FeeLevel3Intervals.Add(interval);
                }
                else if (startHour == 8)
                {
                    if (startMin == 0)
                    {
                        FeeLevel2Intervals.Add(interval);
                    }
                    else if (startMin == 30)
                    {
                        FeeLevel1Intervals.Add(interval);
                    }
                }
                else if (startHour == 9)
                {
                    FeeLevel1Intervals.Add(interval);
                }
                else if (startHour == 10)
                {
                    FeeLevel1Intervals.Add(interval);
                }
                else if (startHour == 11)
                {
                    FeeLevel1Intervals.Add(interval);
                }
                else if (startHour == 12)
                {
                    FeeLevel1Intervals.Add(interval);
                }
                else if (startHour == 13)
                {
                    FeeLevel1Intervals.Add(interval);
                }
                else if (startHour == 14)
                {
                    FeeLevel1Intervals.Add(interval);
                }
                else if (startHour == 15)
                {
                    if (startMin == 0)
                    {
                        FeeLevel2Intervals.Add(interval);
                    }
                    else if (startMin == 30)
                    {
                        FeeLevel3Intervals.Add(interval);
                    }
                }
                else if (startHour == 16)
                {
                    FeeLevel3Intervals.Add(interval);
                }
                else if (startHour == 17)
                {
                    FeeLevel2Intervals.Add(interval);
                }
                else if (startHour == 18)
                {
                    if (startMin == 0)
                    {
                        FeeLevel1Intervals.Add(interval);
                    }
                    else if (startMin == 30)
                    {
                        NoFeeIntervals.Add(interval);
                    }
                }
            }

            #region Alternative solution - Switch
            // Alt Switch även detta mer överskådligt
            //switch (startHour)
            //{
            //    case 0:
            //    case 1:
            //    case 2:
            //    case 3:
            //    case 4:
            //    case 5:
            //        NoFeeIntervals.Add(interval);
            //        break;
            //    case 6:
            //        if (startMin == 0)
            //        {
            //            FeeLevel1Intervals.Add(interval);
            //            break;
            //        }
            //        FeeLevel2Intervals.Add(interval);
            //        break;
            //    case 7:
            //        FeeLevel3Intervals.Add(interval);
            //        break;
            //    case 8:
            //        if (startMin == 0)
            //        {
            //            FeeLevel2Intervals.Add(interval);
            //            break;
            //        }
            //        FeeLevel1Intervals.Add(interval);
            //        break;
            //    case 9:
            //    case 10:
            //    case 11:
            //    case 12:
            //    case 13:
            //    case 14:
            //        FeeLevel1Intervals.Add(interval);
            //        break;
            //    case 15:
            //        if (startMin == 0)
            //        {
            //            FeeLevel2Intervals.Add(interval);
            //            break;
            //        }
            //        FeeLevel3Intervals.Add(interval);
            //        break;
            //    case 16:
            //        FeeLevel3Intervals.Add(interval);
            //        break;
            //    case 17:
            //        FeeLevel2Intervals.Add(interval);
            //        break;
            //    case 18:
            //        if(startMin == 0)
            //        {
            //            FeeLevel3Intervals.Add(interval);
            //            break;
            //        }
            //        NoFeeIntervals.Add(interval);
            //        break;
            //    case 19:
            //    case 20:
            //    case 21:
            //    case 22:
            //    case 23:
            //        NoFeeIntervals.Add(interval);
            //        break;
            //    default:
            //        NoFeeIntervals.Add(interval);
            //        break;
            //}
            #endregion

            #region Alternative solution - Compressed if
            //if ((startHour < 6 && startHour > 18) || (startHour == 18 && startMin == 30))
            //{
            //    NoFeeIntervals.Add(interval);
            //}
            //else
            //{
            //    if (startHour == 18 ||
            //       (startHour == 6 && startMin == 0) ||
            //       (startHour == 8 && startMin == 0) ||
            //       (startHour >= 9 && startHour <= 14))
            //    {
            //        FeeLevel1Intervals.Add(interval);
            //    }
            //    else if ((startHour == 6 && startMin == 30) ||
            //             (startHour == 8 && startMin == 0) ||
            //             (startHour == 15 && startMin == 0) ||
            //              startHour == 17)
            //    {
            //        FeeLevel2Intervals.Add(interval);
            //    }
            //    else
            //    {
            //        FeeLevel3Intervals.Add(interval);
            //    }
            //}
            #endregion
        }

        // If intervals are modified this will updated all lists.
        private static void UpdateAllIntervalLists()
        {
            // Clear all lists.
            NoFeeIntervals.Clear();
            FeeLevel1Intervals.Clear();
            FeeLevel2Intervals.Clear();
            FeeLevel3Intervals.Clear();

            foreach (var item in AllIntervals)
            {
                AddToFeeIntervalsList(item);
            }
        }

        // Used for setting up 30 mins intervals for testing.
        public static void CreateFeeTimeIntervals() // Used for testing purposes.
        {
            for (int i = 0; i < 24; i++)
            {
                _ = new FeeTimeInterval(new DateTime(1, 1, 1, i, 00, 00), new DateTime(1, 1, 1, i, 30, 00));
                if (i < 23)
                {
                    _ = new FeeTimeInterval(new DateTime(1, 1, 1, i, 30, 00), new DateTime(1, 1, 1, i + 1, 00, 00));
                }
                else
                {
                    _ = new FeeTimeInterval(new DateTime(1, 1, 1, i, 30, 00), new DateTime(1, 1, 1, 0, 00, 00));
                }
            }
        }
    }
}