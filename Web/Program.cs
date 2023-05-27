using BusinessLogic.Services;
using DataAccess.Wrapper;
using DataAccess;
using Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Web.Auth;
using Blazored.LocalStorage;
using MudBlazor.Services;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<InternetstoreContext>(
                optionsAction: options => options.UseSqlServer(
                    "Server= LAPTOP-2TNGE6GH ;Database= Internet-store; Trusted_Connection=True;"));

            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IHaracterysticaService, HaracterysticaService>();
            builder.Services.AddScoped<ITovarService, TovarService>();
            builder.Services.AddScoped<ICorzinaService, CorzinaService>();
            builder.Services.AddScoped<IZakazService, ZakazService>();

            // Add services to the container.
            builder.Services.AddAuthenticationCore();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddScoped<ProtectedSessionStorage>();
            builder.Services.AddScoped<ProtectedLocalStorage>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddMudServices();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}