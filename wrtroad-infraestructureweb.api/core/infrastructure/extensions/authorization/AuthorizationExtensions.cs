using Microsoft.AspNetCore.Authorization;
using wrtroad_infraestructureweb.api.modules.auth.application.helpers.authorization;
using wrtroad_infraestructureweb.api.modules.user.application.helpers.authorization;

namespace wrtroad_infraestructureweb.api.core.infrastructure.extensions.authorization
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Restricted", policy =>
                policy.Requirements.Add(new RolesRequirement("super-admin")));

                options.AddPolicy("OpenWall", policy =>
                policy.Requirements.Add(new RolesRequirement("client-writter")));

                options.AddPolicy("DynamicWall", policy =>
                policy.Requirements.Add(new RolesRequirement("super-admin", "client-writter")));

                options.AddPolicy("OwnResource", policy =>
                policy.Requirements.Add(new OwnResourceRequirement()));

            });
            return services;
        }
    }
}
