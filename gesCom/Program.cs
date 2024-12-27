using gesCom.Data;
using gesCom.Fixtures;
using gesCom.Services;
using gesCom.Services.Impl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("MysqlConnection")!;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString,
            new MySqlServerVersion(new Version(8, 0, 31))));

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IResponsableStockService, ResponsableStockService>();
builder.Services.AddScoped<IComptableService, ComptableService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
    });

builder.Services.AddScoped<DataFixtures>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())

{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<DataFixtures>();
    seeder.Load();
}
app.Run();
