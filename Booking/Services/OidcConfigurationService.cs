using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
namespace Booking.Services
{
    public class OidcConfigurationService
    {
        private readonly IConfigurationManager<OpenIdConnectConfiguration> _configurationManager;

        public OidcConfigurationService(IConfiguration config)
        {
            var authority = "https://api.ivao.aero";
            _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{authority}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever()
            );
        }
        public async Task<string> GetUserInfoEndpointAsync()
        {
            var oidcConfig = await _configurationManager.GetConfigurationAsync(CancellationToken.None);
            return oidcConfig.UserInfoEndpoint;
        }
    }
}
