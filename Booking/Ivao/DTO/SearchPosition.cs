using System.Text.Json.Serialization;

namespace Booking.Ivao.DTO
{
    public class SearchPositionResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("airportId")]
        public string? AirportId { get; set; }
        [JsonPropertyName("centerId")]
        public string? CenterId { get; set; }
        /// <summary>
        /// Radio callsign
        /// </summary>
        [JsonPropertyName("atcCallsign")]
        public string AtcCallsign { get; set; } = default!;
        [JsonPropertyName("military")]
        public bool Military { get; set; }
        /// <summary>
        /// CTR, APP and others
        /// </summary>
        [JsonPropertyName("position")]
        public string Position { get; set; } = default!;
        [JsonPropertyName("middleIdentifier")]
        public string? MiddleIdentifier { get; set; }
        [JsonPropertyName("frequency")]
        public double? Frequency { get; set; }
        /// <summary>
        /// Full IVAO position
        /// </summary>
        [JsonPropertyName("composePosition")]
        public string ComposePosition { get; set; } = default!;
    }
}
