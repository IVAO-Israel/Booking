using System.ComponentModel.DataAnnotations;

namespace Booking.Data
{
    public class Administrator
    {
        [Key]
        public Guid Id { get; set; }
        public int IVAOUserId { get; set; }
        /// <summary>
        /// If it's null, it can edit any division.
        /// </summary>
        public string? DivisionId { get; set; }
    }
}
