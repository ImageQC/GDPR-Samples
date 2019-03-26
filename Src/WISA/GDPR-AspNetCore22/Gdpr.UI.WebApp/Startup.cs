using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Gdpr.Domain;
using Gdpr.UI.WebApp.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gdpr.UI.WebApp.Data;
using Gdpr.UI.WebApp.Pages;
using Gdpr.UI.WebApp.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PaulMiami.AspNetCore.Mvc.Recaptcha;

using MxReturnCode;

namespace Gdpr.UI.WebApp
{
    public class Startup
    {
        public static readonly String WebAppVersion = typeof(Startup)?.GetTypeInfo()?.Assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        public static readonly string WebAppName = typeof(Startup)?.GetTypeInfo()?.Assembly?.GetName().Name ?? "[not set]";

        private bool _isDevelopment = false;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _isDevelopment = env.IsDevelopment() ? true : false;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>($"{Startup.WebAppName} v{Startup.WebAppVersion}");

            rc.Init(Assembly.GetExecutingAssembly(), "admin@imageqc.com", null,
                Configuration?.GetConnectionString("AzureWebJobsServiceBus"), Configuration?["MxLogMsg:AzureServiceBusQueueName"], MxMsgs.SupportedCultures);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.ConsentCookie = new CookieBuilder
                {
                    Name = PrivacyModel.ConsentCookieName,
                    Expiration = new TimeSpan(PrivacyModel.ConsentCookieExpiryDays, 0, 0, 0),
                    IsEssential = true
                };  //ConsentCookie needs to be marked essential - see support 119030623000476 https://docs.microsoft.com/en-us/aspnet/core/security/gdpr?view=aspnetcore-2.2  
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()                   //add support for roles - https://github.com/aspnet/Identity/issues/1884
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false; //was true
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = MxIdentityCommon.EmailAllowedChars;
                //at registration must allow same characters as for email, but when changing username restrict
                //to MxIdentityCommon.NewUsernameAllowedChars - "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true; //was false
                options.SignIn.RequireConfirmedEmail = true; //added
            });

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

            services.AddMvc(options =>
            {
                options.SslPort = _isDevelopment ? 44357 : 443;
                options.Filters.Add(new RequireHttpsAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IMxIdentityDb, MxIdentityDb>();

            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<ServiceConfig>(Configuration.GetSection("ServiceConfig"));

            var recapt = new RecaptchaOptions
            {
                SiteKey = Configuration["ServiceConfig:reCaptchaSiteKey"] ?? "MissingRecaptchaSiteKey",
                SecretKey = Configuration["ServiceConfig:reCaptchaSecretKey"] ?? "MissingRecaptchaSecretKey"
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
