using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Gdpr.Domain.Models;
using MxReturnCode;

namespace Gdpr.Domain
{
    public class ControllerRepo : RepositoryBase, IControllerRepo  //03-12-18
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

        public ControllerRepo(string connection) : base(connection) { }

        public async Task<MxReturnCode<bool>> IsExistWstAsync(string title)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("ControllerRepository.IsExistWstAsync()");

            if (String.IsNullOrWhiteSpace(title))
                rc.SetError(1020101, MxError.Source.Param, "title is null or empty");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "SELECT * FROM GdprWst WHERE Title = @Title";
                        var res = await db.QuerySingleOrDefaultAsync<GdprWst>(sql, new { Title = title });
                        rc.SetResult((res != null));
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1020102, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public async Task<MxReturnCode<bool>> CreateWstAsync(string title, string description, string url)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("ControllerRepository.CreateWstAsync()");

            if (String.IsNullOrWhiteSpace(title) || String.IsNullOrWhiteSpace(description) || String.IsNullOrWhiteSpace(url))
                rc.SetError(1020201, MxError.Source.Param, $"invalid title={title ?? "[null]"} or null description or url");
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
                            rc.SetError(1020202, MxError.Source.Data, String.Format("invalid record data {0}", terms.ToString()));
                        else
                            rc.SetResult(true);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1020203, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public async Task<MxReturnCode<bool>> DeleteWstAsync(string title)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>($"ControllerRepository.DeleteWstAsync({title ?? "[null]"})", false);

            if (string.IsNullOrWhiteSpace(title))
                rc.SetError(1010301, MxError.Source.Param, "role is null");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sqlFind = "SELECT * FROM GdprWst WHERE Title = @Title";
                        var resFind = await db.QuerySingleOrDefaultAsync<GdprWst>(sqlFind, new { Title = title });
                        if (resFind == null)
                            rc.SetError(1010302, MxError.Source.Data, $"{title} not found");
                        else
                        {
                            var sql = "DELETE FROM GdprWst WHERE Id = @Id";
                            var res = await db.ExecuteAsync(sql, new { Id = resFind.Id });
                            if (res != 1)
                                rc.SetError(1010303, MxError.Source.Data, $"Wst record not deleted Id={resFind.Id}, title={title}");
                            else
                                rc.SetResult(true);
                        }
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1010303, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public async Task<MxReturnCode<GdprWst>> GetWstAsync(string title)
        {
            MxReturnCode<GdprWst> rc = new MxReturnCode<GdprWst>("ControllerRepository.GetWstAsync()");

            if (String.IsNullOrWhiteSpace(title))
                rc.SetError(1010401, MxError.Source.Param, "title is null or empty");
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
                    rc.SetError(1010402, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }
    }
}
