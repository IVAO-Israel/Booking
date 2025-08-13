using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class DbEventAtcPositionService(BookingDbContext dbContext, IEventService eventService) : IEventAtcPositionService
    {
        private readonly BookingDbContext _dbContext = dbContext;
        private readonly IEventService _eventService = eventService;
        void IEventAtcPositionService.AddEventAtcPosition(EventAtcPosition position)
        {
            _eventService.LoadAvailableAtcPositions(position.Event);
            if (position.Event.AvailableAtcPositions is not null)
            {
                if (!position.HasOverlap())
                {
                    position.Event.AvailableAtcPositions.Add(position);
                    _dbContext.Entry(position).State = EntityState.Added;
                }
                else
                {
                    throw new ArgumentException("Positions are overlapping.");
                }
            }
        }
        async Task<EventAtcPosition?> IEventAtcPositionService.GetEventAtcPosition(Guid id)
        {
            return await _dbContext.EventAtcPositions.Include(p => p.AtcPosition).FirstOrDefaultAsync(p => p.Id == id);
        }
        async Task<List<EventAtcPosition>> IEventAtcPositionService.GetEventAtcPositions(Event eventObj)
        {
            return await _dbContext.EventAtcPositions.Include(p => p.AtcPosition).Where(p => p.EventId == eventObj.Id).AsNoTracking().ToListAsync();
        }
        void IEventAtcPositionService.RemoveEventAtcPosition(EventAtcPosition position)
        {
            if (_dbContext.Entry(position).State == EntityState.Added)
            {
                _dbContext.Entry(position).State = EntityState.Detached;
            }
            else
            {
                _dbContext.Entry(position).State = EntityState.Deleted;
            }
        }
        void IEventAtcPositionService.UpdateEventAtcPosition(EventAtcPosition position)
        {
            if (_dbContext.Entry(position).State != EntityState.Added)
            {
                _dbContext.Entry(position).State = EntityState.Modified;
            }
        }
        async Task IEventAtcPositionService.LoadBookings(EventAtcPosition position)
        {
            _dbContext.Entry(position).State = EntityState.Unchanged;
            await _dbContext.Entry(position).Collection(p => p.Bookings!).Query()
                .Include(b => b.EventAtcPosition).LoadAsync();
            _dbContext.Entry(position).State = EntityState.Detached;
        }
    }
}
