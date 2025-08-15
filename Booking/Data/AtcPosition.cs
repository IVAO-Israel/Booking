using System.ComponentModel.DataAnnotations;

namespace Booking.Data
{
    /// <summary>
    /// ATC position that can be used in the system.
    /// </summary>
    public class AtcPosition
    {
        [Key]
        public Guid Id { get; set; }
        public required string IVAOCallsign { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public override string ToString()
        {
            return $"{Name} ({IVAOCallsign})";
        }
    }
}
