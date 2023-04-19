using Application.Interfaces.GetTollFee;

namespace TollCalculator.Application.Features.Queries.GetTollFee;

public class GetTollFeeQueryHandler : IRequestHandler<GetTollFeeQuery, string>
{
    private readonly IGetTollFeeRepository _getTollFeeRepository;

    public GetTollFeeQueryHandler(IGetTollFeeRepository tollFeeRepository)
    {
        _getTollFeeRepository = tollFeeRepository;
    }
    public async Task<string> Handle(GetTollFeeQuery request, CancellationToken cancellationToken)
    {

        var result = await _getTollFeeRepository.GetTollFee(request.GetFeeDTO, cancellationToken);

        return result;
    }
}
