using AfryToll.Model.Dtos;
using AfryTollApi.Entities;
using AfryTollApi.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using AfryTollApi.Data;

namespace AfryTollApi.Repositories
{
    public class TollRepository : ITollRepository
    {
        private readonly TollDBContext TollDBContext;

        public TollRepository(TollDBContext TollDBContext)
        {
            this.TollDBContext = TollDBContext;
        }
        public async Task<Toll> GetItem(int id)
        {
            var Toll = await TollDBContext.Tolls
                                .Include(p => p.TollCost)
                                .SingleOrDefaultAsync(p => p.Id == id);
            return Toll;
        }

        public async Task<IEnumerable<Toll>> GetItems()
        {
            var Toll = await this.TollDBContext.Tolls
                                     .Include(p => p.TollCost).ToListAsync();

            return Toll;

        }
        //public async Task<TollCost> GetTollCost(int CategoryId, string Time)
        //{
        //    var TollCost = await this.TollDBContext.TollCosts.SingleOrDefaultAsync(c => c.CategoryId == CategoryId && c.Time == Time);
        //    return TollCost;
        //}

        public async Task<TollCost> GetTollCost(int categoryId, string time)
        {
            if (!TimeSpan.TryParse(time, out var inputTime))
                return null;

            var TollCosts = await this.TollDBContext.TollCosts
                .Where(c => c.CategoryId == categoryId)
                .ToListAsync();

            var matchedCost = TollCosts
                .Where(c => TimeSpan.TryParse(c.Time, out var costTime) && costTime <= inputTime)
                .OrderByDescending(c => TimeSpan.Parse(c.Time))
                .FirstOrDefault();
            return matchedCost;
        }

        public async Task<Toll> GetTolltime(int userid, string time)
        {
            if (!TimeSpan.TryParse(time, out var inputTime))
                return null;

            var TollList = await this.TollDBContext.Tolls
                .Where(c => c.UserId == userid && c.Temp == 1)
                .ToListAsync();

            var matchedToll = TollList
                .Where(c =>
                    TimeSpan.TryParse(c.Time, out var TollTime) &&
                    Math.Abs((TollTime - inputTime).TotalMinutes) < 60
                )
                .OrderByDescending(c => TimeSpan.Parse(c.Time))
                .FirstOrDefault();

            return matchedToll;
        }

        private async Task<bool> TollExists(int id)
        {
            return await this.TollDBContext.Tolls.AnyAsync(c => c.Id == id
            );
        }

        public async Task<Toll> AddItem(TollToAddDto TollToAddDto)
        {


            if (await TollExists(TollToAddDto.Id) == false)
            {
                var item = await (from TollCost in this.TollDBContext.TollCosts
                                  where TollCost.Id == TollToAddDto.TollCostId
                select new Toll
                                  {

                                      TollCostId = TollCost.Id,
                                      Date = TollToAddDto.Date,

                                      UserCost = TollToAddDto.UserCost,
                                      UserId = TollToAddDto.UserId,
                                      Time = TollToAddDto.Time,
                                      Temp = TollToAddDto.Temp,
                                      TempCost = TollToAddDto.TempCost,


                                  }).SingleOrDefaultAsync();


                if (item != null)
                {
                    var result = await this.TollDBContext.Tolls.AddAsync(item);
                    await this.TollDBContext.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;

        }
        public async Task<bool> UpdateUserCost(int TollId, int UserCost)
        {
            var Toll = await this.TollDBContext.Tolls
                .FirstOrDefaultAsync(t => t.Id == TollId);

            if (Toll == null)
            {
                return false;
            }

            Toll.UserCost = UserCost;

            await this.TollDBContext.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Toll>> GetUserItem(int userid, string Date)
        {
            var Toll = await this.TollDBContext.Tolls
                .Where(c => c.UserId == userid && c.Date.Trim() == Date)
                .ToListAsync();

            return Toll;
        }
        public async Task<IEnumerable<Toll>> GetUser(int userid)
        {
            var Toll = await this.TollDBContext.Tolls
                .Where(c => c.UserId == userid )
                .ToListAsync();

            return Toll;
        }
        public async Task<int> GetTotalCostForDay(int userId, string date)
        {

            var userTolls = await this.TollDBContext.Tolls
                .Where(c => c.UserId == userId && c.Date == date && c.Temp == 1)
                .ToListAsync();


            int totalCost = userTolls.Sum(t => t.UserCost);

            return totalCost;

        }









    }
}
