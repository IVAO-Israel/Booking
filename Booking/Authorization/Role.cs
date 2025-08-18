using System.Security.Claims;
using Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Booking.Authorization
{
    public class RoleRequirement(string role) : IAuthorizationRequirement
    {
        public string Role { get; } = role;
    }
    public class RoleAuthorizationHandler(IAdministratorService administratorService) : AuthorizationHandler<RoleRequirement>
    {
        private readonly IAdministratorService _administratorService = administratorService;

        private static readonly Dictionary<string, string[]> RoleHierarchy = new()
        {
            { "ADMIN",   new[] { "ADMIN", "DIR", "EVENT", "ATC", "FLIGHT" } },
            { "DIR",    new[] { "DIR", "EVENT", "ATC", "FLIGHT" } },
            { "EVENT",  new[] { "EVENT", "ATC", "FLIGHT" } },
            { "ATC",    new[] { "ATC" } },
            { "FLIGHT", new[] { "FLIGHT" } }
        };
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleRequirement requirement)
        {
            var vid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (vid is not null && int.TryParse(vid, out var userId))
            {
                var admin = await _administratorService.GetAdministrator(userId);

                if (admin?.Roles is not null)
                {
                    if (RoleHierarchy.TryGetValue(requirement.Role, out var allowedRoles))
                    {
                        if (admin.Roles.Any(r => allowedRoles.Contains(r.Role)))
                        {
                            context.Succeed(requirement);
                        }
                    }
                }
            }
        }
    }
}