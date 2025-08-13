using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class DbAtcPositionBookingService (BookingDbContext dbContext, IEventAtcPositionService eventAtcPositionService) : IAtcPositionBookingService
    {
        private readonly BookingDbContext _dbContext = dbContext;
        private readonly IEventAtcPositionService _eventAtcPositionService = eventAtcPositionService;
        void IAtcPositionBookingService.AddAtcPositionBooking(AtcPositionBooking booking)
        {
            _eventAtcPositionService.LoadBookings(booking.EventAtcPosition);
            if(booking.EventAtcPosition.Bookings is not null)
            {
                if (!booking.HasOverlap())
                {
                    booking.EventAtcPosition.Bookings.Add(booking);
                    _dbContext.Entry(booking).State = EntityState.Added;
                } else
                {
                    throw new ArgumentException("Bookings are overlapping.");
                }
            }
        }
        async Task<List<AtcPositionBooking>> IAtcPositionBookingService.GetAtcPositionBookings(EventAtcPosition eventAtcPosition)
        {
            return await _dbContext.BookedAtcPositions.Where(b => b.EventAtcPositionId == eventAtcPosition.Id).AsNoTracking().ToListAsync();
        }
        void IAtcPositionBookingService.RemoveAtcPositionBooking(AtcPositionBooking booking)
        {
            if(_dbContext.Entry(booking).State == EntityState.Added)
            {
                _dbContext.Entry(booking).State = EntityState.Detached;
            } else
            {
                _dbContext.Entry(booking).State = EntityState.Deleted;
            }
        }
        void IAtcPositionBookingService.UpdateAtcPositionBooking(AtcPositionBooking booking)
        {
            if (_dbContext.Entry(booking).State != EntityState.Added)
            {
                _dbContext.Entry(booking).State = EntityState.Modified;
            }
        }
    }
}
