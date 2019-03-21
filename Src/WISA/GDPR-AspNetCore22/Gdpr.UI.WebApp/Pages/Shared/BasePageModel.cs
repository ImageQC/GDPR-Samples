using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gdpr.UI.WebApp.Pages.Shared
{
    public class BasePageModel : PageModel
    {
        public string Tip { get; protected set; }

        protected enum ExistingMsg { Append, Overwrite, Keep };
        [TempData]
        public string StatusMessage { get; set; }           //for Identity/Account/Manage pages

        //see _StatusPage.cshtml
        //colour of message bar depends on start of msg text (case insensitive): Error (danger), Warning (warning), Info (info), [default] (success)
        //actions:
        //   Append - add as new line to any existing message
        //   Overwrite - any existing message is overwritten
        //   Keep - any existing message is retained, so msg is not set (useful if msg is just some sort of greeting)
        protected string SetPageStatusMsg(string msg, ExistingMsg action = ExistingMsg.Append)  //for standard pages 
        {
            if (action == ExistingMsg.Overwrite)
                StatusMessage = msg;
            else
            {
                if (string.IsNullOrWhiteSpace(StatusMessage))
                    StatusMessage = msg;
                else
                {
                    if ((action != ExistingMsg.Keep) && (string.IsNullOrWhiteSpace(msg) == false))
                        StatusMessage += "<br />" + msg;
                }
            }
            return TempData?.Peek("StatusMessage")?.ToString();
        }
        protected bool RemovePageStatusMsg(string match)
        {
            bool rc = false;

            if (match == null)
            {
                StatusMessage = null;
                rc = true;
            }
            else if (TempData?.Peek("StatusMessage")?.ToString().Contains(match) == true)
            {
                StatusMessage = null;
                rc = true;
            }
            else
            {
                rc = false;
            }
            return rc;
        }

    }
}
