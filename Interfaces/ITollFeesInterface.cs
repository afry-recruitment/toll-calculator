using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficTollCalculator.Models;

namespace TrafficTollCalculator.Interfaces
{
    internal interface ITollFeesInterface
    {
        bool IsTollFreeVehicle(string vehicleType);
        bool IsTollFreeDate(DateTime date);
        int GetApplicableFee(TimeSpan timeOfDay);
        int GetTollFee(List<List<DateTime>> timestampsPerVehicle, List<Vehicle> vehicles);
    }
}
