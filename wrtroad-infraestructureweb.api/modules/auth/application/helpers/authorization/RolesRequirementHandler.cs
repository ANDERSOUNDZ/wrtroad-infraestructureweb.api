using Microsoft.AspNetCore.Authorization;

namespace wrtroad_infraestructureweb.api.modules.auth.application.helpers.authorization
{
    public class RolesRequirementHandler : AuthorizationHandler<RolesRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesRequirement requirement)
        {
            var hasRequiredRole = requirement.AllowedRoles
                .Any(role => context.User.IsInRole(role));
            if (hasRequiredRole)
            {
                context.Succeed(requirement); // El usuario cumple con los requisitos
            }
            else
            {
                context.Fail(new AuthorizationFailureReason(this, $"You are not authorized to perform this action. Only users with the {string.Join(" o ", requirement.AllowedRoles)} role can access this resource."));
            }
            return Task.CompletedTask;
        }
    }
}
