using System.ComponentModel.DataAnnotations;

namespace Booking.Data
{
    public class Administrator
    {
        [Key]
        public Guid Id { get; set; }
        public int IVAOUserId { get; set; }
    }
}
