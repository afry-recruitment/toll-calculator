using TollFeeCalculator.Enterprise.Entities;

namespace TollFeeCalculator.Application.Interfaces
{
    public interface IVehicleService
    {
        public bool IsTollFree(Vehicle vehicle);
    }
}
