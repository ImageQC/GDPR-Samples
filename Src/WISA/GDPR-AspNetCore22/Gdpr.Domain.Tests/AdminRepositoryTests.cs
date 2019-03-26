using Gdpr.Domain;
using Gdpr.Domain.Models;
using System;
using Xunit;


namespace Gdpr.DomainTests
{
    public class AdminRepositoryTests: IClassFixture<AdminRepositoryFixture>
    {
        private readonly AdminRepositoryFixture _repo;
        public AdminRepositoryTests(AdminRepositoryFixture repo)
        {
            _repo = repo;
        }


        [Fact]
        public async void GetUrdCountZero()
        {
            var res = await _repo.Db.GetUrdCountAsync();
            Assert.Equal(0, res.GetResult());
        }

        [Fact]
        public async void CRUDUrd()
        {
            //await _repo.Db.DeleteUrdAsync("first role");

            //var resCnt = await _repo.Db.GetUrdCountAsync();
            //Assert.Equal(0, resCnt.GetResult());

            //Guid wst = Guid.NewGuid();
            //        //Create
            //var resDone = await _repo.Db.CreateUrdAsync( "first role", (int)UrdCodeStd.Admin, UrdStatus.PublishProduction, "purpose", "description", wst);
            //Assert.True(resDone.GetResult());

            //resCnt = await _repo.Db.GetUrdCountAsync();
            //Assert.Equal(1, resCnt.GetResult());
            //        //Read
            //var resRole = await _repo.Db.GetUrdAsync("first role");
            //GdprUrd role = resRole.GetResult();

            //Assert.NotNull(role);
            //Assert.Equal("first role", role.Name);
            //Assert.Equal((int)UrdCodeStd.Admin, role.RoleCode);
            //Assert.Equal("purpose", role.Purpose);
            //Assert.Equal("description", role.Description);
            //Assert.Equal((int)UrdStatus.PublishProduction, role.Status);

            ////Update
            //resDone = await _repo.Db.UpdateUrdAsync(role, 10, UrdStatus.NotImplemented, "changed purpose", "changed description");
            //Assert.True(resDone.GetResult());
            //resRole = await _repo.Db.GetUrdAsync("first role");
            //role = resRole.GetResult();

            //Assert.NotNull(role);
            //Assert.Equal("first role", role.Name);
            //Assert.Equal(10, role.RoleCode);
            //Assert.Equal("changed purpose", role.Purpose);
            //Assert.Equal("changed description", role.Description);
            //Assert.Equal((int)UrdStatus.NotImplemented, role.Status);

            ////Delete
            //resDone = await _repo.Db.DeleteUrdAsync(role.Name);
            //Assert.True(resDone.GetResult());

        }
    }
}
