using AfryToll.Model.Dtos;
using AfryTollApi.Entities;


namespace AfryTollApi.Repositories.Contracts
{
    public interface ITollCostRepository
    {
        Task<TollCost> AddItem(TollCostAddDto TollCostAddDto);
        Task<TollCost> GetItem(int id);
        Task<IEnumerable<TollCost>> GetItems();

        Task<IEnumerable<TollCost>> GetCategorItem(int CategoryId);
        Task<TollCost> DeleteItem(int id);
    }
}
