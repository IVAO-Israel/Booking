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
        public required Guid AtcPositionId { get; set; }
        public required AtcPosition AtcPosition { get; set; }
        public required DateTime BegingTime { get; set; }
        public required DateTime EndTime { get; set; }
    }
}
