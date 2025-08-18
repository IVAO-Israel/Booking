using System.ComponentModel.DataAnnotations;

namespace Booking.Data
{
    public class AdministratorRole
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AdministratorId { get; set; }
        public Administrator Administrator { get; set; } = default!;
        /// <summary>
        /// DIR, EVENT, ATC, FLIGHT
        /// </summary>
        public string Role { get; set; } = default!;
    }
}
