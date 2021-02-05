using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Infrastructure.Middleware;
using WebStore.Infrastructure.Interfaces;

using System;
using WebStore.DAL.Context;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Infrastructure.Services.InMemory;
using WebStore.Infrastructure.Services.InSQL;
using WebStore.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace WebStore
{
    public record Startup(IConfiguration Configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<WebStoreDB>(opt => opt.UseSqlite(Configuration.GetConnectionString("Sqlite")));
            services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddTransient<WebStoreDbInitializer>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;
#endif

                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            });

            services.ConfigureExternalCookie(opt =>
            {
                opt.Cookie.Name = "WebStore.GB";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            //регистрируем сервис
            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();
            //services.AddTransient<IProductData, InMemoryProductData>();
            services.AddTransient<IProductData, SqlProductData>();

            //services.AddTransient<>(); так регистрируем сервис, который не должен хранить состояние
            //services.AddScoped<>(); так регистрируем сервис, который должен помнить состояние на время обработки входящего потока          
            //services.AddSingleton<>(); так регистрируем сервис, хранящий состояние на все время жизни приложения 

            //services.AddMvc(opt => opt.Conventions.Add(new TestControllerModelConvention()));        

            services
                 .AddControllersWithViews(/*opt => opt.Conventions.Add(new TestControllerModelConvention())*/)
                 .AddRazorRuntimeCompilation();
        }
       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebStoreDbInitializer db /*, IServiceProvider services*/)
        {
            //var employees1 = services.GetService<IEmployeesData>();
            //var employees2 = services.GetService<IEmployeesData>();

            //var hash1 = employees1.GetHashCode();
            //var hash2 = employees2.GetHashCode();

            //using (var scope = services.CreateScope())
            //{
            //    var employees3 = scope.ServiceProvider.GetService<IEmployeesData>();
            //    var hash3 = employees3.GetHashCode();
            //}

            db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWelcomePage("/welcome");

            app.UseMiddleware<TestMiddleware>();

            app.MapWhen(
               context => context.Request.Query.ContainsKey("id") && context.Request.Query["id"] == "5",
               context => context.Run(async request => await request.Response.WriteAsync("Hello with id == 5!"))
               );
            app.Map("/hello", context => context.Run(async request => await request.Response.WriteAsync("Hello!!!")));
           

            //var greetings = Configuration["Greetings"];
            app.UseEndpoints(endpoints =>
            {
              endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                // htpp:localhost:5000 -> controller = "Home" action = "Index" параметр  = null
                // htpp:localhost:5000/Catalog/Products/5 controller = "Catalog" action = "Products" параметр  = 5
            });
        }
    }
}
