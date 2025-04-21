
using AfryToll.Model.Dtos;


namespace AfryTollApp.Services.Contracts
{
    public interface ITollService
    {
        Task<List<TollDto>> GetItems();
        Task<TollDto> GetItem(int id);
        Task<List<TollDto>> GetUser(int userid);
        Task<List<TollDto>> GetUserItem(int userid, string Date);
        Task<TollCostDto> GetTollCost(int CategoryId, string Time);
        Task<TollToAddDto> AddItem(TollToAddDto TollToAddDto);
        Task<TollDto> GetTolltime(int userid, string time);
        Task<bool> UpdateUserCost(int TollId, int UserCost);
        Task<IEnumerable<TollCostDto>> GetTollCosts();
        Task<int> GetTotalCostForDay(int userid, string date);
    }
}
