using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class DbAdministratorService(IDbContextFactory<BookingDbContext> factory) : IAdministratorService
    {
        private readonly IDbContextFactory<BookingDbContext> _factory = factory;
        async Task IAdministratorService.AddAdministrator(Administrator administrator)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(administrator).State = EntityState.Added;
            await dbContext.SaveChangesAsync();
        }
        async Task IAdministratorService.RemoveAdministrator(Administrator administrator)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(administrator).State = EntityState.Deleted;
            await dbContext.SaveChangesAsync();
        }
        async Task<Administrator?> IAdministratorService.GetAdministrator(int IVAOUserId)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Administrators.Where(a => a.IVAOUserId == IVAOUserId)
                .Include(a => a.Roles).AsNoTracking().FirstOrDefaultAsync();
        }
        async Task IAdministratorService.UpdateAdministrator(Administrator administrator)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(administrator).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
        async Task<List<Administrator>> IAdministratorService.GetAllAdministrators(string divisionId)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Administrators.Where(a => a.DivisionId == divisionId)
                .Include(a => a.Roles).AsNoTracking().ToListAsync();
        }
    }
}
