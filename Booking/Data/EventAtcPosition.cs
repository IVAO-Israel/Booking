using System.ComponentModel.DataAnnotations;
using Booking.Extensions.Validation;

namespace Booking.Data
{
    /// <summary>
    /// ATC position available for booking during given event and time.
    /// </summary>
    public class EventAtcPosition
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public required Event Event { get; set; }
        public Guid AtcPositionId { get; set; }
        public AtcPosition AtcPosition { get; set; } = default!;
        [DateTimeBefore(nameof(EndTime), ErrorMessage = "Begin time must be before end time.")]
        [Required]
        public required DateTime BeginTime { get; set; }
        [DateTimeAfter(nameof(BeginTime), ErrorMessage = "End time must be after begin time.")]
        [Required]
        public required DateTime EndTime { get; set; }
        public int RequiredRating { get; set; }
        public string DivisionId { get; set; } = default!;
        public ICollection<AtcPositionBooking>? Bookings { get; set; }
        /// <summary>
        /// Check if position can be booked based on user ATC rating.
        /// </summary>
        /// <param name="AtcRating">User ATC rating.</param>
        /// <returns>Bool if position can be booked by given user.</returns>
        public bool CanBeBookedByUser(int AtcRating)
        {
            return AtcRating <= RequiredRating;
        }
        /// <summary>
        /// Check if there are overlaps in bookings before saving it.
        /// </summary>
        /// <returns>Bool if there are overlaps in bookings.</returns>
        /// <exception cref="ArgumentNullException">Event.AvailableAtcPositions is null.</exception>
        public bool HasOverlap()
        {
            if (Event.AvailableAtcPositions is null)
            {
                throw new ArgumentNullException(nameof(Event.AvailableAtcPositions));
            }
            return Event.AvailableAtcPositions.Where(p => p.AtcPositionId == AtcPositionId)
                                       .Where(p => BeginTime < p.EndTime && p.BeginTime < EndTime)
                                       .Where(p => p.Id != Id).Any();
        }
    }
}
