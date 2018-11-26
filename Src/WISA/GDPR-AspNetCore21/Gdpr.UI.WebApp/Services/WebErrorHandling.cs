using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gdpr.UI.WebApp.Services
{
    public static class WebErrorHandling
    {
        public static readonly string FormValidationErrorPreamble = "Form is incorrect. Please fix the problems and try again.";
        public static string GetModelStateErrors(ModelStateDictionary modelState, string preamble= null, string action = null)
        {
            var rc = "";
            if (preamble != null)
                rc += preamble + "<br />";

            var details = "";
            bool errDetailFound = false;
            foreach (var key in modelState.Keys)
            {
                if (key != null)
                {
                    if (modelState.TryGetValue(key, out ModelStateEntry item))
                    {
                        if (item != null)
                        {
                            var errCnt = 0;
                            foreach (ModelError err in item.Errors)
                            {
                                if (errCnt == 0)
                                    details += "<li>" + key + ": ";
                                if (errCnt + 1 < item.Errors.Count())
                                    details += err.ErrorMessage + ", ";
                                else
                                    details += err.ErrorMessage + "</li>";
                                errCnt++;
                                errDetailFound = true;
                            }
                        }
                    }
                }
            }
            if (errDetailFound == false)
                rc += "no error details. ";
            else
                rc += "<ul>" + details + "</ul>";

            if (action != null)
                rc += "<br />" + action;
            return rc;
        }
        public static IdentityResult CreateIdentityFailResult(int errorNum, string errorMsg)
        {
            List<IdentityError> errors = new List<IdentityError>() { new IdentityError() { Description = errorMsg, Code = errorNum.ToString() } };
            return IdentityResult.Failed(errors.ToArray());
        }
        public static string GetIdentityErrors(IdentityResult res, string preamble=null, string action=null)
        {
            var rc = "";
            if (preamble != null)
                rc += preamble + "<br />"; 

            var details = "";
            var errCnt = 0;
            foreach (var error in res.Errors)
            {
                if (errCnt == 0)
                    details += "<li>";
                if (errCnt + 1 < res.Errors.Count())
                    details += error.Description + ", ";
                else
                    details += error.Description + "</li>";
                errCnt++;
            }
            if (errCnt == 0)
                rc += "no error details. ";
            else
                rc += "<ul>" + details + "</ul>";

            if (action != null)
                rc += "<br />" + action;

            return rc;
        }
        public static string GetMxRcReportToEmailBody(string additionalinfo = null) //scheduled to be included in next release of MxReturnCode
        {
            var rc = "";

            rc += "I encountered the following problem whilst using your software. Please advise." + Environment.NewLine;
            rc += Environment.NewLine;
            if (additionalinfo != null)
            {
                rc += Environment.NewLine;
                rc += additionalinfo + Environment.NewLine;
                rc += Environment.NewLine;
                rc += Environment.NewLine;
            }
            rc += "[please add your own details or comments here]" + Environment.NewLine + Environment.NewLine;
            rc += "Thanks" + Environment.NewLine + Environment.NewLine;

            return rc;
        }
    }
}
