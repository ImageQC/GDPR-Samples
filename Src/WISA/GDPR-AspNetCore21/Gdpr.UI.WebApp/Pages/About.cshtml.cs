
using System.Diagnostics;
using System.Reflection;

using MxReturnCode;
using Gdpr.UI.WebApp.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Gdpr.UI.WebApp.Pages
{
    public class AboutModel : BasePageModel
    {
        public readonly string Company = "[not set]";
        public readonly string Product = "[not set]";
        public readonly string Copyright = "[not set]";
        public readonly string Description = "[not set]";

        public AboutModel()
        {
            var info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            if (info != null)
            {
                Company = info.CompanyName ?? "[null]";
                Product = info.ProductName ?? "[null]";
                Copyright = info.LegalCopyright ?? "[null]";
                Description = info.Comments ?? "[null]";
            }
        }
        public IActionResult OnGet()
        {
            SetPageStatusMsg("Information about this website", ExistingMsg.Overwrite);

            return Page();
        }
    }
}
