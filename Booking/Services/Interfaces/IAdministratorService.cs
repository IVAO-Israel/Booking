using Booking.Data;

namespace Booking.Services.Interfaces
{
    public interface IAdministratorService
    {
        public Task AddAdministrator(Administrator administrator);
        public Task<Administrator?> GetAdministrator(int IVAOUserId);
        public Task UpdateAdministrator(Administrator administrator);
        public Task RemoveAdministrator(Administrator administrator);
        public Task<List<Administrator>> GetAllAdministrators(string divisionId);
    }
}
