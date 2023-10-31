global using DeviceSystem.Models;
global using DeviceSystem.Data;
global using Microsoft.EntityFrameworkCore;
global using DeviceSystem.Requests.User;
global using DeviceSystem.Response_DTO_;
using DeviceSystem.Extensions;

var builder = WebApplication.CreateBuilder(args);

//* Get the values from the configuration file and match them with the class properties
var _settings = builder.Configuration
                .GetSection(nameof(MySQLSettings))
                .Get<MySQLSettings>() ??
                 throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDatabase(_settings!.ConnectionString);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add authentication cookies
builder.Services.AddApplicationAuthentication();

//Add application Repositories and Services
builder.Services.AddApplication();

//for accessing HttpContext 
builder.Services.AddHttpContextAccessor();

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

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Login}/{id?}");

app.Run();
