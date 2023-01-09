using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace APIAPP.Models
{
    public class InfoContext : DbContext
    {
        public InfoContext(DbContextOptions<InfoContext> options) : base(options)
        {

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
