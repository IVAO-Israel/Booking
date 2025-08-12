using System.Security.Claims;
using Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Booking.Authorization
{
    public class AdministratorRequirement : IAuthorizationRequirement
    {
    }
    public class AdministratorAuthorizationHandler(IAdministratorService administratorService) : AuthorizationHandler<AdministratorRequirement>
    {
        private readonly IAdministratorService _administratorService = administratorService;

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdministratorRequirement requirement)
        {
            var vid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // IVAO user ID from OIDC claim

            if (vid != null && int.TryParse(vid, out int userId))
            {
                var admin = await _administratorService.GetAdministrator(userId);

                if (admin is not null)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
