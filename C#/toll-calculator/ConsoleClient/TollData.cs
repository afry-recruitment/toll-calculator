
using ConsoleClient.Interfaces;

namespace ConsoleClient
{
    public class TollData
    {
        public IVehicle Vehicle { get; }
        public DateTime DroveThruDate { get; }
        public int DroveThruCount { get; }

        public TollData(
            IVehicle vehicle,
            DateTime droveThruDate,
            int droveThruCount)
        {
            Vehicle = vehicle;
            DroveThruDate = droveThruDate;
            DroveThruCount = droveThruCount;
        }

        public TollData UpdateTollData(DateTime droveThruDate, int droveThruCount) =>
            new TollData(
                Vehicle,
                droveThruDate,
                droveThruCount);
        

    }
}
