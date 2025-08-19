using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class DbEventService(IDbContextFactory<BookingDbContext> factory) : IEventService
    {
        private readonly IDbContextFactory<BookingDbContext> _factory = factory;
        async Task IEventService.AddEvent(Event eventObj)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(eventObj).State = EntityState.Added;
            await dbContext.SaveChangesAsync();
        }
        async Task<Event?> IEventService.GetEvent(Guid id)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Events.FindAsync(id);
        }
        async Task<Event?> IEventService.GetEventByUrl(string divisionId, string url)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Events.Where(e => e.DivisionId == divisionId && e.Url == url).AsNoTracking().FirstOrDefaultAsync();
        }
        async Task IEventService.LoadAvailableAtcPositions(Event eventObj)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(eventObj).State = EntityState.Unchanged;
            await dbContext.Entry(eventObj).Collection(e => e.AvailableAtcPositions!).Query()
                    .Include(p => p.AtcPosition).LoadAsync();
            dbContext.Entry(eventObj).State = EntityState.Detached;
        }
        async Task IEventService.RemoveEvent(Event eventObj)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(eventObj).State = EntityState.Deleted;
            await dbContext.SaveChangesAsync();
        }
        async Task IEventService.UpdateEvent(Event eventObj)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            dbContext.Entry(eventObj).State = EntityState.Modified;
            var oldEvent = await dbContext.Events.Where(e => e.Id == eventObj.Id).AsNoTracking().FirstOrDefaultAsync();
            if (oldEvent is not null && (eventObj.BeginTime != oldEvent.BeginTime || eventObj.EndTime != oldEvent.EndTime))
            {
                //If event time is changed
                double beginHourDifferenece = (eventObj.BeginTime - oldEvent.BeginTime).TotalHours;
                double endHourDifferenece = (eventObj.EndTime - oldEvent.EndTime).TotalHours;
                foreach (var position in eventObj.AvailableAtcPositions!)
                {
                    position.BeginTime.AddHours(beginHourDifferenece);
                    position.EndTime.AddHours(endHourDifferenece);
                    dbContext.Entry(position).State = EntityState.Modified;
                    await dbContext.Entry(position).Collection(p => p.Bookings!).LoadAsync();
                    foreach (var booking in position.Bookings!)
                    {
                        if (booking.BeginTime < position.BeginTime || booking.EndTime > position.EndTime)
                        {
                            //If booking is out of range, delete it
                            dbContext.Entry(booking).State = EntityState.Deleted;
                        }
                    }
                }
            }
            await dbContext.SaveChangesAsync();
        }
        async Task<List<Event>> IEventService.GetAllEvents(string divisionId)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Events.Where(e => e.DivisionId == divisionId).AsNoTracking().ToListAsync();
        }
        async Task<List<Event>> IEventService.GetUpcomingEvents(string divisionId)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Events.Where(e => e.DivisionId == divisionId)
                .Where(e => e.BeginTime > DateTime.UtcNow && e.IsVisible).AsNoTracking().ToListAsync();
        }
        async Task<List<Event>> IEventService.GetUpcomingEventsForAtc(string divisionId)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Events.Where(e => e.DivisionId == divisionId)
                .Where(e => e.BeginTime > DateTime.UtcNow && e.IsVisible && e.AvailableAtcPositions != null && e.AvailableAtcPositions.Any()).AsNoTracking().ToListAsync();
        }
    }
}
