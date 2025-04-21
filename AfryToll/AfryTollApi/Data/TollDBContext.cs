using AfryTollApi.Entities;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using Microsoft.EntityFrameworkCore;

namespace AfryTollApi.Data
{
    public class TollDBContext : DbContext
    {
        public TollDBContext(DbContextOptions<TollDBContext> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<TollCost> TollCosts { get; set; }
        public DbSet<Toll> Tolls { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Toll>()
                .HasOne(t => t.User)
                .WithMany() // اگر User تعدادی Toll دارد، بنویس WithMany(u => u.Tolls)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict); // یا .NoAction()

            modelBuilder.Entity<Toll>()
                .HasOne(t => t.TollCost)
                .WithMany() // اگر TollCost تعدادی Toll دارد، بنویس WithMany(tc => tc.Tolls)
                .HasForeignKey(t => t.TollCostId)
                .OnDelete(DeleteBehavior.Restrict); // یا فقط یکی از این‌ها کافی است
        }
    }
}
