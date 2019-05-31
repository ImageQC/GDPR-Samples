using Dapper;
using Gdpr.Domain.Models;
using MxReturnCode;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gdpr.Domain
{
    public class SystemRepo : RepositoryBase, ISystemRepo  //03-12-18
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
        public SystemRepo(string connection) : base(connection) { }

        public async Task<MxReturnCode<bool>> IsExistRpdAsync(string email)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>($"SystemRepo.IsExistRpdAsync(email={email ?? "[null]"})");

            if (String.IsNullOrWhiteSpace(email))
                rc.SetError(1040101, MxError.Source.Param, "email is null or empty");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "SELECT * FROM GdprRpd WHERE Email = @Email";
                        var res = await db.QuerySingleOrDefaultAsync<GdprRpd>(sql, new { Email = email });
                        if ((res == null) || (res.Email != email))
                            rc.SetResult(false);
                        else
                            rc.SetResult(true);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1040102, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public async Task<MxReturnCode<bool>> CreateRpdAsync(string email, string fullName, RpdChildFlag childFlag, string roleName, Guid identityUserId)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("SystemRepo.CreateRpdAsync()");

            if (String.IsNullOrWhiteSpace(email))
                rc.SetError(1040201, MxError.Source.Param, "invalid email");
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
                    rc.SetError(1040202, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public async Task<MxReturnCode<GdprRpd>> GetRpdAsync(string email)
        {
            MxReturnCode<GdprRpd> rc = new MxReturnCode<GdprRpd>("SystemRepo.GetRpdAsync()");

            if (String.IsNullOrWhiteSpace(email))
                rc.SetError(1040301, MxError.Source.Param, "invalid email");
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
                    rc.SetError(1040302, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public Task<MxReturnCode<bool>> UpdateRpdAsync(string email, string fullName, RpdChildFlag childFlag)
        {
            throw new NotImplementedException();
        }

        public async Task<MxReturnCode<bool>> DeleteRpdAsync(string email)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>($"AdminRepository.DeleteRpdAsync(email={email ?? "[null]"})");

            if (string.IsNullOrWhiteSpace(email))
                rc.SetError(1040401, MxError.Source.Param, "email is null or empty");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "DELETE FROM GdprRpd WHERE Email = @Email";
                        var res = await db.ExecuteAsync(sql, new { Email = email });
                        if (res != 1)
                            rc.SetError(1040402, MxError.Source.Data, $"record not deleted: email={email ?? "[null]"}");
                        else
                        {
                            rc.SetResult(true);
                        }
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1040403, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public async Task<MxReturnCode<int>> GetRpdCountAsync()
        {
            MxReturnCode<int> rc = new MxReturnCode<int>("SystemRepo.GetRpdCountAsync()", -1);
            try
            {
                if ((rc += CheckConnection()).IsSuccess())
                {
                    var sql = "SELECT COUNT(*) FROM GdprRpd;";
                    var res = await db.QuerySingleAsync<int>(sql);
                    rc.SetResult(res);
                }
            }
            catch (Exception e)
            {
                rc.SetError(1040501, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
            }
            return rc;
        }
    }
}
