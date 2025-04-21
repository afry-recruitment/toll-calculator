using AfryToll.Model.Dtos;
using AfryTollApi.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using AfryToll.Model.Dtos;
using AfryTollApi.Entities;
using AfryTollApi.Extention;
using AfryTollApi.Repositories;
using AfryTollApi.Repositories.Contracts;

namespace AfryTollApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TollController : ControllerBase
    {
        private readonly ITollRepository TollRepository;
        private readonly IUserRepository UserRepository;
        private readonly ITollCostRepository TollCostRepository;
        private readonly ICategoryRepository CategoryRepository;
        public TollController(ITollRepository TollRepository, ITollCostRepository TollCostRepository, IUserRepository UserRepository, ICategoryRepository CategoryRepository)
        {
            this.TollRepository = TollRepository;
            this.TollCostRepository = TollCostRepository;
            this.UserRepository = UserRepository;
            this.CategoryRepository = CategoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TollDto>>> GetItems()
        {
            try
            {
                var Tolls = await this.TollRepository.GetItems();


                if (Tolls == null)
                {
                    return NotFound();
                }
                var TollCost = await this.TollCostRepository.GetItems();
                var Category = await this.CategoryRepository.GetItems();
               
                if (TollCost == null || Category == null)
                {
                    throw new Exception("No products exist in the system");

                }
                else
                {
                    var cartItemsDto = Tolls.ConvertToDto(TollCost, Category);

                    return Ok(cartItemsDto);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TollDto>> GetItem(int id)
        {
            try
            {
                var Toll = await this.TollRepository.GetItem(id);

                if (Toll == null)
                {
                    return BadRequest();
                }
                else
                {

                    var TollDto = Toll.ConvertToDto();

                    return Ok(TollDto);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }




        [HttpPost]
        public async Task<ActionResult<TollToAddDto>> PostItem([FromBody] TollToAddDto TollToAddDto)
        {
            try
            {
                var newItem = await this.TollRepository.AddItem(TollToAddDto);

                if (newItem == null)
                {
                    return NoContent();
                }

                return CreatedAtAction(nameof(GetItem), new { id = TollToAddDto.Id }, TollToAddDto);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("GetTollCost/{CategoryId}/{Time}")]
        public async Task<ActionResult<IEnumerable<TollDto>>> GetTollCost(int CategoryId, string Time)
        {
            try
            {
                var Tolls = await this.TollRepository.GetTollCost(CategoryId, Time);

                var TollsDtos = Tolls.ConvertToDto();

                return Ok(TollsDtos);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }



        [HttpGet("{userid}/{Date}")]
        public async Task<ActionResult<IEnumerable<TollDto>>> GetUserItem(int userid, string Date)
        {
            try
            {
                var Tolls = await this.TollRepository.GetUserItem(userid, Date);

                if (Tolls == null || !Tolls.Any())
                {
                    return null;
                }
                var TollCost = await this.TollCostRepository.GetItems();
                var Category = await this.CategoryRepository.GetItems();
                if (TollCost == null || Category == null)
                {
                    throw new Exception("No products exist in the system");

                }
                else
                {
                    var cartItemsDto = Tolls.ConvertToDto(TollCost, Category);

                    return Ok(cartItemsDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("GetTolltime/{userid}/{time}")]
        public async Task<ActionResult<TollDto>> GetTolltime(int userid, string time)
        {
            try
            {
                var Toll = await this.TollRepository.GetTolltime(userid, time);

                if (Toll == null)
                {
                    return NotFound("No Toll record found within the last hour.");
                }

                var TollDto = Toll.ConvertToDto();

                return Ok(TollDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpPut("UpdateUserCost/{TollId}")]
        public async Task<ActionResult> UpdateUserCost(int TollId, [FromBody] UpdateUserCostDto model)
        {
            var success = await TollRepository.UpdateUserCost(TollId, model.UserCost);
            if (success)
                return Ok();
            return NotFound();
        }


        [HttpGet("GetTotalCostForDay/{userid}/{date}")]
        public async Task<ActionResult<decimal>> GetTotalCostForDay(int userid, string date)
        {
            try
            {
                var totalCost = await this.TollRepository.GetTotalCostForDay(userid, date);
                if (totalCost == 0)
                {
                    return Ok(0);
                }

                return Ok(totalCost);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving total cost data from the database");
            }
        }


    }
}
