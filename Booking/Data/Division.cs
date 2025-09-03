using System.ComponentModel.DataAnnotations;

namespace Booking.Data
{
    public class Division
    {
        [Key]
        public string DivisionId { get; set; } = default!;
    }
}
