using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gdpr.Domain;
using Gdpr.UI.WebApp.Pages.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MxReturnCode;

namespace Gdpr.UI.WebApp.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public string DatabaseStatus { get; set; }

        public IndexModel(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            MxReturnCode<IActionResult> rc = new MxReturnCode<IActionResult>("Index.OnGetAsync()", Page());

            var userID = "[nobody logged-in]";
            var msg = "unknown error";
            try
            {
                var loggedInUser = await _userManager.GetUserAsync(User);
                userID = loggedInUser?.Id ?? "[nobody logged-in]";
                msg = $"{loggedInUser?.UserName ?? "nobody"} is logged-in. ";
                if (loggedInUser?.EmailConfirmed == false)
                    msg += "Check your emails to complete your registration";

                using (IAdminRepository repository = new AdminRepository(_config?.GetConnectionString("DefaultConnection")))
                {
                    var resCnt = await repository.GetRoleCountAsync();
                    rc += resCnt;
                    if (rc.IsError())
                        DatabaseStatus = "Database access failed";
                    else
                    {
                        DatabaseStatus = $"Database access ok, Role Count = {resCnt.GetResult()}";
                        rc.SetResult(Page());
                    }
                }
            }
            catch (Exception e)
            {
                rc.SetError(3040101, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
            }
            if (rc.IsError())
                SetPageStatusMsg(rc.GetErrorUserMsgHtml(userID), ExistingMsg.Overwrite);
            else
                SetPageStatusMsg(msg, ExistingMsg.Overwrite);

            return rc.GetResult();
        }
    }
}
