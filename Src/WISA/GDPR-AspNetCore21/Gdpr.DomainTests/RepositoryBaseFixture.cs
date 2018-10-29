using System;
using System.Reflection;
using Gdpr.Domain;
using MxReturnCode;

namespace Gdpr.DomainTests
{
    public class RepositoryBaseFixture : IDisposable
    {
        private bool disposed = false;
        public RepositoryBase Db { get; private set; }
        public RepositoryBaseFixture()
        {
            MxUserMsg.Init(Assembly.GetExecutingAssembly(), MxMsgs.SupportedCultures);

            Db = new AdminRepository(ConfigSettings.LocalDbConnectionStr);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposed == false)
                {
                    if (disposing)
                    {
                        if (Db != null)
                            Db.Dispose();
                    }
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}
