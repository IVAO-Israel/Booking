using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class DbEventAtcPositionService (BookingDbContext dbContext, IEventService eventService) : IEventAtcPosition
    {
        private readonly BookingDbContext _dbContext = dbContext;
        private readonly IEventService _eventService = eventService;
        void IEventAtcPosition.AddEventAtcPosition(EventAtcPosition position)
        {
            _eventService.LoadAvailableAtcPositions(position.Event);
            if(position.Event.AvailableAtcPositions is not null)
            {
                if (!position.HasOverlap())
                {
                    position.Event.AvailableAtcPositions.Add(position);
                    _dbContext.Entry(position).State = EntityState.Added;
                } else
                {
                    throw new ArgumentException("Positions are overlapping.");
                }
            }
        }
        async Task<EventAtcPosition?> IEventAtcPosition.GetEventAtcPosition(Guid id)
        {
            return await _dbContext.EventAtcPositions.FindAsync(id);
        }
        async Task<List<EventAtcPosition>> IEventAtcPosition.GetEventAtcPositions(Event eventObj)
        {
            return await _dbContext.EventAtcPositions.Where(p => p.EventId == eventObj.Id).ToListAsync();
        }
        void IEventAtcPosition.RemoveEventAtcPosition(EventAtcPosition position)
        {
            if(_dbContext.Entry(position).State == EntityState.Added)
            {
                _dbContext.Entry(position).State = EntityState.Detached;
            } else
            {
                _dbContext.Entry(position).State = EntityState.Deleted;
            }
        }
        void IEventAtcPosition.UpdateEventAtcPosition(EventAtcPosition position)
        {
            if(_dbContext.Entry(position).State != EntityState.Added)
            {
                _dbContext.Entry(position).State = EntityState.Modified;
            }
        }
    }
}
