using Booking.Data;

namespace Booking.Services.Interfaces
{
    public interface IAtcPositionBookingService
    {
        public void AddAtcPositionBooking(AtcPositionBooking booking);
        public Task<List<AtcPositionBooking>> GetAtcPositionBookings(EventAtcPosition eventAtcPosition);
        public void UpdateAtcPositionBooking(AtcPositionBooking booking);
        public void RemoveAtcPositionBooking(AtcPositionBooking booking);
    }
}
