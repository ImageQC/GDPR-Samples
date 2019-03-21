using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gdpr.UI.WebApp.Services
{
    public class ServiceConfig
    {
        public string SendGridSenderEmail { get; set; }
        public string SendGridSenderName { get; set; }
        public string SendGridKey { get; set; }
        public string ReCaptchaSiteKey { get; set; }
        public string ReCaptchaSecretKey { get; set; }
    }
}
