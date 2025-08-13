using System.ComponentModel.DataAnnotations;

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
        [Required]
        public DateTime BeginTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public bool IsVisible { get; set; }
        public string? Url { get; set; }
        public ICollection<EventAtcPosition>? AvailableAtcPositions { get; set; }
    }
}
