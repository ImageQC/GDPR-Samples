using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Gdpr.UI.WebApp.Pages.Shared;
using MxReturnCode;
using Microsoft.Extensions.Logging;
using Gdpr.UI.WebApp.Services;

namespace Gdpr.UI.WebApp.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : BasePageModel
    {
        private readonly ILogger<ErrorModel> _logger;
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int? statusCode=null)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("Error.OnGet()");

            if (statusCode.HasValue == false)
            {
                var feature = this.HttpContext.Features?.Get<IExceptionHandlerFeature>();
                rc.SetError(3010101, MxError.Source.Exception, feature?.Error?.Message ?? "[not set]", MxMsgs.MxErrUnknownException, true);
                SetPageStatusMsg(rc.GetErrorUserMsgHtml(Startup.WebAppName, WebErrorHandling.GetMxRcReportToEmailBody()), ExistingMsg.Overwrite);
                _logger.LogError(rc.GetErrorTechMsg());
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
