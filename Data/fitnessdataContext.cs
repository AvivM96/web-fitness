using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace web_fitness.Data
{
    public class fitnessdataContext : DbContext
    {

        public fitnessdataContext(DbContextOptions<fitnessdataContext> options)
    : base(options){ }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            optionsBuilder.UseSqlite("Data Source=App_Data/fitnessdata.db");
        }
        public DbSet<web_fitness.Models.TrainingType> TrainingTypes { get; set; }

    }
}
