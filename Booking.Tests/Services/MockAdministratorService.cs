using Booking.Data;
using Booking.Services.Interfaces;
using Booking.Tests.Extensions;

namespace Booking.Tests.Services
{
    internal class MockAdministratorService : IAdministratorService
    {
        private readonly List<Administrator> _administrators = [];
        Task IAdministratorService.AddAdministrator(Administrator administrator)
        {
            _administrators.Add(administrator);
            return Task.CompletedTask;
        }
        Task<Administrator?> IAdministratorService.GetAdministrator(int IVAOUserId)
        {
            return Task.FromResult(_administrators.Where(a => a.IVAOUserId == IVAOUserId).FirstOrDefault());
        }
        Task IAdministratorService.UpdateAdministrator(Administrator administrator)
        {
            _administrators.Update(administrator);
            return Task.CompletedTask;
        }
        Task IAdministratorService.RemoveAdministrator(Administrator administrator)
        {
            _administrators.Remove(administrator);
            return Task.CompletedTask;
        }
        Task<List<Administrator>> IAdministratorService.GetAllAdministrators()
        {
            return Task.FromResult(_administrators);
        }
    }
}
