using System.Collections.Concurrent;
using Booking.Ivao.Services;
using Booking.Services.Interfaces;
using Booking.Ivao.DTO;
using Booking.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Booking.Services
{
    public class DbTokenCacheService (IDbContextFactory<BookingDbContext> factory) : ITokenCacheService
    {
        private readonly IDbContextFactory<BookingDbContext> _factory = factory;
        public async Task<TokenData?> GetTokensAsync(string userId)
        {
            using var dbContext = await _factory.CreateDbContextAsync();
            return (await dbContext.IvaoTokenData.FindAsync(userId))?.TokenData;
        }
        public async Task StoreTokensAsync(string userId, TokenData tokenData)
        {
            using var dbContext = await _factory.CreateDbContextAsync();
            DbTokenData? token = await dbContext.IvaoTokenData.FindAsync(userId);
            if(token is null) {
                token = new()
                {
                    UserId = userId,
                    JsonData = JsonSerializer.Serialize(tokenData)
                };
                dbContext.Entry(token).State = EntityState.Added;
            } else
            {
                token.JsonData = JsonSerializer.Serialize(tokenData);
                dbContext.Entry(token).State = EntityState.Modified;
            }
            await dbContext.SaveChangesAsync();
        }
        public async Task RemoveTokensAsync(string userId)
        {
            using var dbContext = await _factory.CreateDbContextAsync();
            var token = await dbContext.IvaoTokenData.FindAsync(userId);
            if (token is not null) {
                dbContext.Entry(token).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
