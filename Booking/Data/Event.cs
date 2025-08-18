using System.ComponentModel.DataAnnotations;
using Booking.Extensions.Validation;

namespace Booking.Data
{
    /// <summary>
    /// Event for which booking is needed.
    /// </summary>
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = default!;
        [DateTimeBefore(nameof(EndTime), ErrorMessage = "Begin time must be before end time.")]
        [Required]
        public DateTime BeginTime { get; set; }
        [DateTimeAfter(nameof(BeginTime), ErrorMessage = "End time must be after begin time.")]
        [Required]
        public DateTime EndTime { get; set; }
        public bool IsVisible { get; set; }
        public string? Url { get; set; }
        public string DivisionId { get; set; } = default!;
        public ICollection<EventAtcPosition>? AvailableAtcPositions { get; set; }
    }
}
