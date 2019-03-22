using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MxReturnCode;

using Gdpr.Domain.Models;

namespace Gdpr.Domain
{
    public interface IAdminRepository : IDisposable
    {
        Task<MxReturnCode<bool>> CreateRoleAsync(string name, int rolecode, string purpose, string description);
        Task<MxReturnCode<bool>> DeleteRoleAsync(GdprUrd role);
        Task<MxReturnCode<bool>> UpdateRoleAsync(GdprUrd role, string name, int rolecode, string purpose, string description, GdprUrd.StatusVal status);

        Task<MxReturnCode<int>> GetRoleCountAsync();

        Task<MxReturnCode<GdprUrd>> GetRoleAsync(string rolename);
        Task<MxReturnCode<GdprUrd>> GetRoleAsync(GdprRpd user);
        Task<MxReturnCode<bool>> AssignRoleAsync(GdprUrd role, GdprRpd user); //all users have a role; it is changed to 'Dormant' rather than deleted

        Task<MxReturnCode<List<GdprUrd>>> GetAllRolesAsync();
        Task<MxReturnCode<List<GdprRpd>>> GetUsersWithRoleAsync(GdprUrd role);
        Task<MxReturnCode<List<GdprFpd>>> GetProcessingForRole(GdprUrd role);
        Task<MxReturnCode<List<GdprWst>>> GetWebsiteTermsForRole(GdprUrd role);

    }
}
