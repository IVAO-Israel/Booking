using System.ComponentModel.DataAnnotations;

namespace Booking.Data
{
    /// <summary>
    /// ATC position available for booking during given event and time.
    /// </summary>
    public class EventAtcPosition
    {
        [Key]
        public Guid Id { get; set; }
        public required int EventId { get; set; }
        public required Event Event { get; set; }
        public Guid AtcPositionId { get; set; }
        public AtcPosition AtcPosition { get; set; } = default!;
        public required DateTime BegingTime { get; set; }
        public required DateTime EndTime { get; set; }
        public int RequiredRating { get; set; }
        public ICollection<AtcPositionBooking>? Bookings { get; set; }
    }
}
