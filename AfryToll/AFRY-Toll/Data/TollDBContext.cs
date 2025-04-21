using AFRY_Toll.Entities;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using AFRYTollApi.Entities;

namespace AFRY_Toll.Data
{
    public class TollDBContext : DbContext
    {
        public TollDBContext(DbContextOptions<TollDBContext> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<TollCost> TollCosts { get; set; }
        public DbSet<Transport> Transports { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
