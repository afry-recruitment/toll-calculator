using AfryToll.Model.Dtos;
using AfryTollApi.Entities;

namespace AfryTollApi.Repositories.Contracts
{
    public interface ITollRepository
    {

        Task<Toll> GetItem(int id);
        Task<IEnumerable<Toll>> GetItems();
        Task<TollCost> GetTollCost(int Categoryid, string Time);
        Task<Toll> GetTolltime(int userid, string time);
        Task<bool> UpdateUserCost(int TollId, int UserCost);
        Task<Toll> AddItem(TollToAddDto TollToAddDto);
        Task<IEnumerable<Toll>> GetUser(int Userid);
        Task<IEnumerable<Toll>> GetUserItem(int Userid, string Date);
        Task<int> GetTotalCostForDay(int userId, string date);
        //Task<IEnumerable<Toll>> GetUserCost(int UserId, string Date);


        //Task<IEnumerable<Toll>> GetUserItems();


    }
}
