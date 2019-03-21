using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gdpr.UI.WebApp.Pages.Shared;
using Gdpr.UI.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MxReturnCode;

namespace Gdpr.UI.WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : BasePageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<ExternalLoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ProviderEmail { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            MxReturnCode<IActionResult> rc = new MxReturnCode<IActionResult>("Account.ExternalLogin.OnGetCallbackAsync()", RedirectToPage("./Login", new { ReturnUrl = returnUrl }));

            returnUrl = returnUrl ?? Url.Content("~/");
            if ((returnUrl == null) || (remoteError != null))
                rc.SetError(3020101, MxError.Source.Service, $"Error from external provider: {remoteError}");
            else
            {
                try
                {
                    var info = await _signInManager.GetExternalLoginInfoAsync();
                    if (info == null)
                        rc.SetError(3020102, MxError.Source.Service, "Error loading external login information.");
                    else
                    {   // Sign in the user with this external login provider if the user already has a login.
                        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
                        if (result.Succeeded)
                        {
                            SetPageStatusMsg($"Welcome {info.Principal.Identity.Name} you have been authenticated by {info.LoginProvider}", ExistingMsg.Overwrite);
                            rc.SetResult(LocalRedirect(returnUrl));
                        }
                        else if (result.IsLockedOut)
                            rc.SetError(3020103, MxError.Source.Sys, "user account locked out", MxMsgs.MxErrAccountLockout);
                        else if (result.RequiresTwoFactor)
                        {
                            SetPageStatusMsg($"Welcome {info.Principal.Identity.Name} you have been authenticated by {info.LoginProvider}", ExistingMsg.Overwrite);
                            rc.SetResult(LocalRedirect($"~/Identity/Account/LoginWith2fa?ReturnUrl={returnUrl ?? "%2f"}"));
                        }
                        else
                        { // If the user does not have an account, then ask the user to create an account.
                            ReturnUrl = returnUrl;
                            LoginProvider = info.LoginProvider;
                            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                            {
                                Input = new InputModel
                                {
                                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                                };
                                ProviderEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
                            }
                            rc.SetResult(Page());
                        }
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(3020104, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
                }
            }
            if (rc.IsError(true))
                SetPageStatusMsg(rc.GetErrorUserMsgHtml(), ExistingMsg.Overwrite);

            return rc.GetResult();
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            MxReturnCode<IActionResult> rc = new MxReturnCode<IActionResult>("Account.ExternalLogin.OnPostConfirmationAsync()", RedirectToPage("./Login", new { ReturnUrl = returnUrl }));

            try
            {      // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                    rc.SetError(3090201, MxError.Source.Sys, "Error loading external login information during confirmation.");
                else
                {
                    if (ModelState.IsValid == false)
                        rc.SetError(3090202, MxError.Source.Data, WebErrorHandling.GetModelStateErrors(ModelState, WebErrorHandling.FormValidationErrorPreamble));
                    else
                    {
                        var providerEmail = ProviderEmail;
                        if (providerEmail != Input.Email)
                            rc.SetError(3090203, MxError.Source.Sys, $"{providerEmail} from provider != {Input.Email} from form", MxMsgs.MxErrUnexpected);
                        else
                        {
                            IdentityUser user = null;
                            if (await _userManager.FindByEmailAsync(providerEmail) == null)
                            {
                                user = new IdentityUser { UserName = providerEmail, Email = providerEmail, EmailConfirmed = true };
                                var result = await _userManager.CreateAsync(user);
                                if (result.Succeeded == false)
                                    rc.SetError(3090204, MxError.Source.Sys, WebErrorHandling.GetIdentityErrors(result, $"cannot create user account for {providerEmail}"));
                            }
                            if (rc.GetErrorCode() != 3090204)
                            {
                                if ((user = await _userManager.FindByEmailAsync(providerEmail)) == null)
                                    rc.SetError(3090205, MxError.Source.Sys, $"Unable to load user {providerEmail}", MxMsgs.MxErrUnexpected, true);
                                else
                                {
                                    var result = await _userManager.AddLoginAsync(user, info);
                                    if (result.Succeeded == false)
                                        rc.SetError(3090206, MxError.Source.Sys, WebErrorHandling.GetIdentityErrors(result, $"cannot add  {info.LoginProvider} login  for {providerEmail}"));
                                    else
                                    {
                                        await _signInManager.SignInAsync(user, isPersistent: false);
                                        SetPageStatusMsg($"Welcome {info.Principal.Identity.Name} you have been authenticated by {info.LoginProvider}", ExistingMsg.Overwrite);
                                        rc.SetResult(LocalRedirect(returnUrl));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                rc.SetError(3090207, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
            }
            if (rc.IsError(true))
                SetPageStatusMsg(rc.GetErrorUserMsgHtml(), ExistingMsg.Overwrite);

            return rc.GetResult();

        }
    }
}
