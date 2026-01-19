using Microsoft.AspNetCore.Mvc;
using TollFeeCalculator.Application.Interfaces;
using TollFeeCalculator.Presentation.Models;

namespace TollFeeCalculator.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TollFeeCalculatorController : ControllerBase
    {
        private readonly ILogger<TollFeeCalculatorController> _logger;
        private readonly ITollFeeService _tollFeeService;

        public TollFeeCalculatorController(ILogger<TollFeeCalculatorController> logger, ITollFeeService tollFeeService)
        {
            _logger = logger;
            _tollFeeService = tollFeeService;
        }

        [HttpPost(nameof(GetFee))]
        public async Task<IActionResult> GetFee([FromBody] GetTollFeeRequestModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _tollFeeService.GetFee(model.DateTimes, model.Vehicle, cancellationToken).ConfigureAwait(false);
                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return new StatusCodeResult(StatusCodes.Status499ClientClosedRequest);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Invalid request data");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate toll fee");
                return Problem(detail: "Internal server error");
            }
        }
    }
}
