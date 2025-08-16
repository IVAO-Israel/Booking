using Booking.Data;
using Booking.Services.Interfaces;
using Booking.Tests.Services;

namespace Booking.Tests
{
    public class AdministratorService_Tests
    {
        [Fact]
        public async Task AddAdministrator_ShouldStoreAdministrator()
        {
            //Arrange
            IAdministratorService service = new MockAdministratorService();
            var admin = new Administrator
            {
                Id = Guid.NewGuid(),
                IVAOUserId = 123456
            };
            //Act
            service.AddAdministrator(admin);
            var result = await service.GetAdministrator(123456);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(123456, result.IVAOUserId);
        }
        [Fact]
        public async Task GetAdministrator_ShouldBeNull()
        {
            //Arrange
            IAdministratorService service = new MockAdministratorService();

            //Act
            var result = await service.GetAdministrator(123456);

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetAdministrator_ShouldReturnAdministrator()
        {
            //Arrange
            IAdministratorService service = new MockAdministratorService();
            Administrator administrator = new()
            {
                Id = Guid.NewGuid(),
                IVAOUserId = 123456
            };
            service.AddAdministrator(administrator);

            //Act
            var result = await service.GetAdministrator(123456);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(administrator, result);
        }
        [Fact]
        public async Task UpdateAdministrator_ShouldUpdateIVAOUserId()
        {
            //Arrange
            IAdministratorService service = new MockAdministratorService();
            Administrator administrator = new()
            {
                Id = Guid.NewGuid(),
                IVAOUserId = 123456
            };
            service.AddAdministrator(administrator);

            //Act
            administrator.IVAOUserId = 123457;
            service.UpdateAdministrator(administrator);
            var result = await service.GetAdministrator(123457);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(123457, result.IVAOUserId);
        }
        [Fact]
        public async Task RemoveAdministrator_ShouldRemoveAdministrator()
        {
            //Arrange
            IAdministratorService service = new MockAdministratorService();
            var admin = new Administrator
            {
                Id = Guid.NewGuid(),
                IVAOUserId = 123456
            };
            service.AddAdministrator(admin);
            //Act
            service.RemoveAdministrator(admin);
            var result = await service.GetAdministrator(123456);

            //Assert
            Assert.Null(result);
        }
    }
}
