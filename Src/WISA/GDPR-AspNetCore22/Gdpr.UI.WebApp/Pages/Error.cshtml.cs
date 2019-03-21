using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Gdpr.UI.WebApp.Pages.Shared;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MxReturnCode;

namespace Gdpr.UI.WebApp.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : BasePageModel
    {
        public void OnGet(int? statusCode = null)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("Error.OnGet()");

            if (statusCode.HasValue == false)
            {
                var feature = this.HttpContext.Features?.Get<IExceptionHandlerFeature>();
                rc.SetError(3030101, MxError.Source.Exception, feature?.Error?.Message ?? "[not set]", MxMsgs.MxErrUnknownException, true);
                SetPageStatusMsg(rc.GetErrorUserMsgHtml(), ExistingMsg.Overwrite);
            }
            else
            {
                if (statusCode.Value == 404) //Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound
                    SetPageStatusMsg("Error: Page not found. Correct the URL in your browser's address bar and try again", ExistingMsg.Overwrite);
                else
                    SetPageStatusMsg($"Error: Invalid request; status={statusCode}. Correct the URL in your browser's address bar and try again", ExistingMsg.Overwrite);
                rc.SetResult(true);
            }
        }
    }
}
