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
            //Overlap only if any other bookings ends before this starts or other starts before this ends
            //If one starts at the same time as other ends, it's not overlap
            return EventAtcPosition.Bookings.Where(b => b.EndTime < BeginTime || b.BeginTime < EndTime).Any();
        }
    }
}
