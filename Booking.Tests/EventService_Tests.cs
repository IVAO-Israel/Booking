using Booking.Data;
using Booking.Services.Interfaces;
using Booking.Tests.Services;

namespace Booking.Tests
{
    public class EventService_Tests
    {
        [Fact]
        public async Task AddEvent_ShouldStoreEvent()
        {
            //Arrange
            IEventService service = new MockEventService();
            Guid id = Guid.NewGuid();
            var e = new Event
            {
                Id = id,
                Name = "New event",
                BeginTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event"
            };
            //Act
            service.AddEvent(e);
            var result = await service.GetEvent(id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("New event", result.Name);
        }
        [Fact]
        public async Task GetEvent_ShouldBeNull()
        {
            //Arrange
            IEventService service = new MockEventService();

            //Act
            var result = await service.GetEvent(Guid.NewGuid());

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetEvent_ShouldReturnEvent()
        {
            //Arrange
            IEventService service = new MockEventService();
            Guid id = Guid.NewGuid();
            Event e = new ()
            {
                Id = id,
                Name = "New event",
                BeginTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event"
            };
            service.AddEvent(e);

            //Act
            var result = await service.GetEvent(id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(e, result);
        }
        [Fact]
        public async Task UpdateEvent_ShouldUpdateName()
        {
            //Arrange
            IEventService service = new MockEventService();
            Guid id = Guid.NewGuid();
            Event e = new()
            {
                Id = id,
                Name = "New event",
                BeginTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event"
            };
            service.AddEvent(e);

            //Act
            e.Name = "New event name";
            service.UpdateEvent(e);
            var result = await service.GetEvent(id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("New event name", result.Name);
        }
        [Fact]
        public async Task RemoveEvent_ShouldRemoveEvent()
        {
            //Arrange
            IEventService service = new MockEventService();
            Guid id = Guid.NewGuid();
            Event e = new()
            {
                Id = id,
                Name = "New event",
                BeginTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event"
            };
            service.AddEvent(e);

            //Act
            service.RemoveEvent(e);
            var result = await service.GetEvent(id);

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task LoadAvailableAtcPositions_ShouldReturnNull()
        {
            //Arrange
            IEventService service = new MockEventService();
            Guid id = Guid.NewGuid();
            Event e = new()
            {
                Id = id,
                Name = "New event",
                BeginTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event"
            };
            service.AddEvent(e);

            //Act
            var result = await service.GetEvent(id);

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.AvailableAtcPositions);
        }
        [Fact]
        public async Task LoadAvailableAtcPositions_ShouldReturnList()
        {
            //Arrange
            IEventService service = new MockEventService();
            Guid id = Guid.NewGuid();
            Event e = new()
            {
                Id = id,
                Name = "New event",
                BeginTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event"
            };
            service.AddEvent(e);

            //Act
            service.LoadAvailableAtcPositions(e);
            var result = await service.GetEvent(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.AvailableAtcPositions);
        }
        [Fact]
        public async Task GetAllEvents_ShouldReturnList()
        {
            //Arrange
            IEventService service = new MockEventService();
            service.AddEvent(new()
            {
                Id = Guid.NewGuid(),
                Name = "New event",
                BeginTime = DateTime.UtcNow.AddHours(5),
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event"
            });
            service.AddEvent(new()
            {
                Id = Guid.NewGuid(),
                Name = "New event1",
                BeginTime = DateTime.UtcNow.AddHours(-1),
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event1"
            });

            //Act
            var result = await service.GetAllEvents();

            //Assert
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public async Task GetUpcomingEvents_ShouldReturnList()
        {
            //Arrange
            IEventService service = new MockEventService();
            service.AddEvent(new()
            {
                Id = Guid.NewGuid(),
                Name = "New event",
                BeginTime = DateTime.UtcNow.AddHours(5),
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event"
            });
            service.AddEvent(new()
            {
                Id = Guid.NewGuid(),
                Name = "New event1",
                BeginTime = DateTime.UtcNow.AddHours(-1),
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event1"
            });

            //Act
            var result = await service.GetUpcomingEvents();

            //Assert
            Assert.Single(result);
        }
        [Fact]
        public async Task GetUpcomingEventsForAtc_ShouldReturnList()
        {
            //Arrange
            IEventService service = new MockEventService();
            List<EventAtcPosition> positions = [];
            Event e = new()
            {
                Id = Guid.NewGuid(),
                Name = "New event",
                BeginTime = DateTime.UtcNow.AddHours(5),
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event",
                AvailableAtcPositions = positions
            };
            service.AddEvent(e);
            positions.Add(new EventAtcPosition() { 
                EventId = e.Id,
                Event = e, 
                BeginTime = DateTime.UtcNow.AddHours(5), 
                EndTime = DateTime.UtcNow.AddDays(1) 
            });
            service.AddEvent(new()
            {
                Id = Guid.NewGuid(),
                Name = "New event1",
                BeginTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1),
                IsVisible = true,
                Url = "new-event1"
            });

            //Act
            var result = await service.GetUpcomingEventsForAtc();
            var first = result.FirstOrDefault();

            //Assert
            Assert.Single(result);
            Assert.NotNull(first);
            Assert.NotNull(first.AvailableAtcPositions);
            Assert.Single(first.AvailableAtcPositions);
        }
    }
}
