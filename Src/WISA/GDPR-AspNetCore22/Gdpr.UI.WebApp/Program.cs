using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gdpr.Domain;
using Gdpr.UI.WebApp.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MxReturnCode;

namespace Gdpr.UI.WebApp
{
    public class Program
    {
        static async Task<int> Main(string[] args)  //03-12-18 made async and returns int not void
        {
            MxReturnCode<int> rc = new MxReturnCode<int>("Main()", -1);

            try
            {
                IWebHost host = CreateWebHostBuilder(args).Build();      //calls Startup.ConfigureServices()
                using (var scope = host?.Services?.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    if (services == null)
                        rc.SetError(3050101, MxError.Source.Sys, "scope.ServiceProvider is null");
                    else
                    {
                        var mxIdentitySeedDb = services.GetRequiredService<IMxIdentitySeedDb>(); 
                        if (mxIdentitySeedDb == null)
                            rc.SetError(3050102, MxError.Source.Sys, "mxIdentityDb is null");
                        else
                        {
                            rc = await mxIdentitySeedDb.SetupAsync();
                            if (rc.IsSuccess(true))
                            {
                                host.Run(); //calls Startup.Configure()
                                rc.SetResult(0); //success - webapp has completed
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                rc.SetError(3050103, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
            }
            return rc.GetResult();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
