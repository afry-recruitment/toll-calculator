using TollCalculator.Domain.DTOs;

namespace Application.Interfaces.GetTollFee;

public interface IGetTollFeeRepository
{
    Task<string> GetTollFee(GetFeeByVehicleDTO getFeeDTO, CancellationToken cancellationToken);
}
