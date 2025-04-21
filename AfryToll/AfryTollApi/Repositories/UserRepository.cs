using AfryToll.Model.Dtos;
using AfryTollApi.Entities;
using AfryTollApi.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using AfryTollApi.Data;


namespace AfryTollApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TollDBContext TollDBContext;

        public UserRepository(TollDBContext TollDBContext)
        {
            this.TollDBContext = TollDBContext;
        }

        public async Task<User> GetUserItem(string PlateNumber, string Password)
        {
            var User = await TollDBContext.Users

                               .SingleOrDefaultAsync(p => p.PlateNumber == PlateNumber && p.Password == Password);
            return User;
        }
        private async Task<bool> UserExists(int id)
        {
            return await this.TollDBContext.Users.AnyAsync(c => c.UserId == id
                                                                    );

        }

        public async Task<User> AddItem(UserAddDto UserAddDto)
        {


            if (await UserExists(UserAddDto.UserId) == false)
            {

                User item = new User();


                item.PlateNumber = UserAddDto.PlateNumber;
                item.Password = UserAddDto.Password;
                item.CategoryId = UserAddDto.CategoryId;

                if (item != null)
                {
                    var result = await this.TollDBContext.Users.AddAsync(item);
                    await this.TollDBContext.SaveChangesAsync();
                    return result.Entity;
                }

            }

            return null;
        }
        public async Task<User> GetItem(int id)
        {
            var User = await TollDBContext.Users

                                .SingleOrDefaultAsync(p => p.UserId == id);
            return User;
        }









    }





}
