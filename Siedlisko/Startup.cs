using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Extensions;
using Siedlisko.Models;
using Siedlisko.Models.Helper;
using Siedlisko.Models.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AutoMapper;
using Siedlisko.ViewModels;
using System;

namespace Siedlisko
{
    public class Startup
    {
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
          
            _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISOptions>(config =>
            {
                config.AutomaticAuthentication = false;
                config.ForwardClientCertificate = true;
                config.ForwardWindowsAuthentication = false;
            });

            services.AddSingleton(_config);
            // Add framework services.
            services.AddTransient<PriceCounter>();
            services.AddDbContext<SiedliskoContext>();
            services.AddTransient<SiedliskoDataSeeder>();
            services.AddScoped<IRepository, Repository>();
            services.AddIdentity<SiedliskoUser, IdentityRole>(config =>
            {
                config.Password.RequireUppercase = false;
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Password.RequireNonAlphanumeric = false;
                config.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
            }).AddEntityFrameworkStores<SiedliskoContext>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SiedliskoDataSeeder seeder)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<SiedliskoUser, AccountDetailsViewModel>();
            });
            
            loggerFactory.AddFile(string.Format("Logs/{0}_Siedlisko.log", DateTime.Now.ToString("dd-MM-yyyy")));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            seeder.SeedData().Wait();
        }
    }
}
