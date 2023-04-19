using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TollCalculator.Application.Features.Queries.GetTollFee;
using TollCalculator.Domain.DTOs;

namespace TollCalculator.Controllers
{
    [Route("api/tollfee")]
    public class TollFeeController : BaseController
    {
        [HttpPost("getfeebyvehicle")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> GetTollFeeController(GetFeeByVehicleDTO getFeeDTO, CancellationToken cancellationToken)
        {
            var query = new GetTollFeeQuery(getFeeDTO);
            return Ok(await Mediator.Send(query, cancellationToken));
        }
    }
}