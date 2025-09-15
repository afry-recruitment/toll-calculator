using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using tollCalculator.Models;

namespace tollCalculator.Data
{
	public class ApplicationDbContext : DbContext
	{
		private readonly IConfiguration _configuration;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
			: base(options)
		{
			_configuration = configuration;
		}

		public DbSet<Vehicle> Vehicles { get; set; }
		public DbSet<ActiveFeeData> ActiveFeeDatas { get; set; }
		public DbSet<FeeDataReceipt> FeeDataReceipts { get; set; }
		public DbSet<VehicleType> VehicleTypes { get; set; }
		public DbSet<TollExempt> TollExempts { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connectionString = _configuration.GetConnectionString("Defaultconnection");
			optionsBuilder.UseSqlServer(connectionString);
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			//Relation Vehicle -> VehicleType
			modelBuilder.Entity<Vehicle>()
			.HasOne(v => v.VehicleType)
			.WithMany(vt => vt.Vehicles)
			.HasForeignKey(v => v.VehicleTypeID)
			.IsRequired();

			//Relation Vehicle -> ActiveFeeData
			modelBuilder.Entity<Vehicle>()
			.HasOne(v => v.ActiveFeeData)
			.WithOne(a => a.Vehicle)
			.HasForeignKey<Vehicle>(v => v.ActiveFeeDataID);

			//Relation Vehicle -> FeeDataReceipt
			modelBuilder.Entity<FeeDataReceipt>()
			.HasOne(f => f.Vehicle)
			.WithMany(v => v.FeeDataReceipts)
			.HasForeignKey(f => f.VehicleID)
			.IsRequired();

			//Relation VehicleType -> TollExempt
			modelBuilder.Entity<VehicleType>()
			.HasOne(vt => vt.TollExempt)
			.WithMany(te => te.VehicleTypes)
			.HasForeignKey(vt => vt.TollExemptID)
			.IsRequired();
		}

		}
}
