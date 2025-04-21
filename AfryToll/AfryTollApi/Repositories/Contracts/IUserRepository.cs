using AfryToll.Model.Dtos;
using AfryTollApi.Entities;

namespace AfryTollApi.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetUserItem(string PlateNumber, string Password);
        Task<User> AddItem(UserAddDto UserAddDto);
        Task<User> GetItem(int id);
    }
}
