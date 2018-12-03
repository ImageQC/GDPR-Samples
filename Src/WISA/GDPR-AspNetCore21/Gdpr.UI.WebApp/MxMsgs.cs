
namespace Gdpr.UI.WebApp
{
    public static class MxMsgs
    {
        public const string SupportedCultures = "en;";      //must end with ;

        //Messages
        public const string MxMsgNotFound = "MxMsgNotFound";
        //"Message not found. Coding defect. Please report this problem" 

        //Errors
        public const string MxErrTest = "MxErrTest";
        public const string MxErrUnknownException = "MxErrUnknownException";
        public const string MxErrUnexpected = "MxErrUnexpected";
        public const string MxErrAccountEmailNotUnique = "MxErrAccountEmailNotUnique";
        public const string MxErrAccountUserNameNotUnique = " MxErrAccountUserNameNotUnique";
        public const string MxErrAccountPasswordNotSet = "MxErrAccountPasswordNotSet";
        public const string MxErrAccountInvalidLoginAttempt = "MxErrAccountInvalidLoginAttempt";
        public const string MxErrAccountDisabled = "MxErrAccountDisabled";
        public const string MxErrAccountLockout = "MxErrAccountLockout";
        //"Error test. Coding defect. Please report this problem"


        //suggested error code numbering scheme zyyxxww - z assembly code, yy class code, xx method code, ww error instance
        //-----------------------------------------------------------------------------------------------------------------
        //The benefit of an error code lies in it providing a quick way of identifying a line in your code base. Therefore don't be too worried if codes are duplicated in assemblies outside your codebase
        //      Note: max 32 bit int is  2,147,483,647 >  max error code value (9,999,999)
        //Whilst you should try to avoid duplicating error code values in your code base, doing so doesn't break anything. The following conventions may help:
        //  each assembly starts with a new million value; ReturnCodeApp=1yyxxww, ReturnCodeTest=1yyxxww, Last=9yyxxww (>10 assemblies then add an extra digit to error code, but this shouldn't happen too often)
        //  each class starts with a new ten thousand value; Program=z01xxww, Detail=z02xxww,  Last=z99xxww (>100 classes in assembly then restart with next available assembly number, but this shouldn't happen in well structured code) 
        //  each method starts with a new hundred value; main=zyy01ww, Process=zyy02ww, Last=zyy99ww (>100 methods in class then restart with next available class number, but this shouldn't happen in well structured code)
        //  each error starts with a new unit value; error1=zyyxx01, error2=zyyxx02, Last=zyyxx99 (>100 errors in method then restart with next available method number, but this shouldn't happen in well structured code)

        public static class ErrorCodeGdprDomain
        {       //Gdrp.Domain
            public const int DomainRepositoryBaseFirst = 1010101;    //first error code in class Gdrp.Domain.RepositoryBase
            public const int DomainRepositoryBaseLast = 1010103;     //last error code in class Gdrp.Domain.RepositoryBase
            public const int DomainAdminRepositoryFirst = 1020101;   //first error code in class Gdrp.Domain.AdminRepository
            public const int DomainAdminRepositoryLast = 1020601;    //last error code in class Gdrp.Domain.AdminRepository
            public const int DomainSysRepositoryFirst = 1030101;   //first error code in class Gdrp.Domain.AdminRepository
            public const int DomainSysRepositoryLast = 1030103;    //last error code in class Gdrp.Domain.AdminRepository
            public const int DomainControllerRepositoryFirst = 1040101;   //first error code in class Gdrp.Domain.AdminRepository
            public const int DomainControllerRepositoryLast = 1040103;    //last error code in class Gdrp.Domain.AdminRepository
                                                                   //Gdrp.UI.Cmd
            public const int CmdProgramFirst = 2010101;             //first error code in class Gdrp.UI.Cmd.Program
            public const int CmdProgramLast = 2010101;              //last error code in class Gdrp.UI.Cmd.Program
              //Gdrp.UI.WebApp
            public const int WebAppProgramFirst = 3010101;                      //first error code in class Gdrp.UI.WebApp.Error.cshtml.cs
            public const int WebAppProgramLast = 3010103;                       //last error code in class Gdrp.UI.WebApp.Error.cshtml.cs
            public const int WebAppConfirmEmailChangeFirst = 3020101;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Manage.ConfirmEmailChange.cshtml.cs
            public const int WebAppConfirmEmailChangeLast = 3020108;           //last error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Manage.ConfirmEmailChange.cs
            public const int WebAppAccountManageIndexFirst = 3030101;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Manage.Index.cshtml.cs
            public const int WebAppAccountManageIndexLast = 3030404;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Manage.Index.cshtml.cs
            public const int WebAppAccountRegisterFirst = 3040101;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Register.cshtml.cs
            public const int WebAppAccountRegisterLast = 3040105;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Register.cshtml.cs
            public const int WebAppAccountConfirmEmailFirst = 3050101;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.ConfirmEmail.cshtml.cs
            public const int WebAppAccountConfirmEmailLast = 3050104;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.ConfirmEmail.cshtml.cs
            public const int WebAppAccountLoginFirst = 3060101;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.ConfirmEmail.cshtml.cs
            public const int WebAppAccountLoginLast = 3060207;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.ConfirmEmail.cshtml.cs
            public const int WebAppAccountResetPasswordFirst = 3070101;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.ResetPassword.cshtml.cs
            public const int WebAppAccountResetPasswordLast = 3070104;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.ResetPassword.cshtml.cs
            public const int WebAppAccountForgotPasswordFirst = 3080101;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.ForgotPassword.cshtml.cs
            public const int WebAppAccountForgotPasswordLast = 3080102;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.ForgotPassword.cshtml.cs
            public const int WebAppAccountExternalLoginFirst = 3090101;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.ForgotPassword.cshtml.cs
            public const int WebAppAccountExternalLoginLast = 3090205;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.ForgotPassword.cshtml.cs
            public const int WebAppAccountChangePasswordFirst = 3100101;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Manage.ChangePassword.cshtml.cs
            public const int WebAppAccountChangePasswordLast = 3100104;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Manage.ChangePassword.cshtml.cs
            public const int WebAppAccountSetPasswordFirst = 3110101;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Manage.SetPassword.cshtml.cs
            public const int WebAppAccountSetPasswordLast = 3110104;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Manage.SetPassword.cshtml.cs
            public const int WebAppAccountManageEnableAuthenticatorFirst = 3120101;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Manage.EnableAuthenticator.cshtml.cs
            public const int WebAppAccountManageEnableAuthenticatoLast = 3120104;          //first error code in class Gdpr.UI.WebApp.Areas.Identity.Pages.Account.Manage.EnableAuthenticator.cshtml.cs
            public const int WebAppIndexFirst = 3130101;          //first error code in class Gdpr.UI.WebApp.Pages.Index.cshtml.cs
            public const int WebAppIndexLast = 3130104;          //first error code in class Gdpr.UI.WebApp.Pages.Index.cshtml.cs
            public const int WebAppErrorFirst = 3140101;                      //first error code in class Gdrp.UI.WebApp.Error.cshtml.cs
            public const int WebAppErrorLast = 3140101;                       //last error code in class Gdrp.UI.WebApp.Error.cshtml.cs
            public const int WebAppDataSeedDbFirst = 3150101;                      //first error code in class Gdrp.UI.WebApp.Data.SeedDb.cs
            public const int WebAppDataSeedDbLast = 3150101;                       //last error code in class Gdrp.UI.WebApp.Data.SeedDb.cs
            public const int WebAppDataIdentityDbFirst = 3160101;                      //first error code in class Gdrp.UI.WebApp.Data.SeedDb.cs
            public const int WebAppDataIdentityDbLast = 3160101;                       //last error code in class Gdrp.UI.WebApp.Data.SeedDb.cs

        }
    }
}
