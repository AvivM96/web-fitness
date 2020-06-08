using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using web_fitness.Models;
using web_fitness.Data;

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
        public DbSet<web_fitness.Models.Meeting> Meeting { get; set; }
        public DbSet<web_fitness.Models.Customer> Customer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Meeting>().HasKey(c => new { c.MeetDate,c.TrainingTypeID, c.TrainerID});

           modelBuilder.Entity<Meeting>().Property(p => p.MeetNum).ValueGeneratedOnAdd();
        }

    }
}
