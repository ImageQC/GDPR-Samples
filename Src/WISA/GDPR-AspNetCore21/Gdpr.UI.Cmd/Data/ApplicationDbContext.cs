using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Gdpr.UI.Cmd.Data
{
    class ApplicationDbContext :  IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                 : base(options)
        { }
    }

    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        ApplicationDbContext IDesignTimeDbContextFactory<ApplicationDbContext>.CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("local.settings.json", optional: false);

            var configuration = builder.Build();

            var conn = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer<ApplicationDbContext>(conn);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
