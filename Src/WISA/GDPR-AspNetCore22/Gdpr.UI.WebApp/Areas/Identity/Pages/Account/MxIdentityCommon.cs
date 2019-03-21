using System.Security.Policy;

namespace Gdpr.UI.WebApp.Areas.Identity.Pages.Account
{
    public class MxIdentityCommon
    {
        public static readonly string WebSiteName = "GDPRCore22";
        private static readonly string websiteurl = "http://gdprcore22webapp.azurewebsites.net";
        private static readonly string emailbodyend = "<br /><br />If you are not expecting this message then please do NOT click the link and return this email to its sender.<br /><br />Many thanks<br />Will Stott";

        public static readonly string EmailConfirmNotReceivedForgotEmail = "<ul class='text-info small'><li>If you don't receive this email then check your junk folder or use the forgotten password mechanism to arrange for it to be resent.</li></ul>";
        public static readonly string EmailConfirmNotReceivedResend = "<ul class='text-info small'><li>If you don't receive this email then check your junk folder or click Save again to arrange for it to be resent.</li></ul>";

        public static readonly string WaitForEmailConfirmMsgStr = "To complete the change of your account's email address you must respond to a verification email that will be sent to you. <br />" + EmailConfirmNotReceivedResend;
        public static readonly string WaitForEmailConfirmMsgStrFrmt1 = "To complete {0} you must respond to a verification email that will be sent to you. <br />" + EmailConfirmNotReceivedForgotEmail; 
        public static readonly string MsgStrFrmtParamResetPassword = "the resetting of your account's password";  //if user has forgotten his password, or needs the website to resend the confirmation email for changing email address or registration; all three actions result in setting AspNetUsers.EmailConfirm = true  
        public static readonly string MsgStrFrmtParamRegistration = "your registration";

        public static readonly string MsgVerificationEmailSent = "A verification email has been sent to your email address. " + EmailConfirmNotReceivedForgotEmail;
        public static readonly string MsgPasswordReset = "Your password has been reset";

        public static readonly string MsgAuthenticatorAppVerified = "Your authenticator app has been verified.";

        //strings used in Verification Attributes - cannot be static
        public const string PasswordVerificationMsg = "Your password must be at least six characters long and contain both upper and lower case letters as well as at least one digit.";
        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 100;

        public const string EmailAllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#$%&*+-/=?^_|~*.@";  //removed '`{} and added @ so it allows all characters in a standard email address - see https://stackoverflow.com/questions/2049502/what-characters-are-allowed-in-an-email-address
        public const string EmailVerificationMsg = "Your email address must be at least three characters long and can only contain upper and lower case letters, digits (0-9) as well as the characters ! # $ % & * + - / = ? ^ _ | ~ * . @";
        public const int EmailMinLength = 3;
        public const int EmailMaxLength = 254; //see https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
        public static bool ValidateEmailAddress(string input) { return ValidateInput(input, EmailMinLength, EmailMaxLength, EmailAllowedChars); }

        //note: At Registration username is same as email address, but when changed the username is validated by ValidateNewUsername() unless it is set back to the user's email address
        public const string NewUsernameAllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+-_.";  
        public const string NewUsernameVerificationMsg = "Your username must be at least three characters long and can only contain upper and lower case letters, digits (0-9) as well as the characters + - _ .";
        public const int NewUsernameMinLength = 3;
        public const int NewUsernameMaxLength =15;     //AspNetUsers.UserName allows 256 chars, but limit to 15 when changing it to make it easier to display 
        public static bool ValidateNewUsername(string input) { return ValidateInput(input, NewUsernameMinLength, NewUsernameMaxLength, NewUsernameAllowedChars); }

        public static bool ValidateInput(string input, int minLen, int maxLen, string allowedChars)
        {
            bool rc = false;

            if ((input != null) && (allowedChars != null))
            {
                if ((input.Length >= minLen) && (input.Length <= maxLen))
                {
                    var cnt = 0;
                    foreach (char c in input)
                    {
                        if (allowedChars.Contains(c.ToString()) == false)
                            break;
                        cnt++;
                    }
                    if (cnt == input.Length)
                        rc = true;
                }
            }
            return rc;
        }






        public static string GetEmailSubjectRegistration()
        {
            return "Complete your registration with the website " + websiteurl + ".";
        }

        public static string GetEmailBodyRegistration(string clicklink, string email)  //$"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>"
        {
            string body = "You have registered with the website " + websiteurl + ". " + "Please complete the registration of your account by " + clicklink + ".";
            body += $"<br /><br />You will then need to login using your email address {email} and the password you provided during registration";
            body += emailbodyend;
            return body;
        }

        public static string GetEmailSubjectChangeEmail()
        {
            return "Complete the change of your account email address at " + websiteurl + ".";
        }

        public static string GetEmailBodyChangeEmail(string clicklink, string newEmail) //$"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>"
        {
            string body = "You have initiated a change of your account email address for the website " + websiteurl + ". ";
            body += "<br /><br />WARNING: Changing your email address will cause you to be logged-out of the website. ";
            body += $"You will then need to login using your new email address {newEmail} and existing password";
            body += "<br /><br />Please complete this change by " + clicklink + ".";
            body += emailbodyend;

            return body;
        }
        public static string GetEmailSubjectResetPassword()
        {
            return "Change your account password at " + websiteurl + ".";
        }

        public static string GetEmailBodyResetPassword(string clicklink, string newEmail) //$"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>"
        {
            string body = "You have initiated a change of your account password for the website " + websiteurl + ". ";
            body += "<br /><br />Please complete this change by " + clicklink + ".";
            body += emailbodyend;

            return body;
        }
    }
}
