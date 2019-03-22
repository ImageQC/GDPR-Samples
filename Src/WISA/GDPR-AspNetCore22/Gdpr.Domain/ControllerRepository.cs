using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Gdpr.Domain.Models;
using MxReturnCode;

namespace Gdpr.Domain
{
    public class ControllerRepository : RepositoryBase, IControllerRepository  //03-12-18
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

        public ControllerRepository(string connection) : base(connection) { }

        public async Task<MxReturnCode<GdprWst>> GetTermsConditionsAsync(string title)
        {
            MxReturnCode<GdprWst> rc = new MxReturnCode<GdprWst>("ControllerRepository.GetTermsConditionsAsync()");

            if (String.IsNullOrWhiteSpace(title))
                rc.SetError(1040101, MxError.Source.Param, "title is null or empty");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "SELECT * FROM GdprWst WHERE Title = @Title";
                        var res = await db.QuerySingleOrDefaultAsync<GdprWst>(sql, new { Title = title });
                        rc.SetResult(res);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1040102, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }


        public async Task<MxReturnCode<bool>> CreateTermsConditionsAsync(string title)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("ControllerRepository.CreateTermsConditionsAsync()");

            if (String.IsNullOrWhiteSpace(title))
                rc.SetError(1040201, MxError.Source.Param, "invalid title");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        GdprWst terms = new GdprWst
                        {
                            Title = title,
                            Status = (int)GdprWst.StatusVal.NotImplemented
                        };
                        var sql = "INSERT INTO GdprWst(Title, Status) VALUES(@Title,  @Status);";
                        var res = await db.ExecuteAsync(sql, terms);
                        if (res != 1)
                            rc.SetError(1040202, MxError.Source.Data, String.Format("invalid record data {0}", terms.ToString()));
                        else
                            rc.SetResult(true);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1040203, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }
    }
}
