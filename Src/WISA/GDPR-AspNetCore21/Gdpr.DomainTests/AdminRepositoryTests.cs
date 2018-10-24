using Gdpr.Domain;
using System;
using Xunit;


namespace Gdpr.DomainTests
{
    public class AdminRepositoryTests
    {
        [Fact]
        public void GetURDCountZero()
        {
            IAdminRepository repository = new AdminRepository();
            Assert.Equal(0, repository.GetURDCount());
        }
    }
}
