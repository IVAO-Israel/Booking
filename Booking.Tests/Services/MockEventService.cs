using Booking.Data;
using Booking.Services.Interfaces;
using Booking.Tests.Extensions;

namespace Booking.Tests.Services
{
    internal class MockEventService : IEventService
    {
        private readonly List<Event> _events = [];
        Task IEventService.AddEvent(Event eventObj)
        {
            _events.Add(eventObj);
            return Task.CompletedTask;
        }
        Task<Event?> IEventService.GetEvent(Guid id)
        {
            return Task.FromResult(_events.Where(e => e.Id == id).FirstOrDefault());
        }
        Task<Event?> IEventService.GetEventByUrl(string url)
        {
            return Task.FromResult(_events.Where(e => e.Url == url).FirstOrDefault());
        }
        Task IEventService.LoadAvailableAtcPositions(Event eventObj)
        {
            if (eventObj.AvailableAtcPositions is null)
            {
                eventObj.AvailableAtcPositions = [];
            }
            return Task.CompletedTask;
        }
        Task IEventService.RemoveEvent(Event eventObj)
        {
            _events.Remove(eventObj);
            return Task.CompletedTask;
        }
        Task IEventService.UpdateEvent(Event eventObj)
        {
            _events.Update(eventObj);
            return Task.CompletedTask;
        }
        Task<List<Event>> IEventService.GetAllEvents()
        {
            return Task.FromResult(_events);
        }
        Task<List<Event>> IEventService.GetUpcomingEvents()
        {
            return Task.FromResult(_events.Where(e => e.BeginTime > DateTime.UtcNow && e.IsVisible).ToList());
        }
        Task<List<Event>> IEventService.GetUpcomingEventsForAtc()
        {
            return Task.FromResult(_events.Where(e => e.BeginTime > DateTime.UtcNow && e.IsVisible && e.AvailableAtcPositions != null && e.AvailableAtcPositions.Any()).ToList());
        }
    }
}
