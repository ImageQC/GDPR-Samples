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
        Task<MxReturnCode<GdprRpd>> GetRpdAsync(string email);
        Task<MxReturnCode<bool>> CreateRpdAsync(string email);
    }
}
