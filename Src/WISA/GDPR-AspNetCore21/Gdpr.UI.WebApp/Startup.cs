using System;
using System.Reflection;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PaulMiami.AspNetCore.Mvc.Recaptcha;

using MxReturnCode;

using Gdpr.Domain;
using Gdpr.UI.WebApp.Services;
using Gdpr.UI.WebApp.Data;
using Gdpr.UI.WebApp.Pages.Shared;

namespace Gdpr.UI.WebApp
{
    public class Startup
    {
        public static readonly string WebAppVersion = typeof(Startup)?.GetTypeInfo()?.Assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "[not set]";
        public static readonly string WebAppName = typeof(Startup)?.GetTypeInfo()?.Assembly?.GetName().Name ?? "[not set]";
        public static readonly string DomainVersion = typeof(AdminRepository)?.GetTypeInfo()?.Assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "[not set]";
        public static readonly string DomainName = typeof(AdminRepository)?.GetTypeInfo()?.Assembly?.GetName().Name ?? "[not set]";
        public static readonly string Copyright = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()?.Location)?.LegalCopyright ?? "[not set]";


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

                    //03-12-18 added support for Roles
            services.AddDefaultIdentity<IdentityUser>()     //https://github.com/aspnet/Identity/issues/1884
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddIdentity<IdentityUser, IdentityRole>()
            //        .AddEntityFrameworkStores<ApplicationDbContext>()
            //        .AddDefaultTokenProviders();
            
            services.AddAuthentication()
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                    microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                })
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;        //was true
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;                 //was false
                
                options.SignIn.RequireConfirmedEmail = true;            //added
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<ServiceConfig>(Configuration.GetSection("ServiceConfig"));

            var recapt = new RecaptchaOptions
            {
                SiteKey = Configuration["ServiceConfig:ReCaptchaSiteKey"] ?? "MissingRecaptchaSiteKey",
                SecretKey = Configuration["ServiceConfig:ReCaptchaSecretKey"] ?? "MissingRecaptchaSecretKey"
            };
            services.AddRecaptcha(recapt);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
