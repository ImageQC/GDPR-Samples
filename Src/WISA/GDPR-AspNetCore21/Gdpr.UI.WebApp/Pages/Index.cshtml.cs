using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

using Gdpr.Domain;

namespace Gdpr.UI.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        public string Status { get; set; }
        public string URDCount { get; set; }

        public IndexModel(IConfiguration config)
        {
           _config = config;
        }

        public async Task OnGet()
        {
            URDCount = "Failed";
            var conn = _config.GetConnectionString("DefaultConnection");
            using (IAdminRepository repository = new AdminRepository(conn))
            {
                var resCnt = await repository.GetRoleCountAsync();
                if (resCnt.IsError())
                    Status = resCnt.GetErrorUserMsg();
                else
                {
                    Status = "Success";
                    URDCount = String.Format("URD Count = {0}", resCnt.GetResult());
                }
            }
        }
    }
}
