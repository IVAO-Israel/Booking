using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Booking.Data;
using Booking.Services.Interfaces;
using Booking.Tests.Extensions;

namespace Booking.Tests.Services
{
    internal class MockEventService : IEventService
    {
        private readonly List<Event> _events = [];
        void IEventService.AddEvent(Event eventObj)
        {
            _events.Add(eventObj);
        }
        Task<Event?> IEventService.GetEvent(Guid id)
        {
            return Task.FromResult(_events.Where(e =>  e.Id == id).FirstOrDefault());
        }
        Task<Event?> IEventService.GetEventByUrl(string url)
        {
            return Task.FromResult(_events.Where(e => e.Url == url).FirstOrDefault());
        }
        void IEventService.LoadAvailableAtcPositions(Event eventObj)
        {
            eventObj.AvailableAtcPositions = [];
        }
        void IEventService.RemoveEvent(Event eventObj)
        {
            _events.Remove(eventObj);
        }
        void IEventService.UpdateEvent(Event eventObj)
        {
            _events.Update(eventObj);
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
            return Task.FromResult(_events.Where(e => e.BeginTime > DateTime.UtcNow && e.IsVisible && e.AvailableAtcPositions is not null && e.AvailableAtcPositions.Any()).ToList());
        }
    }
}
