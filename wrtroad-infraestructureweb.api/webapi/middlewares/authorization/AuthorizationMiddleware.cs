using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace wrtroad_infraestructureweb.api.webapi.middlewares.authorization
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var endpoint = context.GetEndpoint();
                var allowAnonymous = endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null;

                if (allowAnonymous)
                {
                    await _next(context);
                    return;
                }

                if (!context.User.Identity?.IsAuthenticated ?? true)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { error = "Authentication failed. Please log in." }); // Use JSON response
                    return;
                }

                await _next(context);

                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden; // Explicitly set 403
                    await context.Response.WriteAsJsonAsync(new { error = "You do not have the required permissions to access this resource." }); // JSON response
                }
            }
            catch (Exception ex)
            {
                // VERY IMPORTANT: Log the full exception details on the server.
                Console.Error.WriteLine($"Authorization Middleware Error: {ex}"); // Or use your logging framework (Serilog, NLog, etc.)

                // Send a generic error message to the client.  NEVER expose raw exception details.
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500 Internal Server Error
                await context.Response.WriteAsJsonAsync(new { error = "An error occurred while processing the request." });
                return;
            }
        }
    }
}
