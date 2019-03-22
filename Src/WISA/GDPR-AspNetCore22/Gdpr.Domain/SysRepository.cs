using Dapper;
using Gdpr.Domain.Models;
using MxReturnCode;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gdpr.Domain
{
    public class SysRepository : RepositoryBase, ISysRepository  //03-12-18
    {
        private bool disposed = false;
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposed == false)
                {
                    if (disposing)
                    {
                        //release any resources created by AdminRepository
                    }
                }
            }
            finally
            {
                this.disposed = true;
                base.Dispose(disposing);    //release any resources created by its base class

            }
        }
        public SysRepository(string connection) : base(connection) { }

        public async Task<MxReturnCode<GdprRpd>> GetUserAsync(string email)
        {
            MxReturnCode<GdprRpd> rc = new MxReturnCode<GdprRpd>("SysRepository. GetUserAsync()");

            if (String.IsNullOrWhiteSpace(email))
                rc.SetError(1030101, MxError.Source.Param, "invalid email");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "SELECT * FROM GdprRpd WHERE Email = @Email";
                        var res = await db.QuerySingleOrDefaultAsync<GdprRpd>(sql, new { Email = email });
                        rc.SetResult(res);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1030102, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }
        public async Task<MxReturnCode<bool>> CreateUserAsync(string email)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("SysRepository.CreateUserAsync()");

            if (String.IsNullOrWhiteSpace(email))
                rc.SetError(1030201, MxError.Source.Param, "invalid email");
            else
            {
                try
                {
                    //Pass AspNetUsers.ID
                    //Pass Role name 
                    //Get URD ID
                    //Get all WXR for URDID
                    //Find lastest WST in WXR list
                    //create RPD
                    //Create UTA for WST and RPD

                }
                catch (Exception e)
                {
                    rc.SetError(1030202, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }
    }
}
