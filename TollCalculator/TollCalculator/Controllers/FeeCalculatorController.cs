using Microsoft.AspNetCore.Mvc;
using TollFeeCalculator.Models.Request;
using TollFeeCalculator.Services;

namespace TollFeeCalculator.Controllers;

/// <summary>
/// API for toll calculation
/// </summary>
[ApiController]
[Route("[controller]")]
public class FeeCalculatorController(ILogger<FeeCalculatorController> logger, ITollService tollService) : ControllerBase
{
    private readonly ILogger<FeeCalculatorController> _logger = logger;
    private readonly ITollService _tollService = tollService;

    /// <summary>
    /// Calculate toll fee for vehicle and passage times 
    /// </summary>
    /// <param name="tollrequest">Tollrequest containing current vehicle and passage times</param>
    /// <returns>Returns the calculated toll fee for current vehicle</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /FeeCalculator
    ///     {
    ///        "vehicle":  {
    ///             "vehicleType" : 6 // Car
    ///         },
    ///        "passageTimes": ["2025-02-28 08:33", "2025-02-28 17:33"],
    ///     }
    ///     
    /// </remarks>
    /// <response code="200">Returns the calculated toll fee for current vehicle</response>
    /// <response code="400">Returns bad request if missing body values passagetimes or vehicle</response>
    /// <response code="500">Returned if error occured</response>
    [HttpPost("Calculate")]
    public IActionResult Get(TollRequest tollrequest)
    {
        if(!tollrequest.PassageTimes.Any())
        {
            return BadRequest($"Missing valid {nameof(tollrequest.PassageTimes)}");
        }

        try
        {
            return Ok(_tollService.GetTollFee(tollrequest.Vehicle, tollrequest.PassageTimes));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, $"{nameof(FeeCalculatorController)}: Error while fetching toll fees");
            return StatusCode(500);
        }

    }
}
