using AfryToll.Model.Dtos;

namespace AfryTollApp.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDto> GetUserItem(string UserName, string Password);
        Task<UserAddDto> AddItem(UserAddDto UserAddDto);
    }
}
