using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MxReturnCode;

using Gdpr.Domain.Models;


namespace Gdpr.Domain
{
    public class AdminRepository : RepositoryBase, IAdminRepository
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
        public AdminRepository(string connection) : base(connection) { }

        public async Task<MxReturnCode<bool>> CreateRoleAsync(string name, int rolecode, string purpose, string description)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("CreateRoleAsync()");

            if ((name?.Length == 0) || (GdprUrd.IsValidRoleCode(rolecode) == false) || (purpose?.Length == 0) || (description?.Length == 0))
                rc.SetError(1020101, MxError.Source.Param, "invalid name, rolecode, purpose, description");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        GdprUrd role = new GdprUrd
                        {
                            Name = name,
                            RoleCode = rolecode,
                            Purpose = purpose,
                            Description = description,
                            Status = (int)GdprUrd.StatusVal.NotImplemented
                        };
                        var sql = "INSERT INTO GdprUrd(Name, RoleCode, Status, Purpose, Description) VALUES(@Name, @RoleCode, @Status, @Purpose, @Description);";
                        var res = await db.ExecuteAsync(sql, role);
                        if (res != 1)
                            rc.SetError(1020102, MxError.Source.Data, String.Format("invalid record data {0}", role.ToString()));
                        else
                            rc.SetResult(true);
                    }
                }
                catch(Exception e)
                {
                    rc.SetError(1020103, MxError.Source.Exception,e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

  
        public async Task<MxReturnCode<int>> GetRoleCountAsync()
        {
            MxReturnCode<int> rc = new MxReturnCode<int>("GetRoleCountAsync()", -1);
            try
            {
                if ((rc += CheckConnection()).IsSuccess())
                {
                    var sql = "SELECT COUNT(*) FROM GdprUrd;";
                    var res = await db.QuerySingleAsync<int>(sql);
                    rc.SetResult(res);
                }
              }
            catch (Exception e)
            {
                rc.SetError(1020201, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
            }
            return rc;
        }


        public async Task<MxReturnCode<bool>> DeleteRoleAsync(GdprUrd role)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("DeleteRoleAsync()", false);

            if (role == null)
                rc.SetError(1020301, MxError.Source.Param, "role is null");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "DELETE FROM GdprUrd WHERE Id = @Id";
                        var res = await db.ExecuteAsync(sql, new { Id = role.Id });
                        if (res != 1)
                            rc.SetError(1020302, MxError.Source.Data, String.Format("record not deleted {0}", role.ToString()));
                        else
                            rc.SetResult(true);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1020303, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public async Task<MxReturnCode<bool>> UpdateRoleAsync(GdprUrd role, string name, int rolecode, string purpose, string description, GdprUrd.StatusVal status)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("UpdateRoleAsync()", false);

            if ((role == null) || (RepositoryBase.IsGuidSet(role.Id) == false) || (name?.Length == 0) || (GdprUrd.IsValidRoleCode(rolecode) == false))
                rc.SetError(1020301, MxError.Source.Param, "role is null, role.Id is not set, name is null or emptpy, or rolecode is invalid");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "UPDATE GdprUrd SET Name = @Name, RoleCode = @RoleCode, Status = @Status, Purpose = @Purpose, Description = @Description WHERE Id = @Id;";
                        var res = await db.ExecuteAsync(sql, new { Name = name, RoleCode = rolecode, Status = (int)status, Purpose = purpose, Description = description, Id = role.Id });
                        if (res != 1)
                            rc.SetError(1020302, MxError.Source.Data, String.Format("record not updated {0}", role.ToString()));
                        else
                            rc.SetResult(true);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1020303, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }
                       
        public async Task<MxReturnCode<GdprUrd>> GetRoleAsync(string rolename)
        {
            MxReturnCode<GdprUrd> rc = new MxReturnCode<GdprUrd>("GetRoleAsync(rolename)", null);

            if (rolename?.Length == 0)
                rc.SetError(1020401, MxError.Source.Param, "rolename is null or empty");
            else
            {
                try
                {

                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "SELECT * FROM GdprUrd WHERE Name = @Name";
                        var res = await db.QuerySingleAsync<GdprUrd>(sql, new { Name = rolename });
                        rc.SetResult(res);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1020402, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }


        public async Task<MxReturnCode<List<GdprUrd>>>GetAllRolesAsync()
        {
            MxReturnCode<List<GdprUrd>> rc = new MxReturnCode<List<GdprUrd>>("GetAllRolesAsync()", null);

            try
            {

                if ((rc += CheckConnection()).IsSuccess())
                {
                    var sql = "SELECT * FROM GdprUrd";
                    using (var res = await db.QueryMultipleAsync(sql))
                    {
                        var roles = res.Read<GdprUrd>().AsList();
                        rc.SetResult(roles);
                    }
                }
            }
            catch (Exception e)
            {
                rc.SetError(1020501, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
            }
            return rc;
        }


        public async Task<MxReturnCode<GdprUrd>> GetRoleAsync(GdprRpd user)
        {
            var res = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM GdprUrd;");
            throw new NotImplementedException();
        }

        public async Task<MxReturnCode<List<GdprRpd>>> GetUsersWithRoleAsync(GdprUrd role)
        {
            var res = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM GdprUrd;");
            throw new NotImplementedException();
        }

        public async Task<MxReturnCode<List<GdprFpd>>> GetProcessingForRole(GdprUrd role)
        {
            var res = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM GdprUrd;");
            throw new NotImplementedException();
        }

        public async Task<MxReturnCode<List<GdprWst>>> GetWebsiteTermsForRole(GdprUrd role)
        {
            var res = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM GdprUrd;");
            throw new NotImplementedException();
        }
        public async Task<MxReturnCode<bool>> AssignRoleAsync(GdprUrd role, GdprRpd user)
        {
            var res = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM GdprUrd;");
            throw new NotImplementedException();
        }
    }
}
