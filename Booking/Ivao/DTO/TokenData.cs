namespace Booking.Ivao.DTO
{
    public class TokenData
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
#pragma warning disable CS8618
#pragma warning disable IDE1006
    public class ReceivedToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }
#pragma warning restore CS8618
#pragma warning restore IDE1006
}
