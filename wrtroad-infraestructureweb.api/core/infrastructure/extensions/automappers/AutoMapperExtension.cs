using System.Reflection;
using wrtroad_infraestructureweb.api.core.infrastructure.extensions.automappers;
using wrtroad_infraestructureweb.api.modules.auth.application.mappings;

namespace wrtroad_infraestructureweb.api.core.infrastructure.extensions.automappers
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AuthProfile)));

            return services;
        }
    }
}
