using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BackgroundWorker.Models
{
    public class InfoContext : DbContext
    {
        public InfoContext(DbContextOptions<InfoContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=tcp:ambrosiadatc.database.windows.net,1433;Initial Catalog=AmbrosiaDB;Persist Security Info=False;User ID=cr1ss;Password=ProiectDATC123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public DbSet<PointsOfInterestDTO> PointsOfInterest { get; set; }
        public DbSet<UserDTO> User { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PointsOfInterestDTO>().ToTable("PointOfInterest").HasKey(s => s.Id);
            modelBuilder.Entity<UserDTO>().ToTable("User").HasKey(s => s.Id);
        }
    }
}
