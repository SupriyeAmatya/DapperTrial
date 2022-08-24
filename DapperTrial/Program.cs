using DapperTrial.Models.DataModel;
using DapperTrial.Services.LoginServices;
using DapperTrial.Services.RentvehicleServices;
using DapperTrial.Services.StationServices;
using DapperTrial.Services.UserhomeServices;
using DapperTrial.Services.VehicleServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IVehicleService, VehileService>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<IUserHomeService, UserhomeService>();
builder.Services.AddScoped<IRentvehicleService, RentvehicleService>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

string dataconnection = builder.Configuration.GetConnectionString("DapperConnection");

//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//            .AddDefaultTokenProviders();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1000);
});
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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Rentvehicle}/{action=Index}/{id?}");

app.Run();
