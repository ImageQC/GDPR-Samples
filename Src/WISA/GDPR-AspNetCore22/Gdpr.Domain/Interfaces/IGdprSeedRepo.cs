using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gdpr.Domain.Models;
using MxReturnCode;

namespace Gdpr.Domain
{
    public interface IGdprSeedRepo : IDisposable
    {
       // Task<MxReturnCode<bool>> IsExistRpdAsync(string email);
       // Task<MxReturnCode<bool>> CreateRpdAsync(string fullname, string email, RpdChildFlag child, int roleCode);
  
        Task<MxReturnCode<bool>> IsExistStdUrdAsync(string urdName);
        Task<MxReturnCode<bool>> CreateStdUrdAsync(string urdName, Guid wstId);
        Task<MxReturnCode<bool>>DeleteStdUrdAsync(string urdName);

        Task<MxReturnCode<bool>> CreateWstAsync(string title, string description, string url);
        Task<MxReturnCode<GdprWst>> GetWstAsync(string title);

        UrdCodeStd GetStdGdprUrdCode(string gdpaUrdname);

    }
}
