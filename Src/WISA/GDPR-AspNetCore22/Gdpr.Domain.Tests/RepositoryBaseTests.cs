using Gdpr.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Gdpr.DomainTests
{
    public class RepositoryBaseTests : IClassFixture<RepositoryBaseFixture>
    {
        private readonly RepositoryBaseFixture _repo;
        public RepositoryBaseTests(RepositoryBaseFixture repo)
        {
            _repo = repo;
        }

        [Fact]
        public void DatabaseOpenError()
        {
            var repo = new RepositoryBase(ConfigSettings.EmptyDbConnectionStr);
            var res = repo.CheckConnection();
            Assert.True(res.IsError(true));
            Assert.Equal("error 1030103: Data connection error. Connection details may be incorrect. Please report this problem.", res.GetErrorUserMsg());
        }

        [Fact]
        public void DatabaseOpenOK()
        {
            var res = _repo.Db.CheckConnection();
            Assert.True(res.IsSuccess());
        }
    }
}
