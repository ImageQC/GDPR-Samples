using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Gdpr.UI.Web.Models;
using Gdpr.Domain;

namespace Gdpr.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        public async Task<IActionResult> Test()
        {
            var conn = _config.GetConnectionString("DefaultConnection");
            using (IAdminRepository repository = new AdminRepository(conn))
            { 
                var resCnt = await repository.GetRoleCountAsync();
                ViewData["Message"] = String.Format("URD Count = {0}", resCnt.GetResult());
            }
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
