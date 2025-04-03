using wrtroad_infraestructureweb.api.webapi.middlewares.authorization;
using wrtroad_infraestructureweb.api.webapi.middlewares.cors;
using wrtroad_infraestructureweb.api.webapi.middlewares.exeptions;

namespace wrtroad_infraestructureweb.api.webapi.middlewares
{
    public static class BaseMiddleware
    {
        public static IApplicationBuilder CustomMiddlewares(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionMiddleware>();
            builder.UseMiddleware<CorsMiddleware>();
            builder.UseMiddleware<AuthorizationMiddleware>();
            return builder;
        }
    }
}
