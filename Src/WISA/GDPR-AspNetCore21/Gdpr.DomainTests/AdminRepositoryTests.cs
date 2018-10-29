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
        public async void GetRoleCountZero()
        {
            var res = await _repo.Db.GetRoleCountAsync();
            Assert.Equal(0, res.GetResult());
        }

        [Fact]
        public async void CRUDFirstRole()
        {
            var resCnt = await _repo.Db.GetRoleCountAsync();
            Assert.Equal(0, resCnt.GetResult());
                    //Create
            var resDone = await _repo.Db.CreateRoleAsync("first role", 1, "purpose", "description");
            Assert.True(resDone.GetResult());

            resCnt = await _repo.Db.GetRoleCountAsync();
            Assert.Equal(1, resCnt.GetResult());
                    //Read
            var resRole = await _repo.Db.GetRoleAsync("first role");
            GdprUrd role = resRole.GetResult();

            Assert.NotNull(role);
            Assert.Equal("first role", role.Name);
            Assert.Equal(1, role.RoleCode);
            Assert.Equal("purpose", role.Purpose);
            Assert.Equal("description", role.Description);
            Assert.Equal((int)GdprUrd.StatusVal.NotImplemented, role.Status);

            //Update
            resDone = await _repo.Db.UpdateRoleAsync(role, "changed role", 10, "changed purpose", "changed description", GdprUrd.StatusVal.Published);
            Assert.True(resDone.GetResult());

            resRole = await _repo.Db.GetRoleAsync("changed role");
            role = resRole.GetResult();

            Assert.NotNull(role);
            Assert.Equal("changed role", role.Name);
            Assert.Equal(10, role.RoleCode);
            Assert.Equal("changed purpose", role.Purpose);
            Assert.Equal("changed description", role.Description);
            Assert.Equal((int)GdprUrd.StatusVal.Published, role.Status);

            //Delete
            resDone = await _repo.Db.DeleteRoleAsync(role);
            Assert.True(resDone.GetResult());

        }
    }
}
