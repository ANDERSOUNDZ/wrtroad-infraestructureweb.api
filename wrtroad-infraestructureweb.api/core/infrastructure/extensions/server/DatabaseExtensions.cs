using Microsoft.EntityFrameworkCore;
using wrtroad_infraestructureweb.api.core.infrastructure.data.context;

namespace wrtroad_infraestructureweb.api.core.infrastructure.extensions.server
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WrtRoadDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(90),
                            errorNumbersToAdd: null);
                    });
            });
            return services;
        }
    }
}
