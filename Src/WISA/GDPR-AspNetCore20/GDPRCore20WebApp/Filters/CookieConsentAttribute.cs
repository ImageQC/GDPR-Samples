using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;


namespace GDPRCore20WebApp.Filters
{
    /*
    * ASP.NET ActionFilterAttribute to help implement EU Cookie-law
    * Derived from work of Maarten Sikkema, Macaw Nederland BV (MIT Licence) 
    * https://www.macaw.nl/inspiratie/blogs/implementing-european-cookie-law-compliance-in-asp-net-mvc> 
    */

    public class CookieConsentAttribute : ActionFilterAttribute
    {
        public const string CONSENT_COOKIE_NAME = "MxConsentCookie";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext != null)
            {
                HttpRequest request = filterContext?.HttpContext?.Request;
                var controller = filterContext?.Controller as Controller;
                var viewBag = controller?.ViewBag;

                if ((controller != null) && (request != null) && (viewBag != null))
                {
                    viewBag.AskCookieConsent = true;
                    viewBag.HasCookieConsent = false;

                    try
                    {
                        var consentCookie = request.Cookies[CONSENT_COOKIE_NAME];
                        if (consentCookie == null)
                        {               // No consent cookie. We first check the Do Not Track header value, this can have the value "0" or "1"
                            string dnt = request.Headers["DNT"];
                            if (!String.IsNullOrEmpty(dnt))
                            {                                           // If we receive a DNT header, we accept its value and do not ask the user anymore
                                viewBag.AskCookieConsent = false;
                                if (dnt == "0")
                                    viewBag.HasCookieConsent = true;
                            }
                            else
                            {
                                if (IsSearchCrawler(request.Headers["User-Agent"]))
                                    viewBag.AskCookieConsent = false;   // don't ask consent from search engines, also don't set cookies
                                else
                                {                                       // first request on the site and no DNT header.
                                    CookieOptions option = new CookieOptions
                                    {
                                        Expires = DateTime.UtcNow.AddMonths(6)   //set expired to ensure it is not a session cookie
                                    };
                                    filterContext?.HttpContext?.Response?.Cookies?.Append(CONSENT_COOKIE_NAME, "asked", option);
                                }
                            }
                        }
                        else
                        {
                            viewBag.AskCookieConsent = false;
                            if (consentCookie == "asked")
                            {           // consent is implicitly given
                                consentCookie = "true";
                                CookieOptions option = new CookieOptions
                                {
                                    Expires = DateTime.UtcNow.AddMonths(6)
                                };
                                if (filterContext?.HttpContext?.Response?.Cookies != null)
                                {
                                    filterContext.HttpContext.Response.Cookies.Append(CONSENT_COOKIE_NAME, consentCookie, option);
                                    viewBag.HasCookieConsent = true;
                                }
                            }
                            else if (consentCookie == "true")
                            {
                                viewBag.HasCookieConsent = true;
                            }
                            else
                            {
                                viewBag.HasCookieConsent = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        var msg = e.Message;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }


        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var controller = filterContext?.Controller as Controller;
        //    var viewBag = controller?.ViewBag;

        //    var cookie = filterContext?.HttpContext?.Request?.Cookies["MyConsentCookie"];
        //    if (cookie == null)
        //    {
        //        viewBag.AskCookieConsent = true;
        //        viewBag.HasCookieConsent = false;

        //        CookieOptions option = new CookieOptions
        //        {
        //            Expires = DateTime.UtcNow.AddDays(5)
        //        };
        //        filterContext?.HttpContext?.Response?.Cookies?.Append("MyConsentCookie", "set", option);
        //    }
        //    else
        //    {
        //        viewBag.AskCookieConsent = false;
        //        viewBag.HasCookieConsent = true;
        //    }
        //    base.OnActionExecuting(filterContext);
        //}
        private bool IsSearchCrawler(string userAgent)
        {
            if (!String.IsNullOrEmpty(userAgent))
            {
                string[] crawlers = new string[]
                {
                    "Baiduspider",
                    "Googlebot",
                    "YandexBot",
                    "YandexImages",
                    "bingbot",
                    "msnbot",
                    "Vagabondo",
                    "SeznamBot",
                    "ia_archiver",
                    "AcoonBot",
                    "Yahoo! Slurp",
                    "AhrefsBot"
                };
                foreach (string crawler in crawlers)
                    if (userAgent.Contains(crawler))
                        return true;
            }
            return false;
        }
    }
}
