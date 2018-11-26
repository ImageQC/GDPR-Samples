using Gdpr.UI.WebApp.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Gdpr.UI.WebApp.Pages
{
    public class ContactModel : BasePageModel
    {
        public IActionResult OnGet()
        {
            SetPageStatusMsg("Information about how to contact us", ExistingMsg.Overwrite);

            return Page();
        }
    }
}
