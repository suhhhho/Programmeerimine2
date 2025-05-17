using System;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public class FakeStartup
    {
        public FakeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            // Генерируем уникальный идентификатор для каждой базы данных теста
            var dbGuid = Guid.NewGuid().ToString("N");
            var baseConnectionString = Configuration.GetConnectionString("TestConnection");

            // Заменяем имя базы данных на уникальное для каждого теста
            var uniqueConnectionString = baseConnectionString.Replace("Database=TestDb", $"Database=TestDb_{dbGuid}");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(uniqueConnectionString);
                // Добавляем настройки для лучшей работы с соединениями
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddAutoMapper(GetType().Assembly);
            services.AddControllersWithViews()
                    .AddApplicationPart(typeof(HomeController).Assembly);

            services.AddScoped<ICarsService, CarsService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IRentService, RentService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{pathStr?}");
            });

            // Упрощаем логику инициализации базы
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (dbContext == null)
                {
                    throw new NullReferenceException("Cannot get instance of dbContext");
                }

                if (dbContext.Database.GetDbConnection().ConnectionString.ToLower().Contains("my.db"))
                {
                    throw new Exception("LIVE SETTINGS IN TESTS!");
                }

                // Удалим и создадим базу с нуля
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
        }
    }
}