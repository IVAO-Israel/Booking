using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class DbEventService(BookingDbContext dbContext) : IEventService
    {
        private BookingDbContext _dbContext = dbContext;
        void IEventService.AddEvent(Event eventObj)
        {
            _dbContext.Entry(eventObj).State = EntityState.Added;
        }
        async Task<Event?> IEventService.GetEvent(Guid id)
        {
            return await _dbContext.Events.FindAsync(id);
        }
        async Task<Event?> IEventService.GetEventByUrl(string url)
        {
            return await _dbContext.Events.Where(e => e.Url == url).AsNoTracking().FirstOrDefaultAsync();
        }
        void IEventService.LoadAvailableAtcPositions(Event eventObj)
        {
            _dbContext.Entry(eventObj).Collection(e => e.AvailableAtcPositions!).LoadAsync();
        }
        void IEventService.RemoveEvent(Event eventObj)
        {
            if (_dbContext.Entry(eventObj).State == EntityState.Added)
            {
                _dbContext.Entry(eventObj).State = EntityState.Detached;
            }
            else
            {
                _dbContext.Entry(eventObj).State = EntityState.Deleted;
            }
        }
        void IEventService.UpdateEvent(Event eventObj)
        {
            if (_dbContext.Entry(eventObj).State != EntityState.Added)
            {
                _dbContext.Entry(eventObj).State = EntityState.Modified;
            }
        }
        async Task<List<Event>> IEventService.GetAllEvents()
        {
            return await _dbContext.Events.AsNoTracking().ToListAsync();
        }
        async Task<List<Event>> IEventService.GetUpcomingEvents()
        {
            return await _dbContext.Events.Where(e => e.BeginTime > DateTime.UtcNow && e.IsVisible).AsNoTracking().ToListAsync();
        }
        async Task<List<Event>> IEventService.GetUpcomingEventsForAtc()
        {
            return await _dbContext.Events.Where(e => e.BeginTime > DateTime.UtcNow && e.IsVisible && e.AvailableAtcPositions != null && e.AvailableAtcPositions.Any()).AsNoTracking().ToListAsync();
        }
    }
}
