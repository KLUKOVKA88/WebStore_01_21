using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Infrastructure.Middleware;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Services;
using System;

namespace WebStore
{
    public record Startup(IConfiguration Configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //регистрируем сервис
            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();

            //services.AddTransient<>(); так регистрируем сервис, который не должен хранить состояние
            //services.AddScoped<>(); так регистрируем сервис, который должен помнить состояние на время обработки входящего потока          
            //services.AddSingleton<>(); так регистрируем сервис, хранящий состояние на все время жизни приложения 

            //services.AddMvc(opt => opt.Conventions.Add(new TestControllerModelConvention()));        

            services
                 .AddControllersWithViews(/*opt => opt.Conventions.Add(new TestControllerModelConvention())*/)
                 .AddRazorRuntimeCompilation();
        }
       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env /*, IServiceProvider services*/)
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();

            app.UseRouting();

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
