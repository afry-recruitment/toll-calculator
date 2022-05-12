using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace TollFeeCalculator.utils
{
    class DataBaseContext : DbContext
    {
        public string DatabaseFile { get; set; } = "TollCalculator.db";
        public DbSet<models.Fee> Fees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(path, "VSDatabase");
            Directory.CreateDirectory(path);
            path = Path.Combine(path, DatabaseFile);

            optionsBuilder.UseSqlite("Data Source = " + path + ";");
        }
    }
}
