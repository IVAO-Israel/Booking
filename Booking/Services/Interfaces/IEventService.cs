using Booking.Data;

namespace Booking.Services.Interfaces
{
    public interface IEventService
    {
        public Task AddEvent(Event eventObj);
        public Task<Event?> GetEvent(Guid id);
        public Task<Event?> GetEventByUrl(string divisionId, string url);
        public Task UpdateEvent(Event eventObj);
        public Task RemoveEvent(Event eventObj);
        public Task LoadAvailableAtcPositions(Event eventObj);
        public Task<List<Event>> GetAllEvents(string divisionId);
        public Task<List<Event>> GetUpcomingEvents(string divisionId);
        public Task<List<Event>> GetUpcomingEventsForAtc(string divisionId);
    }
}
