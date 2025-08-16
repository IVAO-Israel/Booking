using System.Collections.Concurrent;
using Booking.Services.Interfaces;

namespace Booking.Services
{
    public class InMemoryTokenCacheService : ITokenCacheService
    {
        private readonly ConcurrentDictionary<string, TokenData> _cache = new();
        public Task<TokenData?> GetTokensAsync(string userId)
        {
            _cache.TryGetValue(userId, out var tokenData);
            return Task.FromResult(tokenData);
        }
        public Task StoreTokensAsync(string userId, TokenData tokenData)
        {
            _cache[userId] = tokenData;
            return Task.CompletedTask;
        }
        public Task RemoveTokensAsync(string userId)
        {
            _cache.TryRemove(userId, out _);
            return Task.CompletedTask;
        }
    }
}
