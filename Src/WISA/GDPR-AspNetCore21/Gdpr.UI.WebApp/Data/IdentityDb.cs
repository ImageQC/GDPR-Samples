using Gdpr.Domain;
using Gdpr.UI.WebApp.Services;
using Microsoft.AspNetCore.Identity;
using MxReturnCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gdpr.UI.WebApp.Data
{
    public class IdentityDb //03-12-18 added
    {
        public async static Task<MxReturnCode<int>> CreateUser(ISysRepository repo, UserManager<IdentityUser> userManager, string gdprRoleName, string password, string email, string fullName)
        {
            MxReturnCode<int> rc = new MxReturnCode<int>("IdentityDb.CreateUser()", -1);

            if ((repo == null) || (userManager == null) || (gdprRoleName == null) || (password == null) || (email == null) || (fullName == null))
                rc.SetError(3160101, MxError.Source.Param, "repo, userManager, roleName, password, email, or fullname is null", MxMsgs.MxErrUnexpected);
            else
            {
                try
                {
                    if (await IsExistsUser(repo, userManager, email) == true)
                        rc.SetResult(0);
                    else
                    {
                        if (await userManager.FindByEmailAsync(email) == null)
                        {
                            IdentityUser user = new IdentityUser()
                            {
                                UserName = email,
                                Email = email
                            };
                            await userManager.CreateAsync(user, password);
                            IdentityResult result = await userManager.AddToRoleAsync(user, gdprRoleName);
                            if (result.Succeeded == false)
                                rc.SetError(3160102, MxError.Source.Sys, WebErrorHandling.GetIdentityErrors(result, $"cannot create role {gdprRoleName}"));
                        }
                        if (rc.GetErrorCode() == MxErrorLog.UnknownError)
                        {
                                //if (await repository.IsGdprUserExists(email) )

                                //var resCnt = await repository.CreateGDPRUser();
                                //rc += resCnt;
                                //if (res.IsSuccess())
                                //{
                                //   rc.SetResult(true);
                                //}
                        }
                        rc.SetResult(1);
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(3160102, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
                }
                if (rc.IsError())
                {
                    var user = await userManager.FindByEmailAsync(email);
                    if (user != null)
                        await userManager.DeleteAsync(user);
                }
            }
            return rc;
        }

        public async static Task<bool> IsExistsUser(ISysRepository repo, UserManager<IdentityUser> userManager, string email)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("IdentityDb.CreateUser()", false);

            if ((repo == null) || (userManager == null) || (email == null))
                rc.SetError(3160201, MxError.Source.Param, "repo, userManager or email is null", MxMsgs.MxErrUnexpected);
            else
            {
                try
                {

                    //check identity
                    var result = await repo.GetUserAsync(email);
                    rc += result;
                    if (result.IsSuccess())
                        rc.SetResult(true);
                    else
                        rc.SetResult(false);
                }
                catch (Exception e)
                {
                    rc.SetError(3160202, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
                }
            }
            return rc.GetResult();
        }

        public async static Task<MxReturnCode<bool>> CreateRole(IAdminRepository repo, RoleManager<IdentityRole> roleManager, Guid wst, string gdprRoleName)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("IdentityDb.CreateRole()");

            var identityRoleName = AdminRepository.GetIdentityRolename(gdprRoleName);
            if ((repo == null) || (roleManager == null) || (gdprRoleName == null) || (identityRoleName == null))
                rc.SetError(3160301, MxError.Source.Param, "repo, roleManager or gdprRoleName is null", MxMsgs.MxErrUnexpected);
            else
            {
                try
                {
                    if (await roleManager.RoleExistsAsync(identityRoleName) == false)
                    {
                        IdentityResult result = await roleManager.CreateAsync(new IdentityRole { Name = identityRoleName });
                        if (result.Succeeded == false)
                            rc.SetError(3160302, MxError.Source.Sys, WebErrorHandling.GetIdentityErrors(result, $"cannot create role {identityRoleName}"));

                    }
                    if (rc.GetErrorCode() == MxErrorLog.UnknownError)
                    {
                            //if gdprname exists - setresult 

                            //create GDPR records; WST + URD and WXR record for each role 1) Admin 2) Controller  3) Standard 4) Guest 5) System (Guest) 6) Ghost (Guest)

                            //var resCnt = await repository.CreateGDPRRole(gdprRoleName != null ? gdprRoleName : identityRoleName);
                            //rc += resCnt;
                            //if (res.IsSuccess())
                            //{
                            //   rc.SetResult(true);
                            //}
                    }
                }
                catch (Exception e)
                {
                    rc.SetError(3160303, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
                }
            }
            if (rc.IsError())
            {
                //var role = await roleManager.FindByNameAsync(identityRoleName);
                //if (role != null)
                //    await roleManager.DeleteAsync(role);
            }
            return rc;
        }

        public async static Task<bool> IsExistsRole(IAdminRepository repo, RoleManager<IdentityRole> roleManager, string gdprRolename)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("IdentityDb.IsExistsRole()", false);

            if ((repo == null) || (roleManager == null) || (gdprRolename == null))
                rc.SetError(3160401, MxError.Source.Param, "repo, roleManager or rolename is null", MxMsgs.MxErrUnexpected);
            else
            {
                try
                {
                    var identityRolename = AdminRepository.GetIdentityRolename(gdprRolename);
                    //check identity
                    var result = await repo.GetRoleAsync(gdprRolename);
                    rc += result;
                    if (result.IsSuccess())
                        rc.SetResult(true);
                    else
                        rc.SetResult(false);
                }
                catch (Exception e)
                {
                    rc.SetError(3160402, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
                }
            }
            return rc.GetResult();
        }
    }

}
