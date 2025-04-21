using AfryToll.Model.Dtos;

namespace AfryTollApp.Services.Contracts
{
    public interface ITollCostService
    {
        Task<List<TollCostDto>> GetItems();
        Task<TollCostAddDto> AddItem(TollCostAddDto TollCostAddDto);
        Task<List<TollCostDto>> GetCategorItem(int CategoryId);
        Task<TollCostDto> GetItem(int id);
        Task<TollCostDto> DeleteItem(int id);
    }
}
