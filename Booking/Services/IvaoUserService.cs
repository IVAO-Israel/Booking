using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Booking.Services.Interfaces;

namespace Booking.Services
{
    public class IvaoUserService(
        IAuthorizationService authorizationService,
        AuthenticationStateProvider authenticationStateProvider,
        OidcConfigurationService oidcConfigurationService,
        HttpClient httpClient,
        IConfiguration configuration,
        NavigationManager navigationManager,
        ITokenCacheService tokenCacheService)
    {
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
        private readonly OidcConfigurationService _oidcConfigurationService = oidcConfigurationService;
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _configuration = configuration;
        private readonly NavigationManager _navigationManager = navigationManager;
        private readonly ITokenCacheService _tokenCacheService = tokenCacheService;
        private async Task<string> GetUserData()
        {
            try
            {
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId is null)
                    throw new UnauthorizedAccessException();

                // Fetch tokens from server cache
                var tokenData = await _tokenCacheService.GetTokensAsync(userId);
                if (tokenData == null)
                    return RedirectToLogin();

                // Check if token expired and refresh if needed
                if (tokenData.ExpiresAt <= DateTime.UtcNow)
                {
                    tokenData = await RefreshToken(tokenData.RefreshToken, userId);
                    if (tokenData == null)
                        return RedirectToLogin();
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", tokenData.AccessToken);

                string endpoint = await _oidcConfigurationService.GetUserInfoEndpointAsync();
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                return RedirectToLogin();
            }
        }
        private string RedirectToLogin()
        {
            var relativeUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
            var returnUrl = Uri.EscapeDataString("/" + relativeUrl);
            _navigationManager.NavigateTo($"/login?returnUrl={returnUrl}", forceLoad: true);
            return "";
        }
        private async Task<TokenData?> RefreshToken(string refreshToken, string userId)
        {
            var tokenEndpoint = await _oidcConfigurationService.GetTokenEndpointAsync();

            var parameters = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id", _configuration["Authentication:OIDC:ClientId"]! },
                { "client_secret", _configuration["Authentication:OIDC:ClientSecret"]! }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
            {
                Content = new FormUrlEncodedContent(parameters)
            };

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json)!;

            var newAccessToken = (string)obj.access_token;
            var newRefreshToken = obj.refresh_token != null ? (string)obj.refresh_token : refreshToken;
            var expiresIn = (int)obj.expires_in;

            var newTokenData = new TokenData
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn - 60) // buffer 1 min
            };

            await _tokenCacheService.StoreTokensAsync(userId, newTokenData);
            return newTokenData;
        }
        public async Task<int> GetUserIvaoId()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var vid = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return vid != null && int.TryParse(vid, out var id) ? id : 0;
        }
        public async Task<bool> GetIsAdmin()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var result = await _authorizationService.AuthorizeAsync(user, "Administrator");
            return result.Succeeded;
        }
        public async Task<string> GetUserDivisionId()
        {
            var json = await GetUserData();
            dynamic? obj = JsonConvert.DeserializeObject(json!);
            return obj?.divisionId!;
        }
        public async Task<int> GetUserAtcRating()
        {
            var json = await GetUserData();
            dynamic? obj = JsonConvert.DeserializeObject(json!);
            return obj?.rating.atcRating.id;
        }
        public async Task<bool> UserHasGca()
        {
            var json = await GetUserData();
            dynamic? obj = JsonConvert.DeserializeObject(json!);
            Newtonsoft.Json.Linq.JArray gcas = obj?.gcas!;
            var divId = _configuration["DivisionId"]?.ToString().ToUpper();
            foreach (var gca in gcas)
            {
                if (gca.ToString().Equals(divId, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
    public class TokenData
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}