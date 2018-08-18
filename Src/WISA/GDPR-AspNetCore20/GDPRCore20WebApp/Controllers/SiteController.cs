using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using GDPRCore20WebApp.Services;

namespace GDPRCore20WebApp.Controllers
{
    [AllowAnonymous]
    public class SiteController : ControllerBase
    {
        [HttpPost]
        public ActionResult AllowCookies(string returnUrl)
        {
            CookieConsent.SetCookieConsent(Response, true);
            if ((returnUrl != null) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult NoCookies(string returnUrl)
        {
            CookieConsent.SetCookieConsent(Response, false);

            if (SiteHelper.IsAjaxRequest(HttpContext.Request))
                return StatusCode(200);     //// if we got an ajax submit, just return 200 OK
            else
            {
                if ((returnUrl != null) && Url.IsLocalUrl(returnUrl))
                    return LocalRedirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
        }
    }

    public static class SiteHelper
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request?.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }
}
