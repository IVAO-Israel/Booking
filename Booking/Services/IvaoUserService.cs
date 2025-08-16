using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Booking.Services
{
    public class IvaoUserService(IAuthorizationService authorizationService,
        AuthenticationStateProvider authenticationStateProvider,
        OidcConfigurationService oidcConfigurationService,
        HttpClient httpClient,
        IConfiguration configuration,
        NavigationManager navigationManager)
    {
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
        private readonly OidcConfigurationService _oidcConfigurationService = oidcConfigurationService;
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _configuration = configuration;
        private readonly NavigationManager _navigationManager = navigationManager;
        private async Task<string> GetUserData()
        {
            try
            {
                string endpoint = await _oidcConfigurationService.GetUserInfoEndpointAsync();
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                var accessToken = user.FindFirst("access_token")?.Value;
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                //Not authorized, problem with token
                var relativeUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
                var returnUrl = Uri.EscapeDataString("/" + relativeUrl);
                _navigationManager.NavigateTo($"/login?returnUrl={returnUrl}", forceLoad: true);
                return "";
            }
        }
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
                {
                    return true;
                }
            }
            return false;
        }
    }
}
