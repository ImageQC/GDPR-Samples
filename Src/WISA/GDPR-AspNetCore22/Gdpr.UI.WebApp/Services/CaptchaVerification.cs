using System;
using System.Collections;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;

using Microsoft.AspNetCore.Mvc.RazorPages;

using Newtonsoft.Json;
using MxReturnCode;

namespace Gdpr.UI.WebApp.Services
{
    public class CaptchaVerification
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public IList Errors { get; set; }

        public override string ToString()
        {
            string rc = "reCaptcha succeeded";
            if ((Errors != null) && (Errors.Count > 0))
            {
                rc = "User Validation failed: ";
                foreach (var err in Errors)
                    rc += err?.ToString()?.Replace('-', ' ') + "; " ?? "no error found";
                rc += "Fix the problem and try again.";
            }
            return rc;
        }

        public static async Task<CaptchaVerification> GetRequest(PageModel page, ServiceConfig options, bool devMode = false)
        {
            CaptchaVerification rc = null;

            if (devMode)
                rc = new CaptchaVerification() {Success = true};
            else
            {   // by https://github.com/Fleximex @ https://github.com/PaulMiami/reCAPTCHA/issues/22
                if (options?.ReCaptchaSecretKey == null)
                    rc = new CaptchaVerification() { Success = false, Errors = new ArrayList(new[] {"system error: key not found"})};
                else
                {
                    var ipAddress = page?.Request?.HttpContext?.Connection?.RemoteIpAddress;
                    var userIP = ipAddress?.MapToIPv4()?.ToString();
                    if (userIP == null)
                        rc = new CaptchaVerification() { Success = false, Errors = new ArrayList(new[] { "system error: user ip not found" }) };
                    else
                    {
                        var payload = $"&secret={options.ReCaptchaSecretKey}&remoteip={userIP}&response={page?.Request?.Form["g-recaptcha-response"] ?? "response null"}";
                        var client = new HttpClient
                        {
                            BaseAddress = new Uri("https://www.google.com")
                        };
                        var request = new HttpRequestMessage(HttpMethod.Post, "/recaptcha/api/siteverify")
                        {
                            Content = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded")
                        };
                        var response = await client.SendAsync(request);
                        var result = response?.Content?.ReadAsStringAsync()?.Result;
                        if (result == null)
                            rc = new CaptchaVerification() { Success = false, Errors = new ArrayList(new[] { "system error: no result returned from google" }) };
                        else
                            rc = JsonConvert.DeserializeObject<CaptchaVerification>(result);
                    }
                }
            }
            return rc;
        }
    }
}

