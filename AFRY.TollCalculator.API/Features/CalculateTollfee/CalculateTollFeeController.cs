using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFRY.TollCalculator.API.Features.CalculateTollfee;

[Route("api/[controller]")]
[ApiController]
public class CalculateTollFeeController(ICalculateTollFeeService calculateTollFeeService, 
    ILogger<CalculateTollFeeController> logger) : ControllerBase
{
    private readonly ICalculateTollFeeService _calculateTollFeeService = calculateTollFeeService;
    private readonly ILogger<CalculateTollFeeController> _logger = logger;

    [HttpPost]
    public IActionResult CalculateTollFee([FromBody] CalculateTollFeeDto request)
    {
        if (request.Vehicle == null || request.Dates == null || request.Dates.Length == 0)
        {
            _logger.LogWarning("Invalid input: Vehicle or Dates is missing.");
            return BadRequest("Invalid input. Please provide a valid vehicle and dates.");
        }

        try
        {
            var tollFee = _calculateTollFeeService.GetTotalTollFeeForMultipleDays(request.Vehicle, request.Dates);
            _logger.LogInformation("Toll fee calculated successfully: {TollFee}", tollFee);

            return Ok(new { TotalTollFee = tollFee });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while calculating toll fee");

            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }
}
