using System.Threading.Tasks;
using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        async Task<Event?> IEventService.GetEventByUrl(string url)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Events.Where(e => e.Url == url).AsNoTracking().FirstOrDefaultAsync();
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
            await dbContext.SaveChangesAsync();
        }
        async Task<List<Event>> IEventService.GetAllEvents()
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Events.AsNoTracking().ToListAsync();
        }
        async Task<List<Event>> IEventService.GetUpcomingEvents()
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Events.Where(e => e.BeginTime > DateTime.UtcNow && e.IsVisible).AsNoTracking().ToListAsync();
        }
        async Task<List<Event>> IEventService.GetUpcomingEventsForAtc()
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Events.Where(e => e.BeginTime > DateTime.UtcNow && e.IsVisible && e.AvailableAtcPositions != null && e.AvailableAtcPositions.Any()).AsNoTracking().ToListAsync();
        }
    }
}
