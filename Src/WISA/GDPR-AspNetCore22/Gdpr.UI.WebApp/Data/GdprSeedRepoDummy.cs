using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gdpr.Domain;
using Gdpr.Domain.Models;
using MxReturnCode;

namespace Gdpr.UI.WebApp.Data
{
    public class GdprSeedRepoDummy : IGdprSeedRepo
    {
        public void Dispose()
        {
            //dummy repo so do nothing
        }

        public GdprSeedRepoDummy(string connDb)
        {
            
        }

#pragma warning disable 1998
        public async Task<MxReturnCode<bool>> IsExistStdUrdAsync(string name)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("GdprSeedRepoDummy.IsExistUrdAsync()");

            rc.SetResult(true);

            return rc;
        }

        public async Task<MxReturnCode<bool>> CreateStdUrdAsync(string urdName, Guid wstId)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("GdprSeedRepoDummy.CreateStdUrdAsync()");

            rc.SetResult(true);

            return rc;
        }

        public async Task<MxReturnCode<bool>> DeleteStdUrdAsync(string urdName)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("GdprSeedRepoDummy.DeleteStdUrdAsync()");

            rc.SetResult(true);

            return rc;
        }

        public async Task<MxReturnCode<bool>> CreateWstAsync(string title, string description, string url)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("GdprSeedRepoDummy.CreateWstAsync()");

            rc.SetResult(true);

            return rc;
        }

        public async Task<MxReturnCode<GdprWst>> GetWstAsync(string title)
        {
            MxReturnCode<GdprWst> rc = new MxReturnCode<GdprWst>("GdprSeedRepoDummy.GetWstAsync()");

            rc.SetResult(null);

            return rc;
        }

        public UrdCodeStd GetStdGdprUrdCode(string gdpaUrdname)
        {
            return UrdCodeStd.Undefined;
        }

        public async Task<MxReturnCode<bool>> CreateStdUrdAsync(UrdCodeStd roleCode)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("GdprSeedRepoDummy.CreateUrdAsync()");

            rc.SetResult(true);

            return rc;
        }
#pragma warning restore 1998
    }
}
