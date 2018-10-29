using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using MxReturnCode;
using Gdpr.Domain;
using System.IO;

namespace Gdpr.UI.Cmd
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            MxUserMsg.Init(Assembly.GetExecutingAssembly(), MxMsgs.SupportedCultures);

            MxReturnCode<int> rc = new MxReturnCode<int>(string.Format("{0} v{1}", "ReturnCodeApp()", GetVersion()), 1, "defects@ovaryvis.org");

            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                   .AddJsonFile("local.settings.json")
                                                   .Build();
            if (config?.GetSection("ConnectionStrings")?["DefaultConnection"] == null)
                rc.SetError(2010101, MxError.Source.AppSetting, "invalid configuration");
            else
            {
                var conn = config["ConnectionStrings:DefaultConnection"];
                using (IAdminRepository repository = new AdminRepository(conn))
                {
                    rc += await repository.GetRoleCountAsync();
                }
                if (rc.IsSuccess())
                {
                    Console.WriteLine(@"URD Count = {0}", rc.GetResult());
                }

            }
            if (rc.IsError())
            {
                Console.WriteLine(rc.GetErrorUserMsg());
                Console.WriteLine(rc.GetErrorTechMsg());
            }
            else
                Console.WriteLine("ends ok");

            return rc.IsSuccess() ? 0 : -1;
        }

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
