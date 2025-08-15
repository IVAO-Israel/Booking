using System.ComponentModel.DataAnnotations;
using Booking.Extensions.Validation;

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
        [DateTimeBefore(nameof(EndTime), ErrorMessage = "Begin time must be before end time.")]
        [Required]
        public DateTime BeginTime { get; set; }
        [DateTimeAfter(nameof(BeginTime), ErrorMessage = "End time must be after begin time.")]
        [Required]
        public DateTime EndTime { get; set; }
        public int IVAOUserId { get; set; }
        /// <summary>
        /// Check if there are overlaps in bookings before saving it.
        /// </summary>
        /// <returns>Bool if there are overlaps in bookings.</returns>
        /// <exception cref="ArgumentNullException">EventAtcPosition.Bookings is null.</exception>
        public bool HasOverlap()
        {
            if (EventAtcPosition.Bookings is null)
            {
                throw new ArgumentNullException(nameof(EventAtcPosition.Bookings));
            }
            return EventAtcPosition.Bookings.Where(b => BeginTime < b.EndTime && b.BeginTime < EndTime).Any();
        }
    }
}
