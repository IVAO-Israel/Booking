using Booking.Data;

namespace Booking.Services.Interfaces
{
    public interface IEventService
    {
        public void AddEvent(Event eventObj);
        public Task<Event?> GetEvent(Guid id);
        public Task<Event?> GetEventByUrl(string url);
        public void UpdateEvent(Event eventObj);
        public void RemoveEvent(Event eventObj);
        public void LoadAvailableAtcPositions(Event eventObj);
        public Task<List<Event>> GetAllEvents();
        public Task<List<Event>> GetUpcomingEvents();
        public Task<List<Event>> GetUpcomingEventsForAtc();
    }
}
