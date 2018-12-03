using System;
using System.Reflection;
using System.Threading.Tasks;
using Gdpr.UI.WebApp.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MxReturnCode;

namespace Gdpr.UI.WebApp
{
    public class Program
    {
        static async Task<int> Main(string[] args)  //03-12-18 made async and returns int not void
        {
            MxUserMsg.Init(Assembly.GetExecutingAssembly(), MxMsgs.SupportedCultures);
            MxReturnCode<int> rc = new MxReturnCode<int>("Main()", -1, "admin@imageqc.com");

            try   //03-12-18
            {
                IWebHost host = CreateWebHostBuilder(args).Build();      //calls Startup.ConfigureServices()
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    if (services == null)
                        rc.SetError(3130101, MxError.Source.Sys, "scope.ServiceProvider is null");
                    else
                    {
                        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        var config = services.GetRequiredService<IConfiguration>();
                        if ((userManager == null) || (roleManager == null) || (config == null))
                            rc.SetError(3130102, MxError.Source.Sys, "userManager or roleManager or config is null");
                        else
                        {
                            var result = await SeedDb.EnsureDataPresentAsync(config, userManager, roleManager);
                            rc += result;
                            if (result.IsSuccess())
                            {
                                host.Run();         //calls Startup.Configure()
                                rc.SetResult(0);    //success - webapp has completed
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                rc.SetError(3010103, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
            }
            return rc.GetResult();  //03-12-18
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
