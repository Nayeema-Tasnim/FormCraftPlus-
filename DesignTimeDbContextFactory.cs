using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using finalproject.Data;
using System;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var conn = Environment.GetEnvironmentVariable("DATABASE_URL");

        if (!string.IsNullOrEmpty(conn))
        {
            optionsBuilder.UseNpgsql(conn);
        }
        else
        {
           
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=template_db;Username=postgres;Password=YourPass");
        }

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
