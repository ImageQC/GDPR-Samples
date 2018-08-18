using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GDPRCore20WebApp.Filters;


namespace GDPRCore20WebApp.Services
{
    public static class CookieConsent
    {
        public static void SetCookieConsent(HttpResponse response, bool consent)
        {
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(1)
            };
            response.Cookies.Append(CookieConsentAttribute.CONSENT_COOKIE_NAME, consent ? "true" : "false", option);
        }

        public static void RemoveConsentCookie(HttpResponse response, Controller controller)
        {
            var viewBag = controller.ViewBag;
            if (viewBag != null)
            {
                viewBag.AskCookieConsent = false;
                viewBag.HasCookieConsent = false;
                viewBag.CookieConsentFound = false;
            }
            response.Cookies.Delete(CookieConsentAttribute.CONSENT_COOKIE_NAME);
        }

        public static bool AskCookieConsent(ViewContext context)
        {
            return context.ViewBag.AskCookieConsent ?? false;
        }

        public static bool HasCookieConsent(ViewContext context)
        {
            return context.ViewBag.HasCookieConsent ?? false;
        }
    }
}
