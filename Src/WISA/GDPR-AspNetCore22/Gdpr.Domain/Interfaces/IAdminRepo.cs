using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MxReturnCode;

using Gdpr.Domain.Models;

namespace Gdpr.Domain
{
    public interface IAdminRepo : IDisposable
    {
        Task<MxReturnCode<bool>> IsExistUrdAsync(string name);
        Task<MxReturnCode<bool>> CreateUrdAsync(string name, int roleCode, UrdStatus roleStatus, string purpose, string description, Guid wstId);
        Task<MxReturnCode<GdprUrd>> GetUrdAsync(string name);

        Task<MxReturnCode<bool>> UpdateUrdAsync(GdprUrd role, int roleCode, UrdStatus roleStatus, string purpose, string description);
        Task<MxReturnCode<bool>> DeleteUrdAsync(string name);
        Task<MxReturnCode<int>> GetUrdCountAsync();
        Task<MxReturnCode<List<GdprUrd>>> GetUrdAllAsync();

        Task<MxReturnCode<GdprUrd>> GetUrdAsync(GdprRpd user);
        Task<MxReturnCode<List<GdprRpd>>> GetRpdWithUrdAsync(GdprUrd role);
        Task<MxReturnCode<List<GdprFpd>>> GetFdpForUrdAsync(GdprUrd role);
        Task<MxReturnCode<List<GdprWst>>> GetWstForUrdAsync(GdprUrd role);
        Task<MxReturnCode<bool>> AssignUrdToRpdAsync(GdprUrd role, GdprRpd user);

        string     XlatUrdNameToIdentityRoleName(string gdpaUrdName);
        UrdCodeStd GetStdGdprUrdCode(string gdpaRolename);
        string     GetStdGdprUrdName(UrdCodeStd roleCode);
        string     GetStdGdprUrdDescription(UrdCodeStd roleCode);
        string     GetStdGdprUrdPurpose(UrdCodeStd roleCode);
    }
}
