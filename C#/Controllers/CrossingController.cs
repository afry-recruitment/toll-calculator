using Microsoft.AspNetCore.Mvc;
using TollFeeCalculator.Interfaces;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrossingController : Controller
    {
        private readonly ICrossingRepository _crossingRepository;
        private readonly CarService _carService;

        public CrossingController(ICrossingRepository crossingRepository, CarService carService)
        {
            _crossingRepository = crossingRepository;
            _carService = carService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Crossing>))]
        public IActionResult GetCrossings()
        {
            var crossings = List<Crossing>(_crossingRepository.GetCrossings());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(crossings);
        }

        [HttpGet("{crossingId}")]
        [ProducesResponseType(200, Type = typeof(Crossing))]
        [ProducesResponseType(400)]
        public IActionResult GetCrossing(int crossingId)
        {
            if (!_crossingRepository.CrossingExists(crossingId))
                return NotFound();

            var crossing = _crossingRepository.GetCrossing(crossingId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(crossing);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrUpdateCrossing([FromBody] Vehicle vehicle)
        {
            //Validate
            if (vehicle == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            //Check if vehicle exists in database
            var crossing = _crossingRepository.GetCrossing(vehicle.RegistrationNumber);

            if (crossing != null)
            {
                if(crossing.CrossingDate.Date == new Date().date)
                {
                    //Add a new crossing to the existing day crossing
                    crossing.HourlyCrossings.Add(new Date());
                    crossing.TotalFeeDay = GetTollFee(crossing.HourlyCrossings, crossing.Vehicle)

                    //Update and exit function
                    if(_crossingRepository.UpdateCrossing(crossing))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                    return Ok("Successfully updated");
                }
            }
            //Create a new crossing

            //Check if car is registrered in the system and is legal. (Mock placeholder for external api call)
            if (!_carService.VehicleExistsAsync(vehicle.id))
            {
                ModelState.AddModelError("", "Vehicle does not exist in legal system");
                return StatusCode(500, ModelState);
            }

            Crossing crossing = new Crossing();
            crossing.CrossingDates.Add(new Date());
            crossing.fee = GetTollFee(CrossingDates[0], vehicle);
            crossing.Vehicle = vehicle;

            if (!_crossingRepository.CreateCrossing(crossing))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpDelete("{crossingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCrossing(int crossingId)
        {
            if (!_crossingRepository.CrossingExists(crossingId))
            {
                return NotFound();
            }

            var crossingToDelete = _crossingRepository.GetCrossing(crossingId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_crossingRepository.DeleteCrossing(crossingToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting crossing");
            }

            return NoContent();
        }
    }
}