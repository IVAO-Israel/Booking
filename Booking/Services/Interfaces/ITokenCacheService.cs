using System.Collections.Concurrent;

namespace Booking.Services.Interfaces
{
    public interface ITokenCacheService
    {
        Task<TokenData?> GetTokensAsync(string userId);
        Task StoreTokensAsync(string userId, TokenData tokenData);
        Task RemoveTokensAsync(string userId);
    }
}
