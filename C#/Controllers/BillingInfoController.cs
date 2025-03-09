using Microsoft.AspNetCore.Mvc;
using TollFeeCalculator.Interfaces;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrossingController : Controller
    {
        private readonly IBillingInfoRepository _billingInfoRepository;
        private readonly ICrossingRepository _crossingRepository;
        private 

        public CrossingController(IBillingInfoRepository billingInfoRepository, ICrossingRepository crossingRepository)
        {
            _billingInfoRepository = billingInfoRepository;
            _crossingRepository = crossingRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BillingInfo>))]
        public IActionResult GetBillingInfos()
        {
            var billings = List<BillingInfo>(_billingInfoRepository.GetBillingInfos());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(billings);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateMontlyBillingInfo([FromBody] Vehicle vehicle)
        {
            //Validate
            if (vehicle == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            //Check if vehicle exists in database
            var crossing = _crossingRepository.GetCrossings(vehicle.RegistrationNumber);

            if (crossing != null)
            {

                //Get all crossings during a month
                List<Crossing> crossingMonth = _crossingRepository.GetCrossingsMonth(vehicle.RegistrationNumber, new Date())
                    BillingInfo billingInfo = new BillingInfo
                    {
                        VehicleId = vehicle.Id, // Foreign key for Vehicle
                        TotalCostMonth = crossingMonth.Sum(c => c.TotalFeeDay),
                        TotalNumberOfCrossings = crossingMonth.Count,
                        IsPaid = false // Standard unpaid
                    };

                if (!IBillingInfoRepository.CreateBillingInfo(billingInfo))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                //TODO: Generate invoice and send to owner, using ownerID
                return Ok("Successfully created");


            }
        }

        //TODO: Expand with more CRUD methods.

    }
}