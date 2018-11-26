using System;

using MxReturnCode;

using Gdpr.UI.WebApp.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Gdpr.UI.WebApp.Pages
{
    public class CookiesModel : BasePageModel
    {
        public IActionResult OnGet()
        {
            SetPageStatusMsg("Information about our cookie policy", ExistingMsg.Overwrite);

            return Page();
        }
    }
}