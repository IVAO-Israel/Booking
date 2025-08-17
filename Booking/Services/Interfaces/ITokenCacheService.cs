using System.Collections.Concurrent;
using Booking.Ivao.Services;
using Booking.Ivao.DTO;

namespace Booking.Services.Interfaces
{
    public interface ITokenCacheService
    {
        Task<TokenData?> GetTokensAsync(string userId);
        Task StoreTokensAsync(string userId, TokenData tokenData);
        Task RemoveTokensAsync(string userId);
    }
}
