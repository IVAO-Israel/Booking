using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class DbAdministratorService (BookingDbContext dbContext) : IAdministratorService
    {
        private readonly BookingDbContext _dbContext = dbContext;
        void IAdministratorService.AddAdministrator(Administrator administrator)
        {
            _dbContext.Entry(administrator).State = EntityState.Added;
        }
        void IAdministratorService.RemoveAdministrator(Administrator administrator)
        {
            if(_dbContext.Entry(administrator).State == EntityState.Added){
                _dbContext.Entry(administrator).State = EntityState.Detached;
            } else
            {
                _dbContext.Entry(administrator).State = EntityState.Deleted;
            }
        }
        async Task<Administrator?> IAdministratorService.GetAdministrator(int IVAOUserId)
        {
            return await _dbContext.Administrators.Where(a => a.IVAOUserId  == IVAOUserId).FirstOrDefaultAsync();
        }
        void IAdministratorService.UpdateAdministrator(Administrator administrator)
        {
            if (_dbContext.Entry(administrator).State != EntityState.Added)
            {
                _dbContext.Entry(administrator).State = EntityState.Modified;
            }
        }
    }
}
