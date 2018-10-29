using System;
using System.Collections.Generic;
using System.Text;

namespace Gdpr.Domain
{
    public static class MxMsgs
    {
        public const string SupportedCultures = "en;";      //must end with ;
        //Messages
        //public const string MxMsgNotFound = "MxMsgNotFound";
        //"Message not found. Coding defect. Please report this problem" 

        //Errors - summary of the problem(cannot connect to server). details. what to do next(try again later)
        public const string MxErrDbConnClosed = "MxErrDbConnClosed";
        public const string MxErrDbConnNotSet = "MxErrDbConneNotSet";
        public const string MxErrDbConnException = "MxErrDbConnException";
        public const string MxErrDbQueryException = "MxErrDbQueryException";
        public const string MxErrDbCmdException = "MxErrDbCmdException";

        //"Error test. Coding defect. Please report this problem"
    }
}
