using System;
using System.Data;
using System.Data.SqlClient;
using MxReturnCode;

namespace Gdpr.Domain
{
    public class RepositoryBase : IDisposable
    {
        protected string DbConnectionForDisplay { private set; get; }
        protected string DbConnectionForUse { private set; get; }
        protected IDbConnection db = null;
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposed == false)
                {
                    if (disposing)
                    {
                        if (db != null)
                            db.Dispose();
                    }
                }
            }
            finally
            {
                disposed = true;
            }
        }
        public RepositoryBase()  {  }
        public RepositoryBase(string connection)
        {
            try
            {
               DbConnectionForUse = connection;
               var builder = new SqlConnectionStringBuilder(connection)
                {
                    Password = "********",
                    UserID = "********"
                };

                DbConnectionForDisplay = builder.ToString();
                db = new SqlConnection(connection);
            }
            catch (Exception e)
            {
                DbConnectionForDisplay = e.Message;
            }
        }

        public static bool IsGuidSet(Guid guid) { return (guid == Guid.Empty) ? false : true;  }
        public MxReturnCode<bool> CheckConnection()
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("CheckConnection()", false);

            if (db == null)
                rc.SetError(1010101, MxError.Source.AppSetting, String.Format("invalid connection {0}", DbConnectionForDisplay ?? "null", MxMsgs.MxErrDbConnNotSet));
            else
            {
                try
                {
                    if (db.State != ConnectionState.Open)
                        db.Open();
                    if (db.State != ConnectionState.Open)
                        rc.SetError(1010102, MxError.Source.Sys, String.Format("cannot open database connection {0}", DbConnectionForDisplay ?? "[null]"), MxMsgs.MxErrDbConnClosed);
                    else
                        rc.SetResult(true);
                }
                catch (Exception e)
                {
                    rc.SetError(1010103, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbConnException, true);
                }
            }
            return rc;
        }


    }
}
