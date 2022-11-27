using TollFeeCalculator.Models;

namespace TollFeeCalculator
{
    public class Repository
    {
        
        private readonly List<VehicleType> _tollFreeVehiclesList = new List<VehicleType>()
        {
            VehicleType.Motorbike,
            VehicleType.Tractor,
            VehicleType.Emergency,
            VehicleType.Diplomat,
            VehicleType.Foreign,
            VehicleType.Military
        };

        private readonly List<DateTime> _tollFreeDatesList = new List<DateTime>()
        {
            new DateTime(2013, 1, 1),
            new DateTime(2013, 3, 28),
            new DateTime(2013, 3, 29),
            new DateTime(2013, 4, 1),
            new DateTime(2013, 4, 30),
            new DateTime(2013, 5, 1),
            new DateTime(2013, 5, 8),
            new DateTime(2013, 5, 9),
            new DateTime(2013, 6, 5),
            new DateTime(2013, 6, 6),
            new DateTime(2013, 6, 21),
            new DateTime(2013, 11, 1),
            new DateTime(2013, 12, 24),
            new DateTime(2013, 12, 25),
            new DateTime(2013, 12, 26),
            new DateTime(2013, 12, 31)
        };

        private readonly Dictionary<TollFeeTimeRange, int> _tollFeeTimeRangeDict = new Dictionary<TollFeeTimeRange, int>()
        {
            { new TollFeeTimeRange(){
                StartTime = new TimeOnly(0, 0, 0),
                EndTime = new TimeOnly(5, 59, 0)
                },
                0
            },
            { new TollFeeTimeRange(){
                StartTime = new TimeOnly(6, 0, 0),
                EndTime = new TimeOnly(6, 29, 0)
                },
                8
            },
            { new TollFeeTimeRange(){
                StartTime = new TimeOnly(6, 30, 0),
                EndTime = new TimeOnly(6, 59, 0)
                },
                13
            },
            { new TollFeeTimeRange(){
                StartTime = new TimeOnly(7, 0, 0),
                EndTime = new TimeOnly(7, 59, 0)
                },
                18
            },
            { new TollFeeTimeRange(){
                StartTime = new TimeOnly(8, 0, 0),
                EndTime = new TimeOnly(8, 29, 0)
                },
                13
            },
            { new TollFeeTimeRange(){
                StartTime = new TimeOnly(8, 30, 0),
                EndTime = new TimeOnly(14, 59, 0)
                },
                8
            },
            { new TollFeeTimeRange(){
                StartTime = new TimeOnly(15, 0, 0),
                EndTime = new TimeOnly(15, 29, 0)
                },
                13
            },
            { new TollFeeTimeRange(){
                StartTime = new TimeOnly(15, 30, 0),
                EndTime = new TimeOnly(16, 59, 0)
                },
                18
            },
            { new TollFeeTimeRange(){
                StartTime = new TimeOnly(17, 0, 0),
                EndTime = new TimeOnly(17, 59, 0)
                },
                13
            },
            { new TollFeeTimeRange(){
                StartTime = new TimeOnly(18, 0, 0),
                EndTime = new TimeOnly(18, 29, 0)
                },
                8
            },
            { new TollFeeTimeRange(){
                StartTime = new TimeOnly(18, 30, 0),
                EndTime = new TimeOnly(0, 0, 0)
                },
                0
            },
        };
        public bool IsTollFreeVehicle(VehicleType vehicleType)
        {
            return _tollFreeVehiclesList.Contains(vehicleType);
        }

        public bool IsTollFreeDate(DateTime date)
        {
            return _tollFreeDatesList.Any(x => x.Year == date.Year && x.Month == date.Month && x.Day == date.Day);
        }

        public int GetTollFeeForTime(TimeOnly time)
        {
            return _tollFeeTimeRangeDict.Where(x => x.Key.StartTime <= time && x.Key.EndTime >= time).FirstOrDefault().Value;
        }
    }
}
