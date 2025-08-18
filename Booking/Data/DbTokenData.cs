using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Booking.Ivao.DTO;

namespace Booking.Data
{
    public class DbTokenData
    {
        [Key]
        public string UserId { get; set; } = default!;
        public string JsonData { get; set; } = default!;
        public TokenData? TokenData => JsonSerializer.Deserialize<TokenData>(JsonData);
    }
}
