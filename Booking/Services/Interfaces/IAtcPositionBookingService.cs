using Booking.Data;

namespace Booking.Services.Interfaces
{
    public interface IAtcPositionBookingService
    {
        public Task AddAtcPositionBooking(AtcPositionBooking booking);
        public Task<List<AtcPositionBooking>> GetAtcPositionBookings(EventAtcPosition eventAtcPosition);
        public Task UpdateAtcPositionBooking(AtcPositionBooking booking);
        public Task RemoveAtcPositionBooking(AtcPositionBooking booking);
    }
}
