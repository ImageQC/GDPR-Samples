using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gdpr.Domain.Models;
using MxReturnCode;

namespace Gdpr.Domain
{
    public class GdprSeedRepo : RepositoryBase, IGdprSeedRepo
    {
        private AdminRepo _adminRepo;
        private ControllerRepo _controllerRepo;

        private bool disposed = false;
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposed == false)
                {
                    if (disposing)
                    {
                        //release any resources created by AdminRepository
                        _adminRepo.Dispose();
                        _controllerRepo.Dispose();
                    }
                }
            }
            finally
            {
                this.disposed = true;
                base.Dispose(disposing);    //release any resources created by its base class
            }
        }

        public GdprSeedRepo(string connection) : base(connection)
        {
            _adminRepo = new AdminRepo(connection);
            _controllerRepo = new ControllerRepo(connection);
        }

        public async Task<MxReturnCode<bool>> IsExistStdUrdAsync(string urdName)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("GdprSeedRepo.IsExistUrdAsync()");

            if (_adminRepo.GetStdGdprUrdCode(urdName) == UrdCodeStd.Undefined)
                rc.SetError(1050101, MxError.Source.Param, $"name={urdName ?? "[null]"} is invalid");
            else
                rc += await _adminRepo.IsExistUrdAsync(urdName);

            return rc;
        }

        public async Task<MxReturnCode<bool>> CreateStdUrdAsync(string urdName, Guid wstId)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("GdprSeedRepo.CreateUrdAsync()");

            var urdCode = _adminRepo.GetStdGdprUrdCode(urdName);
            if (urdCode == UrdCodeStd.Undefined)
                rc.SetError(1050201, MxError.Source.Param, $"name={urdName ?? "[null]"} is invalid");
            else
            {
                rc += await _adminRepo.CreateUrdAsync(urdName, (int)urdCode,
                    UrdStatus.PublishProduction,
                    _adminRepo.GetStdGdprUrdPurpose(urdCode),
                    _adminRepo.GetStdGdprUrdDescription(urdCode), wstId);
            }
            return rc;
        }

        public async Task<MxReturnCode<bool>> DeleteStdUrdAsync(string urdName)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("GdprSeedRepo.DeleteStdUrdAsync()");

            var urdCode = _adminRepo.GetStdGdprUrdCode(urdName);
            if (urdCode == UrdCodeStd.Undefined)
                rc.SetError(1050301, MxError.Source.Param, $"name={urdName ?? "[null]"} is invalid");
            else
            {
                rc += await _adminRepo.DeleteUrdAsync(urdName);
            }
            return rc;
        }

        public async Task<MxReturnCode<bool>> CreateWstAsync(string title, string description, string url)
        {
            MxReturnCode<bool> rc = new MxReturnCode<bool>("GdprSeedRepo.CreateWstAsync()");

            rc += await _controllerRepo.CreateWstAsync(title, description, url);

            return rc;
        }

        public async Task<MxReturnCode<GdprWst>> GetWstAsync(string title)
        {
            MxReturnCode<GdprWst> rc = new MxReturnCode<GdprWst> ("GdprSeedRepo.GetWstAsync()");

            rc += await _controllerRepo.GetWstAsync(title);

            return rc;
        }

        public UrdCodeStd GetStdGdprUrdCode(string gdpaUrdname)
        {
            return _adminRepo.GetStdGdprUrdCode(gdpaUrdname);
        }

        public string XlatUrdNameToIdentityRoleName(string gdpaUrdname)
        {
            return _adminRepo.XlatUrdNameToIdentityRoleName(gdpaUrdname);
        }
    }
}
