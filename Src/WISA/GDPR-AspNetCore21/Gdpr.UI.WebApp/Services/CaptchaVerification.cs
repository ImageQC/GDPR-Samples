using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gdpr.UI.WebApp.Services
{
    public class CaptchaVerification
    {
        public CaptchaVerification()
        {
        }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public IList Errors { get; set; }

        public override string ToString()
        {
            string rc = "reCaptcha succeeded";
            if (Errors.Count > 0)
            {
                rc = "User Validation failed: ";
                foreach (var err in Errors)
                    rc += err.ToString().Replace('-', ' ') + "; ";
                rc += "Fix the problem and try again.";
            }
            return rc;
        }

        public static async Task<CaptchaVerification> GetRequest(PageModel page, ServiceConfig options, bool devMode = false)
        {
            if (devMode)
                return new CaptchaVerification() { Success = true };
            else
            {   // by https://github.com/Fleximex @ https://github.com/PaulMiami/reCAPTCHA/issues/22
                string userIP = string.Empty;
                var ipAddress = page.Request.HttpContext.Connection.RemoteIpAddress;
                if (ipAddress != null)
                    userIP = ipAddress.MapToIPv4().ToString();
                var payload = string.Format("&secret={0}&remoteip={1}&response={2}", options.ReCaptchaSecretKey, userIP, page.Request.Form["g-recaptcha-response"]);

                var client = new HttpClient
                {
                    BaseAddress = new Uri("https://www.google.com")
                };
                var request = new HttpRequestMessage(HttpMethod.Post, "/recaptcha/api/siteverify")
                {
                    Content = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded")
                };
                var response = await client.SendAsync(request);
                return JsonConvert.DeserializeObject<CaptchaVerification>(response.Content.ReadAsStringAsync().Result);
            }
        }
    }

}
