using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace wrtroad_infraestructureweb.api.modules.user.application.helpers.authorization
{
    public class OwnResourceAuthorizationHandler : AuthorizationHandler<OwnResourceRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OwnResourceAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnResourceRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var routeData = httpContext.GetRouteData();
            var resourceId = routeData.Values["userId"]?.ToString();
            var currentUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(currentUserId) && currentUserId == resourceId)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
