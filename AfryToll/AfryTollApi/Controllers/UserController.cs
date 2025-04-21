using AfryToll.Model.Dtos;
using AfryTollApi.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using AfryToll.Model.Dtos;
using AfryTollApi.Extention;
using AfryTollApi.Repositories.Contracts;
using AfryTollApi.Entities;
using AfryTollApi.Repositories;

namespace AfryTollApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository UserRepository;
        private readonly ICategoryRepository CategoryRepository;
        public UserController(IUserRepository UserRepository, ICategoryRepository CategoryRepository)
        {
            this.UserRepository = UserRepository;
            this.CategoryRepository = CategoryRepository;
        }

        [HttpGet("{PlateNumber}/{Password}")]
        public async Task<ActionResult<UserDto>> GetUserItem(string PlateNumber, string Password)


        {
            try
            {
                var User = await this.UserRepository.GetUserItem(PlateNumber, Password);

                if (User == null)
                {
                    return null;
                }
                else
                {
                    var UserDto = User.ConvertToDto();
                    return Ok(UserDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetItem(int id)
        {
            try
            {
                var User = await this.UserRepository.GetItem(id);

                if (User == null)
                {
                    return BadRequest();
                }

                var Category = await CategoryRepository.GetItem(User.CategoryId);

                if (Category == null)
                {
                    return NotFound();
                }
                var cartItemDto = User.ConvertToDto(Category);

                return Ok(cartItemDto);


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }



        [HttpPost]
        public async Task<ActionResult<UserAddDto>> PostItem([FromBody] UserAddDto UserAddDto)
        {
            try
            {
                var newItem = await this.UserRepository.AddItem(UserAddDto);




                if (newItem == null)
                {
                    return NoContent();
                }


                var newItemDto = newItem.ConvertToDto();

                return CreatedAtAction(nameof(GetItem), new { id = newItemDto.UserId }, newItemDto);

               



            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
