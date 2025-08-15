using Booking.Data;

namespace Booking.Services.Interfaces
{
    public interface IEventService
    {
        public Task AddEvent(Event eventObj);
        public Task<Event?> GetEvent(Guid id);
        public Task<Event?> GetEventByUrl(string url);
        public Task UpdateEvent(Event eventObj);
        public Task RemoveEvent(Event eventObj);
        public Task LoadAvailableAtcPositions(Event eventObj);
        public Task<List<Event>> GetAllEvents();
        public Task<List<Event>> GetUpcomingEvents();
        public Task<List<Event>> GetUpcomingEventsForAtc();
    }
}
