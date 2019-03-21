using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gdpr.UI.WebApp.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gdpr.UI.WebApp.Pages
{
    public class PrivacyModel : BasePageModel
    {
        public const string ConsentCookieName = ".AspNet.Consent";
        public const int ConsentCookieExpiryDays = 365;
        public const string EssentialCookieList = "ARRAffinity;.AspNetCore.Antiforgery;.AspNetCore.Identity.Application;.AspNetCore.Mvc.CookieTempDataProvider;";
        public const char CookieListDelimiter = ';';  //see https://stackoverflow.com/questions/1969232/allowed-characters-in-cookies

        public List<string> NonEssentialCookies { get; private set; }
        public List<string> EssentialCookies { get; private set; }
        [BindProperty]
        public string CookieList { get; private set; }
        public void OnGet()
        {
            SetModel();
        }

        public void OnPost(string cookieList)
        {
            var deleteList = new List<string>();
            var cookies = cookieList?.Split(PrivacyModel.CookieListDelimiter, StringSplitOptions.RemoveEmptyEntries);
            if (cookies != null)
            {
                foreach (var name in cookies)
                {
                    var check = Request.Form[name];
                    if (check == "on")
                    {
                        HttpContext?.Response?.Cookies?.Delete(name);
                        deleteList.Add(name);
                    }
                }
            }
            SetModel(deleteList);
        }

        private void SetModel(ICollection<string> deleteList = null)
        {
            EssentialCookies = new List<string>();
            NonEssentialCookies = new List<string>();

            var cookies = HttpContext?.Request?.Cookies;
            if (cookies != null)
            {
                foreach (var cookie in cookies)
                {
                    if ((deleteList == null) || (deleteList.Contains(cookie.Key) == false))
                    {
                        if (IsEssentialCookie(cookie.Key))
                            EssentialCookies.Add(cookie.Key);
                        else
                            NonEssentialCookies.Add(cookie.Key);
                    }
                }
            }
            var list = "";
            foreach (var cookie in NonEssentialCookies)
                list += cookie + PrivacyModel.CookieListDelimiter;
            CookieList = list;
        }

        private bool IsEssentialCookie(string key)
        {
            bool rc = false;

            var essentials = EssentialCookieList.Split(PrivacyModel.CookieListDelimiter, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cookie in essentials)
            {
                if (key.Contains(cookie))
                {
                    rc = true;
                    break;
                }
            }
            return rc;
        }

    }
}