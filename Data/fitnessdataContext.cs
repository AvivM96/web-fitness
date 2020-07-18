
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace web_fitness.Data
{
    public class fitnessdataContext : DbContext
    {
        public fitnessdataContext(DbContextOptions<fitnessdataContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            optionsBuilder.UseSqlite("Data Source=App_Data/fitnessdata.db");
        }

        public DbSet<web_fitness.Models.TrainingType> TrainingTypes { get; set; }
        public DbSet<web_fitness.Models.Trainer> Trainers { get; set; }
        public DbSet<web_fitness.Models.Meeting> Meetings { get; set; }
        public DbSet<web_fitness.Models.Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Meeting>().HasKey(c => new { c.MeetDate,c.TrainingTypeID, c.TrainerID});
           // modelBuilder.Entity<Meeting>().has(p => p.AuthorFK);

            //modelBuilder.Entity<Meeting>().Property(p => p.MeetNum).ValueGeneratedOnAdd();
        }

    }
}
