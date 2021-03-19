using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using IdentityWorkShop.Identity.Introduction.IdentityOptionsValidations;
using IdentityWorkShop.Identity.Introduction.Mapping;
using IdentityWorkShop.Identity.Introduction.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityWorkShop.Identity.Introduction
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<AppIdentityDbContext>(opts =>
            {
                opts.UseSqlServer(_configuration["ConnectionStrings:MainComputerConnectionString"]);
            });

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddPasswordValidator<IdentityPasswordVadlidator>()
                .AddUserValidator<IdentityUserValidator>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>();

            services.Configure<IdentityOptions>(options =>
            {
                //Password Kurallar�
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;

                //Lockout Kurallar�
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                ////User Kurallar�
                options.User.AllowedUserNameCharacters= "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";// T�rk�e karakterlerin user isimlendirmede kullan�lmas�n� sa�layabiliriz.
                options.User.RequireUniqueEmail = true;// Bu komut her kullan�c�n�n farkl� bir emaili olmas�n� denetler.

                ////SignIn Kurallar�
                //options.SignIn.RequireConfirmedEmail = true;//Kullan�c�n�n email do�rulamas� i�lemini denetler.
                //options.SignIn.RequireConfirmedPhoneNumber = false;// Kullan�c�n�n girmi� oldu�u telefon numaras�ndan bir do�rulama al�p almayaca��m�z� kontrol eder.
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Home/Login");
                //options.LogoutPath = "/account/logout";
                //options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".MyIdentity.Cookie",
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
