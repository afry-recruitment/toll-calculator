using AfryTollApi.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using AfryToll.Model.Dtos;
using AfryTollApi.Entities;
using AfryTollApi.Extention;
using AfryTollApi.Repositories.Contracts;

namespace AfryTollApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TollCostController : ControllerBase
    {
        private readonly ITollCostRepository TollCostRepository;
        private readonly ICategoryRepository CategoryRepository;

        public TollCostController(ITollCostRepository TollCostRepository, ICategoryRepository CategoryRepository)
        {
            this.TollCostRepository = TollCostRepository;
            this.CategoryRepository = CategoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TollCostDto>>> GetItems()
        {
            try
            {
                var TollCosts = await this.TollCostRepository.GetItems();


                if (TollCosts == null)
                {
                    return NotFound();
                }

                var Category = await this.CategoryRepository.GetItems();

                if (Category == null)
                {
                    throw new Exception("No products exist in the system");

                }
                else
                {
                    var cartItemsDto = TollCosts.ConvertToDto(Category);

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
        public async Task<ActionResult<TollCost>> GetItem(int id)
        {
            try
            {
                var TollCost = await this.TollCostRepository.GetItem(id);

                if (TollCost == null)
                {
                    return BadRequest();
                }

                var Category = await CategoryRepository.GetItem(TollCost.CategoryId);

                if (Category == null)
                {
                    return NotFound();
                }
                var cartItemDto = TollCost.ConvertToDto(Category);

                return Ok(cartItemDto);


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }
        [HttpPost]
        public async Task<ActionResult<TollCostAddDto>> PostItem([FromBody] TollCostAddDto TollCostAddDto)
        {
            try
            {
                var newItem = await this.TollCostRepository.AddItem(TollCostAddDto);




                if (newItem == null)
                {
                    return NoContent();
                }


                var newItemDto = newItem.ConvertToDto();

                return CreatedAtAction(nameof(GetItem), new { id = newItemDto.Id }, newItemDto);

                //   return CreatedAtAction(nameof(GetItem), new { id = TollCostDto.Id }, TollCostDto);



            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("Category/{CategoryId}")]
        public async Task<ActionResult<IEnumerable<TollCostDto>>> GetCategorItem(int CategoryId)
        {
            try
            {
                var TollCosts = await this.TollCostRepository.GetCategorItem(CategoryId);
                if (TollCosts == null)
                {
                    return BadRequest();
                }
                var Category = await this.CategoryRepository.GetItems();

                if (Category == null)
                {
                    throw new Exception("No products exist in the system");

                }
                else
                {
                    var cartItemsDto = TollCosts.ConvertToDto(Category);

                    return Ok(cartItemsDto);
                }





            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<TollCostDto>> DeleteItem(int id)
        {
            try
            {
                var Item = await this.TollCostRepository.DeleteItem(id);

                if (Item == null)
                {
                    return NotFound();
                }



                return Ok(Item);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
