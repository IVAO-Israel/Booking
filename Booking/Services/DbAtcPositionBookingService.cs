using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class DbAtcPositionBookingService(IDbContextFactory<BookingDbContext> factory, IEventAtcPositionService eventAtcPositionService) : IAtcPositionBookingService
    {
        private readonly IDbContextFactory<BookingDbContext> _factory = factory;
        private readonly IEventAtcPositionService _eventAtcPositionService = eventAtcPositionService;
        async Task IAtcPositionBookingService.AddAtcPositionBooking(AtcPositionBooking booking)
        {
            await _eventAtcPositionService.LoadBookings(booking.EventAtcPosition);
            if (booking.EventAtcPosition.Bookings is not null)
            {
                if (!booking.HasOverlap())
                {
                    using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
                    booking.EventAtcPosition.Bookings.Add(booking);
                    dbContext.Entry(booking).State = EntityState.Added;
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("Bookings are overlapping.");
                }
            }
        }
        async Task<List<AtcPositionBooking>> IAtcPositionBookingService.GetAtcPositionBookings(EventAtcPosition eventAtcPosition)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.BookedAtcPositions.Where(b => b.EventAtcPositionId == eventAtcPosition.Id).AsNoTracking().ToListAsync();
        }
        async Task IAtcPositionBookingService.RemoveAtcPositionBooking(AtcPositionBooking booking)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            if (dbContext.Entry(booking).State == EntityState.Added)
            {
                dbContext.Entry(booking).State = EntityState.Detached;
            }
            else
            {
                dbContext.Entry(booking).State = EntityState.Deleted;
            }
            await dbContext.SaveChangesAsync();
        }
        async Task IAtcPositionBookingService.UpdateAtcPositionBooking(AtcPositionBooking booking)
        {
            using BookingDbContext dbContext = await _factory.CreateDbContextAsync();
            if (dbContext.Entry(booking).State != EntityState.Added)
            {
                dbContext.Entry(booking).State = EntityState.Modified;
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
