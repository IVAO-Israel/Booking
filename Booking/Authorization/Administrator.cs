using System.Security.Claims;
using Booking.Data;
using Microsoft.AspNetCore.Authorization;

namespace Booking.Authorization
{
    public class AdministratorRequirement : IAuthorizationRequirement
    {
    }
    public class AdministratorAuthorizationHandler(BookingDbContext dbContext) : AuthorizationHandler<AdministratorRequirement>
    {
        private readonly BookingDbContext _dbContext = dbContext;

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdministratorRequirement requirement)
        {
            var vid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // IVAO user ID from OIDC claim

            if (vid != null && int.TryParse(vid, out int userId))
            {
                var isAdmin = _dbContext.Administrators.Any(a => a.IVAOUserId == userId);

                if (isAdmin)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
