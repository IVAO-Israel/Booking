using Booking.Data;
using Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class DbAtcPositionBookingService (BookingDbContext dbContext, IEventAtcPositionService eventAtcPositionService) : IAtcPositionBooking
    {
        private readonly BookingDbContext _dbContext = dbContext;
        private readonly IEventAtcPositionService _eventAtcPositionService = eventAtcPositionService;
        void IAtcPositionBooking.AddAtcPositionBooking(AtcPositionBooking booking)
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
        async Task<List<AtcPositionBooking>> IAtcPositionBooking.GetAtcPositionBookings(EventAtcPosition eventAtcPosition)
        {
            return await _dbContext.BookedAtcPositions.Where(b => b.EventAtcPositionId == eventAtcPosition.Id).AsNoTracking().ToListAsync();
        }
        void IAtcPositionBooking.RemoveAtcPositionBooking(AtcPositionBooking booking)
        {
            if(_dbContext.Entry(booking).State == EntityState.Added)
            {
                _dbContext.Entry(booking).State = EntityState.Detached;
            } else
            {
                _dbContext.Entry(booking).State = EntityState.Deleted;
            }
        }
        void IAtcPositionBooking.UpdateAtcPositionBooking(AtcPositionBooking booking)
        {
            if (_dbContext.Entry(booking).State != EntityState.Added)
            {
                _dbContext.Entry(booking).State = EntityState.Modified;
            }
        }
    }
}
