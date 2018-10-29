using System;
using System.Collections.Generic;
using System.Text;

namespace Gdpr.UI.Web
{
    public static class MxMsgs
    {
        public const string SupportedCultures = "en;";      //must end with ;

        //Messages
        public const string MxMsgNotFound = "MxMsgNotFound";
        //"Message not found. Coding defect. Please report this problem" 

        //Errors
        public const string MxErrTest = "MxErrTest";
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
            public const int DomainAdminRepositoryLast = 1020103;    //last error code in class Gdrp.Domain.AdminRepository
               //Gdrp.UI.Cmd
            public const int CmdProgramFirst = 2010101;     //first error code in class Gdrp.UI.Cmd.Program
            public const int CmdProgramLast = 2010101;     //last error code in class Gdrp.UI.Cmd.Program

        }
    }
}
