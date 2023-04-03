using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TollCalculatorSana.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FeeController : ControllerBase
{
    private readonly IFeeService _feeService;

    public FeeController(IFeeService feeService)
    {
        _feeService = feeService;
    }


    // GET: api/<FeeController>
    [HttpGet]
    public IActionResult Get([FromQuery(Name = "Vehicle registration number")][Required()] string vehicleRegNum, [FromQuery(Name = "Vehicle type")][Required()] string vehicleType, [FromQuery(Name = "Dates to calculate fee")][Required()] List<DateTime> dates)
    {
        int totalFee = _feeService.GetTollFee(vehicleRegNum, vehicleType, dates);
        return new ObjectResult(totalFee);
    }
}
