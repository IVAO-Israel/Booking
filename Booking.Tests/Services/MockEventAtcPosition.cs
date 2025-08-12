using Booking.Data;
using Booking.Services;
using Booking.Services.Interfaces;
using Booking.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
namespace Booking.Tests.Services
{
    internal class MockEventAtcPosition (IEventService eventService) : IEventAtcPosition
    {
        private readonly IEventService _eventService = eventService;
        private readonly List<EventAtcPosition> _positions = [];
        void IEventAtcPosition.AddEventAtcPosition(EventAtcPosition position)
        {
            _eventService.LoadAvailableAtcPositions(position.Event);
            if (position.Event.AvailableAtcPositions is not null)
            {
                if (!position.HasOverlap())
                {
                    position.Event.AvailableAtcPositions.Add(position);
                    _positions.Add(position);
                }
                else
                {
                    throw new ArgumentException("Positions are overlapping.");
                }
            }
        }
        Task<EventAtcPosition?> IEventAtcPosition.GetEventAtcPosition(Guid id)
        {
            return Task.FromResult(_positions.Where(p => p.Id == id).FirstOrDefault());
        }
        Task<List<EventAtcPosition>> IEventAtcPosition.GetEventAtcPositions(Event eventObj)
        {
            return Task.FromResult(_positions.Where(p => p.EventId == eventObj.Id).ToList());
        }
        void IEventAtcPosition.RemoveEventAtcPosition(EventAtcPosition position)
        {
            _positions.Remove(position);
        }
        void IEventAtcPosition.UpdateEventAtcPosition(EventAtcPosition position)
        {
            _positions.Update(position);
        }
    }
}
