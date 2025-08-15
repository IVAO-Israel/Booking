using Booking.Data;
namespace Booking.Services.Interfaces
{
    public interface IEventAtcPositionService
    {
        public Task AddEventAtcPosition(EventAtcPosition position);
        public Task<EventAtcPosition?> GetEventAtcPosition(Guid id);
        public Task<List<EventAtcPosition>> GetEventAtcPositions(Event eventObj);
        public Task UpdateEventAtcPosition(EventAtcPosition position);
        public Task RemoveEventAtcPosition(EventAtcPosition position);
        public Task LoadBookings(EventAtcPosition position);
    }
}
