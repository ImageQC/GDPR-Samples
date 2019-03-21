
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using MxReturnCode;

using Gdpr.Domain;
using Microsoft.Extensions.Configuration;
using Gdpr.UI.WebApp.Services;

namespace Gdpr.UI.WebApp.Data
{
    public class SeedDb //03-12-18 added
    {
        public async static Task<MxReturnCode<int>> EnsureDataPresentAsync(IConfiguration config, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            MxReturnCode<int> rc = new MxReturnCode<int>("SeedDbIdentity.EnsureSeedDataAsync()", -1, "admin@imageqc.com");

            if ((config == null) || (userManager == null) || (roleManager == null))
                rc.SetError(3150101, MxError.Source.Param, "configuration, userManager or roleManager is null", MxMsgs.MxErrUnexpected);
            else
            {
                try
                {
                    Guid wst = new Guid();
                    int recordCnt = 0;
                    var conn = config.GetConnectionString("DefaultConnection");
                    using (IControllerRepository ctrlRepo = new ControllerRepository(conn))
                    {           //create standard T&C
                        var resGet = await ctrlRepo.GetTermsConditionsAsync(config["WST:Standard"]);
                        rc += resGet;
                        if ((resGet.IsSuccess()) && (resGet.GetResult() != null))
                            wst = resGet.GetResult().Id;
                        else
                        {
                            var resCreate = await ctrlRepo.CreateTermsConditionsAsync(config["WST:Standard"]);
                            rc += resCreate;
                            if (resCreate.IsSuccess())
                            {
                                resGet = await ctrlRepo.GetTermsConditionsAsync(config["WST:Standard"]);
                                if (resGet.IsSuccess())
                                {
                                    if (resGet.GetResult() == null)
                                        rc.SetError(3150102, MxError.Source.Data, @"wst cannot be created", MxMsgs.MxErrUnexpected);
                                    else
                                    {
                                        wst = resGet.GetResult().Id;
                                        recordCnt++;
                                    }
                                }
                            }
                        }
                    }
                    var tmp = rc.GetErrorCode();
                    if (rc.IsSuccess())
                    {
                        if (wst == Guid.Empty)
                            rc.SetError(3150103, MxError.Source.Sys, @"wst Default not found and cannot be created", MxMsgs.MxErrUnexpected);
                        else
                        {
                            using (IAdminRepository adminRepo = new AdminRepository(conn))
                            {           //create Roles 1) Admin 2) Controller  3) Standard 4) Guest 5) System 6) Ghost
                                if (await IdentityDb.IsExistsRole(adminRepo, roleManager, AdminRepository.GdpaRoleAdmin) == false)
                                {
                                    var res = await IdentityDb.CreateRole(adminRepo, roleManager, wst, AdminRepository.GdpaRoleAdmin);
                                    rc += res;
                                    if (res.IsSuccess())
                                        recordCnt++;
                                }
                                if ((rc.IsSuccess()) && (await IdentityDb.IsExistsRole(adminRepo, roleManager, AdminRepository.GdpaRoleController) == false))
                                {
                                    var res = await IdentityDb.CreateRole(adminRepo, roleManager, wst, AdminRepository.GdpaRoleController);
                                    rc += res;
                                    if (res.IsSuccess())
                                        recordCnt++;
                                }
                                if ((rc.IsSuccess()) && (await IdentityDb.IsExistsRole(adminRepo, roleManager, AdminRepository.GdpaRoleStandard) == false))
                                {
                                    var res = await IdentityDb.CreateRole(adminRepo, roleManager, wst, AdminRepository.GdpaRoleStandard);
                                    rc += res;
                                    if (res.IsSuccess())
                                        recordCnt++;
                                }
                                if ((rc.IsSuccess()) && (await IdentityDb.IsExistsRole(adminRepo, roleManager, AdminRepository.GdpaRoleGuest) == false))
                                {
                                    var res = await IdentityDb.CreateRole(adminRepo, roleManager, wst, AdminRepository.GdpaRoleGuest);
                                    rc += res;
                                    if (res.IsSuccess())
                                        recordCnt++;
                                }
                                if ((rc.IsSuccess()) && (await IdentityDb.IsExistsRole(adminRepo, roleManager, AdminRepository.GdpaRoleSystem) == false))
                                {
                                    var res = await IdentityDb.CreateRole(adminRepo, roleManager, wst, AdminRepository.GdpaRoleSystem);
                                    rc += res;
                                    if (res.IsSuccess())
                                        recordCnt++;
                                }
                                if ((rc.IsSuccess()) && (await IdentityDb.IsExistsRole(adminRepo, roleManager, AdminRepository.GdpaRoleGhost) == false))
                                {
                                    var res = await IdentityDb.CreateRole(adminRepo, roleManager, wst,  AdminRepository.GdpaRoleGhost);
                                    rc += res;
                                    if (res.IsSuccess())
                                        recordCnt++;
                                }
                            }
                            if (rc.IsSuccess())
                            {
                                using (ISysRepository sysRepo = new SysRepository(conn))
                                {
                                    if (await IdentityDb.IsExistsUser(sysRepo, userManager, config["AdminGold:Email"]) == false)
                                    {   //Create AdminGold user
                                        var res = await IdentityDb.CreateUser(sysRepo, userManager, AdminRepository.GdpaRoleAdmin, config["AdminGold:Password"], config["AdminGold:Email"], config["AdminGold:Fullname"]);
                                        rc += res;
                                        if (res.IsSuccess())
                                            recordCnt++;
                                    }
                                }
                            }
                            if (rc.IsSuccess())
                            {
                                rc.SetResult(recordCnt);
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    rc.SetError(3150104, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
                }
            }
            return rc;
        }
    }
}
