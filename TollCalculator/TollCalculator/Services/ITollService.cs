using TollFeeCalculator.Models;

namespace TollFeeCalculator.Services
{
    /// <summary>
    /// Toll service to handle and calculate toll fees
    /// </summary>
    public interface ITollService
    {
        /// <summary>
        /// Check if current vehicle is of toll free type
        /// </summary>
        bool IsTollFreeVehicle(IVehicle vehicle);

        /// <summary>
        /// Check if current vehicle or date is toll free
        /// </summary>
        Task<bool> IsTollFree(IVehicle vehicle, DateTime date);

        /// <summary>
        /// Get current toll for vehicle and date
        /// </summary>
        Task<int> GetTollFee(IVehicle vehicle, DateTime date);

        /// <summary>
        /// Get current toll for vehicle and dates
        /// </summary>
        int GetTollFee(IVehicle vehicle, IEnumerable<DateTime> passageTimes);
    }
}
