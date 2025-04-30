
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using finalproject.Data;
using finalproject.Models;
using finalproject.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;

using Microsoft.AspNetCore.Identity;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
 

var conn = Environment.GetEnvironmentVariable("CONNECTION_STRING");
if (!string.IsNullOrEmpty(conn))
{
    // Production: Postgres with pending‐changes warning suppressed
    builder.Services.AddDbContext<ApplicationDbContext>(opts =>
        opts.UseNpgsql(conn)
            .ConfigureWarnings(w => 
                w.Ignore(RelationalEventId.PendingModelChangesWarning)));
}
else
{
    // Development: SQL Server fallback
    var sqlConn = builder.Configuration.GetConnectionString("DefaultConnection")
                  ?? throw new InvalidOperationException("Missing DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(opts =>
        opts.UseSqlServer(sqlConn)
            .ConfigureWarnings(w => 
                w.Ignore(RelationalEventId.PendingModelChangesWarning)));
}

// builder.Services.AddIdentity<ApplicationUser, IdentityRole>()


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
  options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});


builder.Services.AddScoped<CloudinaryService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Clear();
app.Urls.Add($"http://0.0.0.0:{port}");



using (var scope = app.Services.CreateScope())
{
    await SeedData.InitializeAsync(scope.ServiceProvider);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 
    app.Run();
