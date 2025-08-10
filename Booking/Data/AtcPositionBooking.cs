using System.ComponentModel.DataAnnotations;

namespace Booking.Data
{
    /// <summary>
    /// Representing ATC position booked by given user during event.
    /// </summary>
    public class AtcPositionBooking
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EventAtcPositionId { get; set; }
        public EventAtcPosition EventAtcPosition { get; set; } = default!;
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int IVAOUserId { get; set; } //TODO: choose int or something else, 6 digit should fit, but not sure if any is 0xxxxx
    }
}
