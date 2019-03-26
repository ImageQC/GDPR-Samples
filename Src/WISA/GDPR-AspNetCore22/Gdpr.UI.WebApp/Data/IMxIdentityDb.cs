using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MxReturnCode;

namespace Gdpr.UI.WebApp.Data
{
    public interface IMxIdentityDb
    {
        Task<MxReturnCode<int>> SeedDatabaseAsync();
    }
}
