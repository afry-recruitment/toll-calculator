using TollFeeCalculator.Application.Interfaces;
using TollFeeCalculator.Enterprise.Entities;
using TollFeeCalculator.Enterprise.Extensions;

namespace TollFeeCalculator.Application.Services
{
    public class VehicleService : IVehicleService
    {
        public bool IsTollFree(Vehicle vehicle)
        {
            if (vehicle is null)
            {
                return false;
            }

            return vehicle.Type.IsTollFree();
        }
    }
}
