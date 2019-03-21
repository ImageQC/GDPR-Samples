using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Gdpr.UI.WebApp.Pages.Shared;
using Gdpr.UI.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MxReturnCode;

namespace Gdpr.UI.WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : BasePageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IHostingEnvironment _env;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IOptions<ServiceConfig> optionsAccessor,
            IEmailSender emailSender,
            IHostingEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            Options = optionsAccessor.Value;
            _emailSender = emailSender;
            _env = env;
        }

        public bool IsDevMode(){return (_env.IsDevelopment()) ? true : false;}
        public ServiceConfig Options { get; } //set only via Secret Manager

        [BindProperty] public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            MxReturnCode<IActionResult> rc = new MxReturnCode<IActionResult>("Account.Manage.Register.OnPostAsync()", Page());

            string userId = null;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
                rc.SetError(3010101, MxError.Source.User, WebErrorHandling.GetModelStateErrors(ModelState, WebErrorHandling.FormValidationErrorPreamble));
            else
            {
                try
                {
                    if (await ValidateForm() == false)
                        rc.SetError(3010102, MxError.Source.User, WebErrorHandling.GetModelStateErrors(ModelState, WebErrorHandling.FormValidationErrorPreamble));
                    else
                    {
                        var user = new IdentityUser {UserName = Input.Email, Email = Input.Email};
                        var result = await _userManager.CreateAsync(user, Input.Password);
                        if (result.Succeeded == false)
                            rc.SetError(3010103, MxError.Source.Sys, WebErrorHandling.GetIdentityErrors(result, $"cannot register user {Input.Email}"));
                        else
                        {
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new {userId = user.Id, code = code},
                                protocol: Request.Scheme);

                            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                            userId = user?.Id;
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            rc.SetResult(LocalRedirect(returnUrl));
                        }
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(3010104, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
                }
            }
            if (rc.IsError(true))
                SetPageStatusMsg(rc.GetErrorUserMsgHtml(userId), ExistingMsg.Overwrite);

            return rc.GetResult();
        }

        private async Task<bool> ValidateForm()     //additional validation to be performed server-side
        {
            bool rc = false;        //Validate Password not needed as it is done client-side

            CaptchaVerification captchaVerification = await CaptchaVerification.GetRequest(this, Options, IsDevMode());
            if (captchaVerification == null)
            {
                ModelState.AddModelError("User Validation", "setup error");
            }
            else if (captchaVerification.Success != true)
            {
                ModelState.AddModelError("User Validation", captchaVerification.ToString());
            }
            else if (MxIdentityCommon.ValidateEmailAddress(Input.Email) == false)
            {
                ModelState.AddModelError("Email", MxIdentityCommon.EmailVerificationMsg);
            }
            else if (await _userManager.FindByEmailAsync(Input.Email) != null)
            {
                ModelState.AddModelError("Email", $"{Input.Email} is already taken");
            }
            else if (await _userManager.FindByNameAsync(Input.Email) != null)
            {                   //during registration username is same as email so no need to call MxIdentityCommon.ValidateNewUsername()
                ModelState.AddModelError("Username", $"{Input.Email} is already taken"); //might suggest an alternative
            }
            else
            {
                rc = true;
            }
            return rc;
        }
    }
}
