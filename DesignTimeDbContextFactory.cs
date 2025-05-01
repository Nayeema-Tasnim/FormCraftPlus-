

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using finalproject.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
       
        builder.UseNpgsql("Host=localhost;Port=5432;Database=template_db;Username=postgres;Password=YourPass");
        return new ApplicationDbContext(builder.Options);
    }
}
