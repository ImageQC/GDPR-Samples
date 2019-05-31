using Gdpr.Domain.Models;
using MxReturnCode;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gdpr.Domain
{
    public interface IControllerRepo : IDisposable  //03-12-18
    {
        Task<MxReturnCode<GdprWst>> GetWstAsync(string title);
        Task<MxReturnCode<bool>> IsExistWstAsync(string title);
        Task<MxReturnCode<bool>> CreateWstAsync(string title, string description, string url);
        Task<MxReturnCode<bool>> DeleteWstAsync(string title);

        //Task<MxReturnCode<GdprWst>> GetFpdAsync(string name);
        //Task<MxReturnCode<bool>> IsExistFpdAsync(string name);
        //Task<MxReturnCode<bool>> CreateFpdtAsync(string name, string processingBasis, string processingPurpose);
        //Task<MxReturnCode<bool>> DeleteFpdAsync(string name);


    }
}
