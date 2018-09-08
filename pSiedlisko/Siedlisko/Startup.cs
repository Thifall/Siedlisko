using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Siedlisko.Models;
using Siedlisko.Models.Helper;
using Siedlisko.Models.Interfaces;
using System;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Siedlisko.Reservations;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Siedlisko.ViewModels;

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
                //config.ForwardWindowsAuthentication = false;
            });
            //My Services
            services.AddSingleton(_config);
            services.AddTransient<PriceCounter>();
            services.AddDbContext<SiedliskoContext>();
            services.AddTransient<SiedliskoDataSeeder>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<Reserver>();
            // Add framework services.
            services.AddIdentity<SiedliskoUser, IdentityRole>(config =>
            {
                config.Password.RequireUppercase = false;
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Password.RequireNonAlphanumeric = false;
               
            }).AddEntityFrameworkStores<SiedliskoContext>();
            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/Login";
                config.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/Api") && ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else if (ctx.Response.StatusCode != 401)
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        await Task.Yield();
                    }
                };
            });
            services.AddMvc().AddJsonOptions(config => config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SiedliskoDataSeeder seeder)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<SiedliskoUser, AccountDetailsViewModel>();
                config.CreateMap<Reservation, CreateReservationViewModel>().ReverseMap();
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

            app.UseAuthentication();

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
