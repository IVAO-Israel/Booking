using System.ComponentModel.DataAnnotations;

namespace Booking.Data
{
    public class AtcPosition
    {
        [Key]
        public Guid Id { get; set; }
        public required string IVAOCallsign { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
