using Booking.Data;
using Booking.Services.Interfaces;
using Booking.Tests.Extensions;

namespace Booking.Tests.Services
{
    internal class MockAdministratorService : IAdministratorService
    {
        private readonly List<Administrator> _administrators = [];
        void IAdministratorService.AddAdministrator(Administrator administrator)
        {
            _administrators.Add(administrator);
        }
        Task<Administrator?> IAdministratorService.GetAdministrator(int IVAOUserId)
        {
            return Task.FromResult(_administrators.Where(a => a.IVAOUserId == IVAOUserId).FirstOrDefault());
        }
        void IAdministratorService.UpdateAdministrator(Administrator administrator)
        {
            _administrators.Update(administrator);
        }
        void IAdministratorService.RemoveAdministrator(Administrator administrator)
        {
            _administrators.Remove(administrator);
        }
    }
}
