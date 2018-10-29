using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MxReturnCode;

namespace Gdpr.UI.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MxUserMsg.Init(Assembly.GetExecutingAssembly(), MxMsgs.SupportedCultures);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static string GetVersion()
        {
            string rc = "[not set}";

            var ver = typeof(Program)?.GetTypeInfo()?.Assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
            if (ver != null)
                rc = ver;

            return rc;
        }
    }
}
