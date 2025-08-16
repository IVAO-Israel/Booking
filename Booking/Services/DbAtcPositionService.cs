using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class DbAtcPositionService (IDbContextFactory<BookingDbContext> factory) : IAtcPositionService
    {
        private readonly IDbContextFactory<BookingDbContext> _factory = factory;
        async Task IAtcPositionService.AddAtcPosition(AtcPosition position)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(position).State = EntityState.Added;
            await dbContext.SaveChangesAsync();
        }
        async Task<List<AtcPosition>> IAtcPositionService.GetAllAtcPositions()
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.AtcPositions.AsNoTracking().ToListAsync();
        }
        async Task<AtcPosition?> IAtcPositionService.GetAtcPosition(Guid id)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.AtcPositions.FindAsync(id);
        }
        async Task<AtcPosition?> IAtcPositionService.GetAtcPosition(string IVAOCallsign)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.AtcPositions.Where(a => a.IVAOCallsign == IVAOCallsign).FirstOrDefaultAsync();
        }
        async Task IAtcPositionService.RemoveAtcPosition(AtcPosition position)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(position).State = EntityState.Deleted;
            await dbContext.SaveChangesAsync();
        }
        async Task IAtcPositionService.UpdateAtcPosition(AtcPosition position)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(position).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
