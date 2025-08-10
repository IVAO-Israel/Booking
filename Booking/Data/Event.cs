using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Booking.Data
{
    /// <summary>
    /// Event for which booking is needed.
    /// </summary>
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime BeginTime { get; set; }
        public required DateTime EndTime { get; set; }
        public bool IsVisible { get; set; }
        public ICollection<EventAtcPosition>? AvailableAtcPositions { get; set; }
    }
}
