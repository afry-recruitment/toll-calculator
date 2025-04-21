using AfryToll.Model.Dtos;
using AfryTollApi.Entities;

namespace AfryTollApi.Repositories.Contracts
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetItems();
        Task<Category> AddItem(CategoryDto CategoryDto);
        Task<Category> GetItem(int id);
        Task<IEnumerable<Category>> GetstatusItems(bool Status);

    }
}
