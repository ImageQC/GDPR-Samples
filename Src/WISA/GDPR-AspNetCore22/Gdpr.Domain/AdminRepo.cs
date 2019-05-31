using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapper;
using MxReturnCode;

using Gdpr.Domain.Models;

namespace Gdpr.Domain
{
    public class AdminRepo : RepositoryBase, IAdminRepo  //03-12-18 added statics and minor changes; errorcodes, function names
    {       //03-12-18 - added statics
        public static readonly string GdprUrdAdmin = "Admin";
        public static readonly string GdprUrdController = "Controller";
        public static readonly string GdprUrdStandard = "Standard";
        public static readonly string GdprUrdGuest = "Guest";
        public static readonly string GdprUrdSystem = "System";
        public static readonly string GdprUrdGhost = "Ghost";

        public static readonly string GdprUrdNameUndefined = "Undefined name";
        public static readonly string GdprUrdDescriptionUndefined = "Undefined description";
        public static readonly string GdprUrdPurposeUndefined = "Undefined purpose";

        public static readonly string IdentityRoleAdmin = "Admin";
        public static readonly string IdentityRoleController = "Controller";
        public static readonly string IdentityRoleStandard = "Standard";
        public static readonly string IdentityRoleGuest = "Guest";
        //03-12-18 - added statics


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
        public AdminRepo(string connection) : base(connection) { }

        public async Task<MxReturnCode<bool>> IsExistUrdAsync(string name)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>($"AdminRepository.IsExistUrdAsync(name={name ?? "[null]"})");

            if (String.IsNullOrWhiteSpace(name))
                rc.SetError(1010101, MxError.Source.Param, "name is null or empty");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "SELECT * FROM GdprUrd WHERE Name = @Name";
                        var res = await db.QuerySingleOrDefaultAsync<GdprUrd>(sql, new { Name = name });
                        if ((res == null) || (res.Name != name))
                            rc.SetResult(false);
                        else
                            rc.SetResult(true);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1010102, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public async Task<MxReturnCode<bool>> CreateUrdAsync(string name, int roleCode, UrdStatus roleStatus, string purpose, string description, Guid wstId)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("AdminRepository.CreateUrdAsync()");

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(purpose) || string.IsNullOrWhiteSpace(description) || (GdprUrd.IsValidRoleCode(roleCode) == false) || (wstId == Guid.Empty))
                rc.SetError(1010201, MxError.Source.Param, $"name={name ?? "[null]"} or purpose or description null or empty, or roleCode {roleCode} is invalid, or wstId is empty");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        GdprUrd role = new GdprUrd
                        {
                            Name = name,
                            RoleCode = roleCode,
                            Purpose = purpose,
                            Description = description,
                            Status = (int) roleStatus
                        };
                        var sql =
                            "INSERT INTO GdprUrd(Name, RoleCode, Status, Purpose, Description) VALUES(@Name, @RoleCode, @Status, @Purpose, @Description);";
                        var resultCreateUrd = await db.ExecuteAsync(sql, role);
                        if (resultCreateUrd != 1)
                            rc.SetError(1010202, MxError.Source.Data, $"unable to create role={name}");
                        else
                        {
                            var resGetUrd = await GetUrdAsync(name);
                            rc += resGetUrd;
                            if ((rc.IsError()) || (resGetUrd.GetResult() == null))
                                rc.SetError(1010203, MxError.Source.Data, $"unable to access new role={name}");
                            {
                                GdprWxr wxr = new GdprWxr
                                {
                                    UrdId = resGetUrd.GetResult().Id,
                                    WstId = wstId
                                };
                                sql = "INSERT INTO GdprWxr(@UrdId, WstId) VALUES(@UrdId, @WstId);";
                                var resultCreateWxr = await db.ExecuteAsync(sql, wxr);
                                if (resultCreateWxr != 1)
                                    rc.SetError(1010204, MxError.Source.Data, $"unable to create WXR for role={name}, WstId={wstId}");
                                else
                                {
                                    rc.SetResult(true);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1010205, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public async Task<MxReturnCode<GdprUrd>> GetUrdAsync(string name)
        {
            MxReturnCode<GdprUrd> rc = new MxReturnCode<GdprUrd>("AdminRepository.GetUrdAsync()");

            if (String.IsNullOrWhiteSpace(name))
                rc.SetError(1010301, MxError.Source.Param, "name is null or empty");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "SELECT * FROM GdprUrd WHERE Name = @Name";
                        var res = await db.QuerySingleOrDefaultAsync<GdprUrd>(sql, new { Name = name });
                        rc.SetResult(res);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1010302, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public async Task<MxReturnCode<bool>> UpdateUrdAsync(string roleName, int roleCode, UrdStatus roleStatus, string purpose, string description)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>($"AdminRepository.UpdateUrdAsync({roleName ?? "[null]"})");

            if (String.IsNullOrWhiteSpace(roleName) || String.IsNullOrWhiteSpace(purpose) || String.IsNullOrWhiteSpace(description) || (GdprUrd.IsValidRoleCode(roleCode) == false))
                rc.SetError(1010401, MxError.Source.Param, "roleName, purpose or description is null or empty, or rolecode is invalid");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "UPDATE GdprUrd SET RoleCode = @RoleCode, Status = @Status, Purpose = @Purpose, Description = @Description WHERE Name = @Name;";
                        var res = await db.ExecuteAsync(sql, new { RoleCode = roleCode, Status = (int)roleStatus, Purpose = purpose, Description = description, Name = roleName });
                        if (res != 1)
                            rc.SetError(1010402, MxError.Source.Data, $"record not updated: Name={roleName}");
                        else
                            rc.SetResult(true);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1010403, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }

            return rc;
        }

        public async Task<MxReturnCode<bool>> DeleteUrdAsync(string name)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>($"AdminRepository.DeleteUrdAsync(name={name ?? "[null]"})");

            if (string.IsNullOrWhiteSpace(name) )
                rc.SetError(1010501, MxError.Source.Param, "name is null");
            else
            {
                try
                {
                    if ((rc += CheckConnection()).IsSuccess())
                    {
                        var sql = "DELETE FROM GdprUrd WHERE Name = @Name";
                        var res = await db.ExecuteAsync(sql, new { Name = name });
                        if (res != 1)
                            rc.SetError(1010502, MxError.Source.Data, $"record not deleted: name={name ?? "[null]"}");
                        else
                        {
                            //also delete corresponding WXR - is there a trigger?
                            rc.SetResult(true);
                        }
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(1010503, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
                }
            }
            return rc;
        }

        public async Task<MxReturnCode<int>> GetUrdCountAsync()
        {
            MxReturnCode<int> rc = new MxReturnCode<int>("AdminRepository.GetUrdCountAsync()", -1);
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
                rc.SetError(1010601, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
            }
            return rc;
        }

        public async Task<MxReturnCode<List<GdprUrd>>> GetUrdAllAsync()
        {
            MxReturnCode<List<GdprUrd>> rc = new MxReturnCode<List<GdprUrd>>("AdminRepository.GetUrdAllAsync()");

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
                rc.SetError(1010701, MxError.Source.Exception, e.Message, MxMsgs.MxErrDbQueryException);
            }
            return rc;
        }


        public async Task<MxReturnCode<GdprUrd>> GetUrdAsync(GdprRpd user)
        {
            var res = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM GdprUrd;");
            throw new NotImplementedException();
        }

        public async Task<MxReturnCode<List<GdprRpd>>> GetRpdWithUrdAsync(GdprUrd role)
        {
            var res = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM GdprUrd;");
            throw new NotImplementedException();
        }

        public async Task<MxReturnCode<List<GdprFpd>>> GetFdpForUrdAsync(GdprUrd role)
        {
            var res = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM GdprUrd;");
            throw new NotImplementedException();
        }

        public async Task<MxReturnCode<List<GdprWst>>> GetWstForUrdAsync(GdprUrd role)
        {
            var res = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM GdprUrd;");
            throw new NotImplementedException();
        }
        public async Task<MxReturnCode<bool>> AssignUrdToRpdAsync(GdprUrd role, GdprRpd user)
        {
            var res = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM GdprUrd;");
            throw new NotImplementedException();
        }

        public UrdCodeStd GetStdGdprUrdCode(string gdpaRolename) //03-12-18
        {
            UrdCodeStd rc = UrdCodeStd.Undefined;

            if (gdpaRolename != null)
            {
                if (gdpaRolename == AdminRepo.GdprUrdAdmin)
                    rc = UrdCodeStd.Admin;
                else if (gdpaRolename == AdminRepo.GdprUrdController)
                    rc = UrdCodeStd.Controller;
                else if (gdpaRolename == AdminRepo.GdprUrdStandard)
                    rc = UrdCodeStd.Standard;
                else if (gdpaRolename == AdminRepo.GdprUrdGuest)
                    rc = UrdCodeStd.Guest;
                else if (gdpaRolename == AdminRepo.GdprUrdSystem)
                    rc = UrdCodeStd.System;
                else if (gdpaRolename == AdminRepo.GdprUrdGhost)
                    rc = UrdCodeStd.Ghost;
                else
                {
                    rc = UrdCodeStd.Undefined;
                }
            }
            return rc;
        }

        public string GetStdGdprUrdName(UrdCodeStd roleCode) //03-12-18
        {
            string rc = AdminRepo.GdprUrdNameUndefined;

            if (roleCode == UrdCodeStd.Admin)
                rc = AdminRepo.GdprUrdAdmin;
            else if (roleCode == UrdCodeStd.Controller)
                rc = AdminRepo.GdprUrdController;
            else if (roleCode == UrdCodeStd.Standard)
                rc = AdminRepo.GdprUrdStandard;
            else if (roleCode == UrdCodeStd.Guest)
                rc = AdminRepo.GdprUrdGuest;
            else if (roleCode == UrdCodeStd.System)
                rc = AdminRepo.GdprUrdSystem;
            else if (roleCode == UrdCodeStd.Ghost)
                rc = AdminRepo.GdprUrdGhost;
            else
            {
                rc = AdminRepo.GdprUrdNameUndefined;
            }
            return rc;
        }

        public string GetStdGdprUrdDescription(UrdCodeStd roleCode) //03-12-18
        {
            string rc = GdprUrdDescriptionUndefined;

            if (roleCode == UrdCodeStd.Admin)
                rc = "admin role";
            else if (roleCode == UrdCodeStd.Controller)
                rc = "data controller role";
            else if (roleCode == UrdCodeStd.Standard)
                rc = "standard user role";
            else if (roleCode == UrdCodeStd.Guest)
                rc = "guest user role";
            else if (roleCode == UrdCodeStd.System)
                rc = "system role";
            else if (roleCode == UrdCodeStd.Ghost)
                rc = "ghost users role";
            else
            {
                rc = GdprUrdDescriptionUndefined;
            }
            return rc;
        }

        public string GetStdGdprUrdPurpose(UrdCodeStd roleCode) //03-12-18
        {
            string rc = GdprUrdPurposeUndefined;

            if (roleCode == UrdCodeStd.Admin)            //https://github.com/wpqs/MxAutoGDPR/wiki/Specification:-Admin-Pages
                rc = "purpose of the admin roleis management of user roles, website control and setting status flag when changes are deployed";
            else if (roleCode == UrdCodeStd.Controller)  //https://github.com/wpqs/MxAutoGDPR/wiki/Specification:-Data-Controller-Pages
                rc = "purpose of the data controller role is terms & condition management, website control, notification of users, control of website design and operation including feature processing details and personal data schema";
            else if (roleCode == UrdCodeStd.Standard)
                rc = "purpose of the standard user is someone registered and using the website";
            else if (roleCode == UrdCodeStd.Guest)
                rc = "purpose of the guest user is someone who is not registered, but still using the website";
            else if (roleCode == UrdCodeStd.System)
                rc = "purpose of the system user is allowing tasks that are automated";
            else if (roleCode == UrdCodeStd.Ghost)
                rc = "purpose of the ghost user is someone who has deleted their account so the system no longer holds their personal information";
            else
            {
                rc = GdprUrdPurposeUndefined;
            }
            return rc;
        }

        public string XlatUrdNameToIdentityRoleName(string gdpaUrdName) //03-12-18
        {
            string rc = null;

            if (string.IsNullOrWhiteSpace(gdpaUrdName) == false)
            {
                var urdCodeStd = GetStdGdprUrdCode(gdpaUrdName);

                if (urdCodeStd == UrdCodeStd.Admin)
                    rc = AdminRepo.IdentityRoleAdmin;
                else if (urdCodeStd == UrdCodeStd.Controller)
                    rc = AdminRepo.IdentityRoleController;
                else if (urdCodeStd == UrdCodeStd.Standard)
                    rc = AdminRepo.IdentityRoleStandard;
                else if (urdCodeStd == UrdCodeStd.Guest)
                    rc = AdminRepo.IdentityRoleGuest;
                else if (urdCodeStd == UrdCodeStd.System)
                    rc = AdminRepo.IdentityRoleGuest;
                else if (urdCodeStd == UrdCodeStd.Ghost)
                    rc = AdminRepo.IdentityRoleGuest;
                else
                {
                    rc = gdpaUrdName;
                }
            }
            return rc;
        }
    }
}
