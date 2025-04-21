using AfryTollApi.Entities;
using System.Security.Cryptography.Xml;
using AfryToll.Model.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;

namespace AfryTollApi.Extention
{
    public static class DtoConversion
    {
        public static IEnumerable<TollCostDto> ConvertToDto(this IEnumerable<TollCost> TollCosts)
        {
            return (from TollCost in TollCosts
                    select new TollCostDto
                    {
                        Id = TollCost.Id,
                        CategoryId = TollCost.CategoryId,
                        Traffic = TollCost.Traffic,
                        Time = TollCost.Time,
                        Cost = TollCost.Cost,



                    }).ToList();
        }
        public static TollCostDto ConvertToDto(this TollCost TollCost)

        {
            if (TollCost == null)
            {
                return null;
            }

            return new TollCostDto
            {
                Id = TollCost.Id,
                Cost = TollCost.Cost,
                Traffic = TollCost.Traffic,
                Time = TollCost.Time,
                CategoryId = TollCost.CategoryId,

            };

        }
        public static IEnumerable<TollCostDto> ConvertToDto(this IEnumerable<TollCost> TollCosts,
                                                       IEnumerable<Category> Categories)
        {
            return (from TransporattionCost in TollCosts
                    join Category in Categories
                    on TransporattionCost.CategoryId equals Category.Id
                    select new TollCostDto
                    {
                        Id = TransporattionCost.Id,
                        CategoryId = TransporattionCost.CategoryId,
                        Traffic = TransporattionCost.Traffic,
                        Cost = TransporattionCost.Cost,
                        Time = TransporattionCost.Time,
                        CategoryName = TransporattionCost.Category?.Name ?? "Unknown"

                    }).ToList();
        }
        public static TollCostDto ConvertToDto(this TollCost TollCost,
                                                    Category Category)
        {
            return new TollCostDto
            {
                Id = TollCost.Id,
                CategoryId = TollCost.CategoryId,
                Traffic = TollCost.Traffic,
                Cost = TollCost.Cost,
                CategoryName = TollCost.Category?.Name ?? "Unknown"
            };
        }



        public static IEnumerable<TollDto> ConvertToDto(this IEnumerable<Toll> Tolls)
        {
            return (from Toll in Tolls
                    select new TollDto
                    {
                        Id = Toll.Id,
                        UserCost = Toll.UserCost,
                        Date = Toll.Date,
                        UserId = Toll.UserId,
                        Time = Toll.Time,
                        Temp = Toll.Temp,
                        TempCost = Toll.TempCost,



                        TollCostCategoryId = Toll.TollCostId,
                    }).ToList();
        }

        public static TollDto ConvertToDto(this Toll Toll)

        {
            return new TollDto
            {
                Id = Toll.Id,
                UserCost = Toll.UserCost,
                Date = Toll.Date,
                UserId = Toll.UserId,
                Time = Toll.Time,
                Temp = Toll.Temp,
                TempCost = Toll.TempCost,

                TollCostId = Toll.TollCostId
            };

        }
        public static IEnumerable<TollDto> ConvertToDto(this IEnumerable<Toll> Tolls,
                                                      IEnumerable<TollCost> TollCosts,
                                                       IEnumerable<Category> Categories)
        {
            return (from Toll in Tolls
                    join TollCost in TollCosts
                    on Toll.TollCostId equals TollCost.Id
                    join Category in Categories
                    on Toll.TollCost.CategoryId equals Category.Id
                    select new TollDto

                    {
                        Id = Toll.Id,
                        UserCost = Toll.UserCost,
                        UserId = Toll.UserId,
                        Date = Toll.Date,

                        Time = Toll.Time,
                        Temp = Toll.Temp,
                        TempCost = Toll.TempCost,

                        TollCostId = Toll.TollCost?.Id ?? 0,
                        TollCostCategoryId = Toll.TollCost?.CategoryId ?? 0,

                        TollCostCost = Toll.TollCost?.Cost ?? 0,
                        TollCostCategoryName = Toll.TollCost?.Category?.Name ?? "Unknown"
                    }).ToList();
        }


        public static TollDto ConvertToDto(this Toll Toll,
                                                TollCost TollCost)
        {
            return new TollDto
            {
                Id = Toll.Id,
                UserCost = Toll.UserCost,
                UserId = Toll.UserId,
                Date = Toll.Date,
                Time = Toll.Time,
                Temp = Toll.Temp,
                TempCost = Toll.TempCost,

                TollCostId = Toll.TollCost?.Id ?? 0,
                TollCostCategoryId = Toll.TollCost?.CategoryId ?? 0,

                TollCostCost = Toll.TollCost?.Cost ?? 0,
                TollCostCategoryName = Toll.TollCost?.Category?.Name ?? "Unknown"

            };
        }




        public static IEnumerable<CategoryDto> ConvertToDto(this IEnumerable<Category> Categories)
        {
            return (from Category in Categories
                    select new CategoryDto
                    {
                        Id = Category.Id,
                        Name = Category.Name,
                        Status = Category.Status,

                    }).ToList();
        }

        public static CategoryDto ConvertToDto(this Category Category)

        {
            return new CategoryDto
            {
                Id = Category.Id,
                Name = Category.Name,
                Status = Category.Status,

            };

        }


        public static IEnumerable<UserDto> ConvertToDto(this IEnumerable<User> Users, 
                                                       IEnumerable<Category> Categories)
        {

            return (from User in Users
                    join Category in Categories
                    on User.CategoryId equals Category.Id
                    select new UserDto
                    {
                        UserId = User.UserId,
                        CategoryId = User.CategoryId,
                        
                        Password = User.Password,
                        PlateNumber = User.PlateNumber,
                        CategoryName = User.Category?.Name ?? "Unknown"

                    }).ToList();

        }
        public static UserDto ConvertToDto(this User User,
                                                 Category Category)
        {
            return new UserDto
            {
                UserId = User.UserId,
                PlateNumber = User.PlateNumber,
                Password = User.Password,
                CategoryId = User.CategoryId,
                CategoryName = User.Category?.Name ?? "Unknown"
            };
        }
        public static UserDto ConvertToDto(this User User)

        {
            return new UserDto
            {
                UserId = User.UserId,
                PlateNumber = User.PlateNumber,
                Password = User.Password,
                CategoryId = User.CategoryId,


            };

        }


    }
}