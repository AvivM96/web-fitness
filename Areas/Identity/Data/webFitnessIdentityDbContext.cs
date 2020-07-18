using System;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class webFitnessIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public webFitnessIdentityDbContext(DbContextOptions<webFitnessIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
        optionsBuilder.UseSqlite("Data Source=App_Data/fitnessdata.db");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
