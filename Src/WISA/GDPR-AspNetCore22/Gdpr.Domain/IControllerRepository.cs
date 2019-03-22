using Gdpr.Domain.Models;
using MxReturnCode;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gdpr.Domain
{
    public interface IControllerRepository : IDisposable  //03-12-18
    {
        Task<MxReturnCode<GdprWst>> GetTermsConditionsAsync(string email);
        Task<MxReturnCode<bool>> CreateTermsConditionsAsync(string email);

    }
}
