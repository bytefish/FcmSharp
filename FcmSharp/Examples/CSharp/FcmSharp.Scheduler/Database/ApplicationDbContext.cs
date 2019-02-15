using FcmSharp.Scheduler.Database.Configuration;
using FcmSharp.Scheduler.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace FcmSharp.Scheduler.Database
{

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Messaging.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MessageTypeConfiguration());
            
            Seeding.SeedData(modelBuilder);
        }
    }
}
