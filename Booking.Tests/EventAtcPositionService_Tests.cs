using Booking.Data;
using Booking.Services.Interfaces;
using Booking.Tests.Services;

namespace Booking.Tests
{
    public class EventAtcPositionService_Tests
    {
        [Fact]
        public async Task AddEventAtcPosition_ShouldStore()
        {
            //Arrange
            IEventService eventService = new MockEventService();
            IEventAtcPositionService service = new MockEventAtcPosition(eventService);
            var eventObj = new Event
            {
                Id = Guid.NewGuid(),
                Name = "New event",
                BeginTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event"
            };
            await eventService.AddEvent(eventObj);
            var id = Guid.NewGuid();
            var position = new EventAtcPosition()
            {
                Id = id,
                Event = eventObj,
                EventId = eventObj.Id,
                BeginTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1)
            };
            await eventService.LoadAvailableAtcPositions(eventObj);
            eventObj.AvailableAtcPositions?.Add(position);

            //Act
            await service.AddEventAtcPosition(position);
            var result = await service.GetEventAtcPosition(id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }
        [Fact]
        public async Task AddEventAtcPosition_ShouldFailOvrelap()
        {
            //Arrange
            IEventService eventService = new MockEventService();
            IEventAtcPositionService service = new MockEventAtcPosition(eventService);
            var eventObj = new Event
            {
                Id = Guid.NewGuid(),
                Name = "New event",
                BeginTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event"
            };
            await eventService.AddEvent(eventObj);
            var position = new EventAtcPosition()
            {
                Id = Guid.NewGuid(),
                Event = eventObj,
                EventId = eventObj.Id,
                BeginTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1)
            };
            await eventService.LoadAvailableAtcPositions(eventObj);
            eventObj.AvailableAtcPositions?.Add(position);
            var id = Guid.NewGuid();
            var position1 = new EventAtcPosition()
            {
                Id = id,
                Event = eventObj,
                EventId = eventObj.Id,
                BeginTime = DateTime.UtcNow.AddMinutes(5),
                EndTime = DateTime.UtcNow.AddHours(1)
            };

            //Act

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddEventAtcPosition(position1));
        }
    }
}
