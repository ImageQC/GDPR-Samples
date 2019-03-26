using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gdpr.Domain;
using Gdpr.UI.WebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MxReturnCode;

namespace Gdpr.UI.WebApp.Data
{
    public class MxIdentityDb : IMxIdentityDb
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _config;

        public MxIdentityDb(IConfiguration config, RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
        }

        public async Task<MxReturnCode<int>> SeedDatabaseAsync()
        {
            MxReturnCode<int> rc = new MxReturnCode<int>("MxIdentityDb.SeedDatabaseAsync()");

            try
            {
                using (var gdprSeedRepo = new GdprSeedRepo(_config?.GetConnectionString("DefaultConnection"))) //could also be GdprSeedRepoDummy
                {
                    var resWst = await CreateStdTermsConditions(gdprSeedRepo);
                    rc += resWst;
                    if (rc.IsSuccess())
                    {
                        var wst = resWst.GetResult();

                        //var resRole = await SeedStdUrdRole(gdprSeedRepo, AdminRepo.GdprUrdAdmin, wst);
                        //rc += resRole;
                        //if (rc.IsSuccess())
                        //{
                        //    resRole = await SeedStdUrdRole(gdprSeedRepo, AdminRepo.GdprUrdController, wst);
                        //    rc += resRole;
                        //    if (rc.IsSuccess())
                        //    {
                        //        resRole = await SeedStdUrdRole(gdprSeedRepo, AdminRepo.GdprUrdStandard, wst);
                        //        rc += resRole;
                        //        if (rc.IsSuccess())
                        //        {
                        //            resRole = await SeedStdUrdRole(gdprSeedRepo, AdminRepo.GdprUrdSystem, wst);
                        //            rc += resRole;
                        //            if (rc.IsSuccess())
                        //            {
                        //                resRole = await SeedStdUrdRole(gdprSeedRepo, AdminRepo.GdprUrdGuest, wst);
                        //                rc += resRole;
                        //                if (rc.IsSuccess())
                        //                {
                        //                    resRole = await SeedStdUrdRole(gdprSeedRepo, AdminRepo.GdprUrdGhost, wst);
                        //                    rc += resRole;
                        //                    if (rc.IsSuccess())
                        //                    {

                        //                        //create GoldUser - will.stott@maximodex.com

                        //                        rc.SetResult(0);
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        rc.SetResult(0);
                    }
                }
            }
            catch (Exception e)
            {
                rc.SetError(3060101, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnexpected);
            }
            return rc;
        }

        private async Task<MxReturnCode<Guid>> CreateStdTermsConditions(GdprSeedRepo gdprSeedRepo)
        {
            MxReturnCode<Guid> rc = new MxReturnCode<Guid>("MxIdentityDb.CreateStdTermsConditions()");

            if (gdprSeedRepo == null)
                rc.SetError(3060201, MxError.Source.Param, "gdprSeedRepo is null");
            else
            {
                var resGetExisting = await gdprSeedRepo.GetWstAsync(_config["WST:Standard"]);
                rc += resGetExisting;
                if (rc.IsSuccess())
                {
                    if (resGetExisting.GetResult() != null)
                        rc.SetResult(resGetExisting.GetResult().Id);
                    else
                    {
                        var resCreate = await gdprSeedRepo.CreateWstAsync(_config["WST:Standard"], "descr", "url");
                        rc += resCreate;
                        if (rc.IsSuccess())
                        {
                            var resGetNew = await gdprSeedRepo.GetWstAsync(_config["WST:Standard"]);
                            rc += resGetNew;
                            if (rc.IsSuccess())
                            {
                                if (resGetExisting.GetResult() == null)
                                    rc.SetError(3060202, MxError.Source.Data, @"wst cannot be created",
                                        MxMsgs.MxErrUnexpected);
                                else
                                {
                                    var wst = resGetExisting.GetResult().Id;
                                    if (wst == Guid.Empty)
                                        rc.SetError(3060203, MxError.Source.Sys,
                                            @"wst Default not found and cannot be created", MxMsgs.MxErrUnexpected);
                                    else
                                        rc.SetResult(wst);
                                }
                            }
                        }
                    }
                }
            }

            return rc;
        }

        private async Task<MxReturnCode<bool>> SeedStdUrdRole(GdprSeedRepo gdprSeedRepo, string urdName, Guid wstId)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>($"MxIdentityDb.SeedStdUrdRole(name={urdName ?? "[null]"})");

            if ((gdprSeedRepo == null) || (string.IsNullOrWhiteSpace(urdName)) || (gdprSeedRepo.GetStdGdprUrdCode(urdName) == UrdCodeStd.Undefined))
                rc.SetError(3060301, MxError.Source.Param, $"gdprSeedRepo is null or urdName={urdName ?? "[null]"} is invalid");
            else
            {
                var resUrdExists = await gdprSeedRepo.IsExistStdUrdAsync(urdName);
                rc += resUrdExists;
                if (rc.IsSuccess())
                {
                    if (resUrdExists.GetResult() == false)
                    {
                        var resCreate = await gdprSeedRepo.CreateStdUrdAsync(urdName, wstId);
                        rc += resCreate;
                    }
                    if (rc.IsSuccess())
                    {
                        var identityRoleName = gdprSeedRepo.XlatUrdNameToIdentityRoleName(urdName);
                        if (await _roleManager.FindByNameAsync(identityRoleName) != null)
                            rc.SetResult(true);
                        else
                        {
                            var idres = await _roleManager.CreateAsync(new IdentityRole() { Name = identityRoleName });
                            if (idres.Succeeded)
                                rc.SetError(3060302, MxError.Source.Sys, $"unable to create Identity Role {identityRoleName ?? "[null]"}");
                            else
                                rc.SetResult(true);
                        }
                    }
                    if (rc.IsError(true))
                    {
                        await gdprSeedRepo.DeleteStdUrdAsync(urdName);
                    }
                }
            }
            return rc;
        }

        //public static async Task<MxReturnCode<int>> CreateUser(ISysRepository repo, UserManager<IdentityUser> userManager, string gdprRoleName, string password, string email, string fullName)
        //{
        //    MxReturnCode<int> rc = new MxReturnCode<int>("IdentityDomain.CreateUser()", -1);

        //    if ((repo == null) || (userManager == null) || (gdprRoleName == null) || (password == null) || (email == null) || (fullName == null))
        //        rc.SetError(3050101, MxError.Source.Param, "repo, userManager, roleName, password, email, or fullname is null", MxMsgs.MxErrUnexpected);
        //    else
        //    {
        //        try
        //        {
        //            if (await IsExistsUser(repo, userManager, email) == true)
        //                rc.SetResult(0);
        //            else
        //            {
        //                if (await userManager.FindByEmailAsync(email) == null)
        //                {
        //                    IdentityUser user = new IdentityUser()
        //                    {
        //                        UserName = email,
        //                        Email = email
        //                    };
        //                    await userManager.CreateAsync(user, password);
        //                    IdentityResult result = await userManager.AddToRoleAsync(user, gdprRoleName);
        //                    if (result.Succeeded == false)
        //                        rc.SetError(3050102, MxError.Source.Sys, WebErrorHandling.GetIdentityErrors(result, $"cannot create role {gdprRoleName}"));
        //                }
        //                if (rc.IsSuccess())
        //                {
        //                    //if (await repository.IsGdprUserExists(email) )

        //                    //var resCnt = await repository.CreateGDPRUser();
        //                    //rc += resCnt;
        //                    //if (res.IsSuccess())
        //                    //{
        //                    //   rc.SetResult(true);
        //                    //}
        //                }
        //                rc.SetResult(1);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            rc.SetError(3050103, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
        //        }
        //        if (rc.IsError(true))
        //        {
        //            var user = await userManager.FindByEmailAsync(email);
        //            if (user != null)
        //                await userManager.DeleteAsync(user);
        //        }
        //    }
        //    return rc;
        //}

        //public static async Task<bool> IsExistsUser(ISysRepository repo, UserManager<IdentityUser> userManager, string email)
        //{
        //    MxReturnCode<bool> rc = new MxReturnCode<bool>("IdentityDomain.CreateUser()", false);

        //    if ((repo == null) || (userManager == null) || (email == null))
        //        rc.SetError(3050201, MxError.Source.Param, "repo, userManager or email is null", MxMsgs.MxErrUnexpected);
        //    else
        //    {
        //        try
        //        {

        //            //check identity
        //            var result = await repo.GetUserAsync(email);
        //            rc += result;
        //            if (result.IsSuccess())
        //                rc.SetResult(true);
        //            else
        //                rc.SetResult(false);
        //        }
        //        catch (Exception e)
        //        {
        //            rc.SetError(3050202, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
        //        }
        //    }
        //    return rc.GetResult();
        //}

        ////if used without GDPR then mock repo so method succeeds
        //public static async Task<MxReturnCode<bool>> CreateRole(IAdminRepo repo, RoleManager<IdentityRole> roleManager, Guid wst, string gdprRoleName)
        //{
        //    MxReturnCode<bool> rc = new MxReturnCode<bool>("IdentityDomain.CreateRole()");

        //    string identityRoleName = null;
        //    if ((repo == null) || (roleManager == null) || (gdprRoleName == null) || ((identityRoleName = repo?.XlatToIdentityRoleName(gdprRoleName)) == null))
        //        rc.SetError(3050301, MxError.Source.Param, $"repo, roleManager null or gdprRoleName={gdprRoleName ?? "[null]"} identityRoleName={identityRoleName ?? "[null]"}", MxMsgs.MxErrUnexpected);
        //    else
        //    {
        //        try
        //        {
        //            if (await roleManager.RoleExistsAsync(identityRoleName) == false)
        //            {
        //                IdentityResult result = await roleManager.CreateAsync(new IdentityRole { Name = identityRoleName });
        //                if (result.Succeeded == false)
        //                    rc.SetError(3050302, MxError.Source.Sys, WebErrorHandling.GetIdentityErrors(result, $"cannot create role {identityRoleName}"));

        //            }
        //            if (rc.IsSuccess())
        //            {
        //                var result = await repo.GetRoleAsync(gdprRoleName);
        //                if ((result.IsSuccess()) && (result.GetResult() != null))
        //                    rc.SetResult(true); //for some reason it already exists - not expected, but not error
        //                else
        //                {
        //                    // var resCnt = await repo.CreateRoleAsync(gdprRoleName);
        //                    //rc += resCnt;
        //                    //if (res.IsSuccess())
        //                    //{
        //                    //   rc.SetResult(true);
        //                    //}

        //                }
        //                //if gdprname exists - setresult 

        //                //create GDPR records; WST + URD and WXR record for each role 1) Admin 2) Controller  3) Standard 4) Guest 5) System (Guest) 6) Ghost (Guest)


        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            rc.SetError(3050303, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
        //        }
        //    }
        //    if (rc.IsError(true))
        //    {
        //        var role = await roleManager.FindByNameAsync(identityRoleName);
        //        if (role != null)
        //            await roleManager.DeleteAsync(role);
        //    }
        //    return rc;
        //}

        //public static async Task<bool> IsExistsRole(RoleManager<IdentityRole> roleManager, string identityRoleName)
        //{
        //    MxReturnCode<bool> rc = new MxReturnCode<bool>($"IdentityDomain.IsExistsRole({identityRoleName ?? "[null]"})", false);

        //    if ((roleManager == null) || (identityRoleName == null))
        //        rc.SetError(3050401, MxError.Source.Param, $"roleManager={((roleManager == null) ? "not null" : "[null]")} or identityRoleName={identityRoleName ?? "[null]"}", MxMsgs.MxErrUnexpected);
        //    else
        //    {
        //        try
        //        {
        //            var role = await roleManager.FindByNameAsync(identityRoleName);
        //            rc.SetResult(role != null);
        //        }
        //        catch (Exception e)
        //        {
        //            rc.SetError(3050402, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
        //        }
        //    }
        //    return rc.GetResult();
        //}
    }
}
