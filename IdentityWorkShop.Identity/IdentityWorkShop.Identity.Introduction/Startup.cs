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
                //Password Kurallarý
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;

                //Lockout Kurallarý
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                ////User Kurallarý
                options.User.AllowedUserNameCharacters= "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";// Türkçe karakterlerin user isimlendirmede kullanýlmasýný saðlayabiliriz.
                options.User.RequireUniqueEmail = true;// Bu komut her kullanýcýnýn farklý bir emaili olmasýný denetler.

                ////SignIn Kurallarý
                //options.SignIn.RequireConfirmedEmail = true;//Kullanýcýnýn email doðrulamasý iþlemini denetler.
                //options.SignIn.RequireConfirmedPhoneNumber = false;// Kullanýcýnýn girmiþ olduðu telefon numarasýndan bir doðrulama alýp almayacaðýmýzý kontrol eder.
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
