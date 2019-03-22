using Gdpr.Domain.Models;
using MxReturnCode;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gdpr.Domain
{
    public interface ISysRepository : IDisposable //03-12-18
    {
        Task<MxReturnCode<GdprRpd>> GetUserAsync(string email);
        Task<MxReturnCode<bool>> CreateUserAsync(string email);
    }
}
