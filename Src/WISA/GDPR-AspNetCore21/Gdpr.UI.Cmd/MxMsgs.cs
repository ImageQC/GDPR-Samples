using System;
using System.Collections.Generic;
using System.Text;

namespace Gdpr.UI.Cmd
{
    public static class MxMsgs
    {
        public const string SupportedCultures = "en;";      //must end with ;

        //Messages
        public const string MxMsgNotFound = "MxMsgNotFound";
        //"Message not found. Coding defect. Please report this problem" 

        //Errors - unique identifier for the error (error code 1234), summary of the problem(cannot connect to server), type of error(program defect, user mistake, service not available, etc),what to do next(try again later)
        public const string MxErrTest = "MxErrTest";
        //"Error test. Coding defect. Please report this problem"
    }
}
