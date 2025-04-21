using AfryToll.Model.Dtos;
using AfryTollApi.Entities;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository CategoryRepository;

        public CategoryController(ICategoryRepository CategoryRepository)
        {
            this.CategoryRepository = CategoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetItems()
        {
            try
            {
                var Categories = await this.CategoryRepository.GetItems();


                if (Categories == null)
                {
                    return NotFound();
                }
                else
                {
                    var CategoryDtos = Categories.ConvertToDto();

                    return Ok(CategoryDtos);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetItem(int id)
        {
            try
            {
                var product = await this.CategoryRepository.GetItem(id);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {



                    return Ok(product);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostItem([FromBody] CategoryDto CategoryDto)
        {
            try
            {
                var newItem = await this.CategoryRepository.AddItem(CategoryDto);

                if (newItem == null)
                {
                    return NoContent();
                }

                return CreatedAtAction(nameof(GetItem), new { id = CategoryDto.Id }, CategoryDto);



            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("CategoryStatus/{Status:bool}")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetstatusItems(bool Status)
        {
            try
            {
                var Categories = await this.CategoryRepository.GetstatusItems(Status);

                var CategoriesDtos = Categories.ConvertToDto();

                return Ok(CategoriesDtos);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }


    }
}
