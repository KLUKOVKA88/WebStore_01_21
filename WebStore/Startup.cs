﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Infrastructure.Middleware;
using WebStore.Infrastructure.Conventions;

namespace WebStore
{
    public record Startup(IConfiguration Configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt => opt.Conventions.Add(new TestControllerModelConvention()));

           services
                .AddControllersWithViews(/*opt => opt.Conventions.Add(new TestControllerModelConvention())*/)
                .AddRazorRuntimeCompilation();
        }
       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
                //Проекция запроса на действие
                endpoints.MapGet("/greetings", async context =>
                {
                    //await context.Response.WriteAsync(greetings);
                    await context.Response.WriteAsync(Configuration["Greetings"]);
                });

                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                // htpp:localhost:5000 -> controller = "Home" action = "Index" параметр  = null
                // htpp:localhost:5000/Catalog/Products/5 controller = "Catalog" action = "Products" параметр  = 5
            });
        }
    }
}
