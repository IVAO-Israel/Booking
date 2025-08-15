using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class DbEventAtcPositionService(IDbContextFactory<BookingDbContext> factory, IEventService eventService) : IEventAtcPositionService
    {
        private readonly IDbContextFactory<BookingDbContext> _factory = factory;
        private readonly IEventService _eventService = eventService;
        async Task IEventAtcPositionService.AddEventAtcPosition(EventAtcPosition position)
        {
            await _eventService.LoadAvailableAtcPositions(position.Event);
            if (position.Event.AvailableAtcPositions is not null)
            {
                if (!position.HasOverlap())
                {
                    using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
                    position.Event.AvailableAtcPositions.Add(position);
                    dbContext.Entry(position).State = EntityState.Added;
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("Positions are overlapping.");
                }
            }
        }
        async Task<EventAtcPosition?> IEventAtcPositionService.GetEventAtcPosition(Guid id)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.EventAtcPositions.Include(p => p.AtcPosition).FirstOrDefaultAsync(p => p.Id == id);
        }
        async Task<List<EventAtcPosition>> IEventAtcPositionService.GetEventAtcPositions(Event eventObj)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.EventAtcPositions.Include(p => p.AtcPosition).Where(p => p.EventId == eventObj.Id).AsNoTracking().ToListAsync();
        }
        async Task IEventAtcPositionService.RemoveEventAtcPosition(EventAtcPosition position)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(position).State = EntityState.Deleted;
            await dbContext.SaveChangesAsync();
        }
        async Task IEventAtcPositionService.UpdateEventAtcPosition(EventAtcPosition position)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(position).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
        async Task IEventAtcPositionService.LoadBookings(EventAtcPosition position)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(position).State = EntityState.Unchanged;
            await dbContext.Entry(position).Collection(p => p.Bookings!).Query()
                .Include(b => b.EventAtcPosition).LoadAsync();
            dbContext.Entry(position).State = EntityState.Detached;
        }
    }
}
