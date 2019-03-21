using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gdpr.UI.WebApp.Pages.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MxReturnCode;

namespace Gdpr.UI.WebApp.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var LoggedInUser = await _userManager.GetUserAsync(User);
            var msg = $"{LoggedInUser?.UserName ?? "nobody"} is logged-in. ";
            if (LoggedInUser?.EmailConfirmed == false)
                msg += "Check your emails to complete your registration";
            SetPageStatusMsg(msg, ExistingMsg.Overwrite);
            return Page();
        }

    }
}
