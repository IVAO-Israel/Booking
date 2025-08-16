using Booking.Data;

namespace Booking.Services.Interfaces
{
    public interface IAdministratorService
    {
        public void AddAdministrator(Administrator administrator);
        public Task<Administrator?> GetAdministrator(int IVAOUserId);
        public void UpdateAdministrator(Administrator administrator);
        public void RemoveAdministrator(Administrator administrator);
        public Task<List<Administrator>> GetAllAdministrators();
    }
}
