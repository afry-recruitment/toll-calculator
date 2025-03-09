using Microsoft.EntityFrameworkCore;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Crossing> Crossings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillingInfo>()
                .HasOne(b => b.Vehicle) 
                .WithMany(v => v.BillingInfos) 
                .HasForeignKey(b => b.VehicleId) 
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}