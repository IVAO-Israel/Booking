using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Booking.Services
{
    public class IvaoUserService(IAuthorizationService authorizationService, AuthenticationStateProvider authenticationStateProvider, OidcConfigurationService oidcConfigurationService, HttpClient httpClient)
    {
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
        private readonly OidcConfigurationService _oidcConfigurationService = oidcConfigurationService;
        private readonly HttpClient _httpClient = httpClient;
        public async Task<int> GetUserIvaoId()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var vid = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (vid is not null && int.TryParse(vid, out int ivaoid))
            {
                return ivaoid;
            }
            return 0;
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
            string endpoint = await _oidcConfigurationService.GetUserInfoEndpointAsync();
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var accessToken = user.FindFirst("access_token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            dynamic? obj = JsonConvert.DeserializeObject(json!);
            return obj?.divisionId!;
        }
        public async Task<int> GetUserAtcRating()
        {
            string endpoint = await _oidcConfigurationService.GetUserInfoEndpointAsync();
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var accessToken = user.FindFirst("access_token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            dynamic? obj = JsonConvert.DeserializeObject(json!);
            return obj?.rating.atcRating.id;
        }
    }
}
