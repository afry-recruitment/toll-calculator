using AfryToll.Model.Dtos;
using AfryTollApi.Entities;
using AfryTollApi.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using AfryTollApi.Data;

namespace AfryTollApi.Repositories
{
    public class TollCostRepository : ITollCostRepository
    {
        private readonly TollDBContext TollDBContext;

        public TollCostRepository(TollDBContext TollDBContext)
        {
            this.TollDBContext = TollDBContext;
        }

        public async Task<IEnumerable<TollCost>> GetItems()
        {
            var TollCost = await this.TollDBContext.TollCosts
                                     .ToListAsync();

            return TollCost;

        }

        private async Task<bool> TollCostExists(int id)
        {
            return await this.TollDBContext.TollCosts.AnyAsync(c => c.Id == id
                                                                    );

        }

        public async Task<TollCost> AddItem(TollCostAddDto TollCostAddDto)
        {


            if (await TollCostExists(TollCostAddDto.Id) == false)
            {
                //if (await GetcostItem(TollCostAddDto.CategoryId, TollCostAddDto.Time) == false)
                //{
                TollCost item = new TollCost();


                item.Cost = TollCostAddDto.Cost;
                item.Time = TollCostAddDto.Time;
                item.Traffic = TollCostAddDto.Traffic;
                item.CategoryId = TollCostAddDto.CategoryId;

                if (item != null)
                {
                    var result = await this.TollDBContext.TollCosts.AddAsync(item);
                    await this.TollDBContext.SaveChangesAsync();
                    return result.Entity;
                }
                //}
            }

            return null;
        }

        public async Task<TollCost> GetItem(int id)
        {
            var TollCost = await TollDBContext.TollCosts

                                .SingleOrDefaultAsync(p => p.Id == id);
            return TollCost;
        }

        public async Task<bool> GetcostItem(int CategoryId, string Time)
        {
            return await this.TollDBContext.TollCosts.AnyAsync(c => c.CategoryId == CategoryId && c.Time == Time
                                                                   );
        }

        public async Task<IEnumerable<TollCost>> GetCategorItem(int CategoryId)
        {
            var Toll = await this.TollDBContext.TollCosts
               .Where(c => c.CategoryId == CategoryId)
               .ToListAsync();

            return Toll;

        }
        public async Task<TollCost> DeleteItem(int id)
        {
            var item = await this.TollDBContext.TollCosts.FindAsync(id);

            if (item != null)
            {
                this.TollDBContext.TollCosts.Remove(item);
                await this.TollDBContext.SaveChangesAsync();
            }

            return item;

        }

    }
}
