using Booking.Data;
namespace Booking.Services.Interfaces
{
    public interface IEventAtcPosition
    {
        public void AddEventAtcPosition(EventAtcPosition position);
        public Task<EventAtcPosition?> GetEventAtcPosition(Guid id);
        public Task<List<EventAtcPosition>> GetEventAtcPositions(Event eventObj);
        public void UpdateEventAtcPosition(EventAtcPosition position);
        public void RemoveEventAtcPosition(EventAtcPosition position);
    }
}
