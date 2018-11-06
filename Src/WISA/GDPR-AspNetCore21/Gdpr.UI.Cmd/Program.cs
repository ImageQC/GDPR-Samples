using System;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Configuration;

using MxReturnCode;

using Gdpr.Domain;


namespace Gdpr.UI.Cmd
{
    class Program
    {

        static async Task<int> Main(string[] args) //Uses Dapper/Gdpr.Domain Repository classes to access the database
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

//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Gdpr.UI.Cmd.Data;

        //public static void Main()  //Uses EF to access the database
        //{
        //    try
        //    {
        //        var builder = new ConfigurationBuilder();
        //        builder.AddJsonFile("local.settings.json", optional: false);

        //        var configuration = builder.Build();
        //        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        //        optionsBuilder.UseSqlServer<ApplicationDbContext>(configuration.GetConnectionString("DefaultConnection"));

        //        using (var db = new ApplicationDbContext(optionsBuilder.Options))
        //        {
        //            db.Users.Add(new IdentityUser { UserName = String.Format("wills-{0}", Guid.NewGuid().ToString()) });

        //            var count = db.SaveChanges();

        //            Console.WriteLine("{0} records saved to database", count);
        //            Console.WriteLine("All users in database:");

        //            foreach (var user in db.Users)
        //            {
        //                Console.WriteLine(" - {0}", user.UserName);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //}
    }
}
