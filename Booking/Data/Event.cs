using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Booking.Data
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime BeginTime { get; set; }
        public required DateTime EndTime { get; set; }
        public bool IsVisible { get; set; }
    }
}
