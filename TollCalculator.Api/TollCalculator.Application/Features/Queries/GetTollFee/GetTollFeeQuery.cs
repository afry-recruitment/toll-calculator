using TollCalculator.Domain.DTOs;

namespace TollCalculator.Application.Features.Queries.GetTollFee;

public class GetTollFeeQuery : IRequest<string>
{
    public GetFeeByVehicleDTO GetFeeDTO { get; set; }
    public GetTollFeeQuery(GetFeeByVehicleDTO getFeeDTO)
    {
        GetFeeDTO = getFeeDTO;
    }
}
