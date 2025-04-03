using Microsoft.AspNetCore.Authorization;
using wrtroad_infraestructureweb.api.modules.auth.application.helpers.authorization;
using wrtroad_infraestructureweb.api.modules.auth.domain.Irepositories;
using wrtroad_infraestructureweb.api.modules.auth.infrastructure.repositories;
using wrtroad_infraestructureweb.api.modules.user.application.helpers.authorization;

namespace wrtroad_infraestructureweb.api.core.infrastructure.extensions.injections
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IEmailSenderRepository, EmailSenderRepository>();
            services.AddSingleton<IAuthorizationHandler, RolesRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, OwnResourceAuthorizationHandler>();
            return services;
        }
    }
}
