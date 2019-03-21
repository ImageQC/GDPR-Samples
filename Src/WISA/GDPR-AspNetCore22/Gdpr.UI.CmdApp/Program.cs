using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using MxReturnCode;

namespace Gdpr.UI.CmdApp
{
    class Program
    {
        public static readonly String WebAppVersion = typeof(Program)?.GetTypeInfo()?.Assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        public static readonly string WebAppName = typeof(Program)?.GetTypeInfo()?.Assembly?.GetName().Name ?? "[not set]";

        static int Main(string[] args)
        {
            MxReturnCode<int> rc = new MxReturnCode<int>($"{Program.WebAppName} v{Program.WebAppVersion}", 1);

            rc.Init(Assembly.GetExecutingAssembly(), "admin@imageqc.com", null, null, null, MxMsgs.SupportedCultures);

            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json")
                .Build();
            var conn = config?["ConnectionStrings:DefaultConnection"];  //03-12-18
            if (conn == null)
                rc.SetError(2010101, MxError.Source.AppSetting, "config not built or ConnectionStrings:DefaultConnection not found");
            else
            {
                rc.SetResult(0);
            }

            Console.WriteLine(rc.GetInvokeDetails());
            Console.WriteLine(rc.IsError() ? rc.GetErrorUserMsg() : $"Hello World!");
            Console.WriteLine(rc.IsError() ? rc.GetErrorTechMsg(): "no error");

            return rc.GetResult();
        }
    }
}
