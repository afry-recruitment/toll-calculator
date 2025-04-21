using AfryToll.Model.Dtos;
using AfryTollApi.Entities;
using AfryTollApi.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using AfryTollApi.Data;


namespace AfryTollApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TollDBContext TollDBContext;

        public CategoryRepository(TollDBContext TollDBContext)
        {
            this.TollDBContext = TollDBContext;
        }
        public async Task<IEnumerable<Category>> GetItems()
        {
            var Category = await this.TollDBContext.Category
                                     .ToListAsync();
            this.TollDBContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return Category;

        }

        private async Task<bool> CategoryExists(int id)
        {
            return await this.TollDBContext.Category.AnyAsync(c => c.Id == id
                                                                    );

        }

        public async Task<Category> AddItem(CategoryDto CategoryDto)
        {


            if (await CategoryExists(CategoryDto.Id) == false)
            {
                Category item = new Category();

                item.Name = CategoryDto.Name;
                item.Status = CategoryDto.Status;

                if (item != null)
                {
                    var result = await this.TollDBContext.Category.AddAsync(item);
                    await this.TollDBContext.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;
        }

        public async Task<Category> GetItem(int id)
        {
            var category = await TollDBContext.Category

                                .SingleOrDefaultAsync(p => p.Id == id);
            return category;
        }

        public async Task<IEnumerable<Category>> GetstatusItems(bool Status)
        {


            var Catgory = await this.TollDBContext.Category
                .Where(c => c.Status == Status)
                .ToListAsync();

            return Catgory;
        }



    }
}
