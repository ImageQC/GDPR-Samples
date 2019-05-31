using Gdpr.Domain.Models;
using MxReturnCode;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gdpr.Domain
{
    public interface ISystemRepo : IDisposable //03-12-18
    {

        Task<MxReturnCode<bool>> IsExistRpdAsync(string email);
        Task<MxReturnCode<bool>> CreateRpdAsync(string email, string fullName, RpdChildFlag childFlag, string roleName, Guid identityUserId);
        Task<MxReturnCode<GdprRpd>> GetRpdAsync(string email);
        Task<MxReturnCode<bool>> UpdateRpdAsync(string email, string fullName, RpdChildFlag childFlag);
        Task<MxReturnCode<bool>> DeleteRpdAsync(string email);
        Task<MxReturnCode<int>> GetRpdCountAsync();
    }
}
