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
        public string IVAOCallsign { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int? IVAOPositionId { get; set; }
        public string DivisionId { get; set; } = default!;
        public override string ToString()
        {
            return $"{Name} ({IVAOCallsign})";
        }
    }
}
