using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace Gdpr.UI.WebApp.Pages.Shared
{
    public class BasePageModel : PageModel
    {
        public string Tip { get; set; }

        public enum ExistingMsg { Append, Overwrite, Keep};
        [TempData]
        public string StatusMessage { get; set; }           //for Identity/Account/Manage pages

        //see _StatusPage.cshtml
        //colour of message bar depends on start of msg text (case insensitive): Error (danger), Warning (warning), Info (info), [default] (success)
        //actions:
        //   Append - add as new line to any existing message
        //   Overwrite - any existing message is overwritten
        //   Keep - any existing message is retained, so msg is not set (useful if msg is just some sort of greeting)
        protected string SetPageStatusMsg(string msg, ExistingMsg action=ExistingMsg.Append)  //for standard pages 
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
            return TempData.Peek("StatusMessage")?.ToString();
        }
        protected bool RemovePageStatusMsg(string match)
        {
            bool rc = false;

            if (match == null)
            {
                StatusMessage = null;
                rc = true;
            }
            else  if (TempData.Peek("StatusMessage")?.ToString().Contains(match) == true)
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



        //public string SetAccountManagePageStatusMsg(string msg, ExistingMsg action=ExistingMsg.Append) //for Identity/Account/Manage pages
        //{
        //    if (action == ExistingMsg.Overwrite)
        //        StatusMessage = msg;
        //    else
        //    {
        //        if (string.IsNullOrWhiteSpace(StatusMessage))
        //            StatusMessage = msg;
        //        else
        //        {
        //            if ((action != ExistingMsg.Keep) && (string.IsNullOrWhiteSpace(msg) == false))
        //                StatusMessage += "<br />" + msg;
        //        }
        //    }
        //    return (string)TempData.Peek("StatusMessage");
        //}

        //[TempData]
        //public string StatusMessage { get; set; }           //for Identity/Account/Manage pages
        //public string StatusMsg { get; private set; }       //for standard pages (set from TempData["StatusLineMsg"])


        //protected string SetPageStatusLineMsg(string msg)  //for standard pages 
        //{
        //    string rc = SetPassedStatusLineMsg();
        //    if (rc == null)
        //        rc = ((string.IsNullOrEmpty(msg) == false) ? msg : null);
        //    else
        //        rc += "<br />" + ((string.IsNullOrEmpty(msg) == false) ? msg : null);

        //    StatusMsg = rc;

        //    return rc;
        //}
        //
        //public void PassStatusLineMsgToPage(string msg)    //for standard pages
        //{
        //    TempData["StatusLineMsg"] = msg;
        //}
        //
        //private string SetPassedStatusLineMsg()
        //{
        //    string rc = null;
        //    var tmp = (string)TempData["StatusLineMsg"];
        //    if (string.IsNullOrEmpty(tmp) == false)
        //    {
        //        StatusMsg = tmp;
        //        rc = tmp;
        //    }
        //    return rc;
        //}
        //
        //public bool SetAccountManagePageStatusMsg(string msg, bool overwrite = true) //for Identity/Account/Manage pages
        //{
        //    bool rc = false;
        //    if (overwrite == true)
        //    {
        //        StatusMessage = msg;
        //        rc = true;
        //    }
        //    else
        //    {
        //        if (StatusMessage == null)
        //        {
        //            StatusMessage = msg;
        //            rc = true;
        //        }
        //    }
        //    return rc;
        //}
    }
}
