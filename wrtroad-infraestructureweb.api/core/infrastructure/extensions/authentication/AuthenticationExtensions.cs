using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace wrtroad_infraestructureweb.api.core.infrastructure.extensions.authentication
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("JwtConfig");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["Issuer"],
                ValidAudience = jwtConfig["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtConfig["SecretKey"])
                )
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var user = context.Principal;

                    if (user != null)
                    {
                        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (!string.IsNullOrEmpty(userIdClaim))
                        {
                            context.HttpContext.Items["UserId"] = userIdClaim;
                        }
                        var roles = user.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value)
                            .ToList();
                        context.HttpContext.Items["UserRoles"] = roles;
                    }
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    string errorMessage = "Authentication failed. Please log in.";
                    int statusCode = StatusCodes.Status401Unauthorized;
                    switch (context.Exception)
                    {
                        case SecurityTokenExpiredException:
                            errorMessage = "Your session has expired. Please log in again.";
                            break;

                        case SecurityTokenInvalidSignatureException or SecurityTokenSignatureKeyNotFoundException:
                            errorMessage = "Invalid token signature. Please contact support.";
                            break;

                        case SecurityTokenInvalidAudienceException:
                            errorMessage = "Invalid token audience. Please contact support.";
                            break;

                        case SecurityTokenInvalidIssuerException:
                            errorMessage = "Invalid token issuer. Please contact support.";
                            break;
                        case Exception ex when ex.GetType().FullName == "Microsoft.AspNetCore.Authentication.JwtBearer.JwtAuthenticationException":
                            errorMessage = $"JWT Authentication failed: {ex.InnerException?.Message ?? ex.Message}";
                            break;

                        case Exception ex:
                            errorMessage = "An unexpected error occurred during authentication. Please try again later.";
                            statusCode = StatusCodes.Status500InternalServerError;
                            Console.Error.WriteLine($"Authentication Error: {ex}");
                            break;
                    }
                    context.Response.StatusCode = statusCode;
                    context.Response.ContentType = "application/json";
                    var errorResponse = new { error = errorMessage };
                    return context.Response.WriteAsJsonAsync(errorResponse);
                }
                /*
                OnTokenValidated = context =>
                {
                    var userIdClaim = context.Principal?.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                        context.HttpContext.Items["UserId"] = userIdClaim.Value;
                    }

                    context.HttpContext.Items["IsAdmin"] = context.Principal?.IsInRole("super-admin");
                    context.HttpContext.Items["IsClientWriter"] = context.Principal?.IsInRole("client-writter");

                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    // Personalizar el mensaje de error según el tipo de excepción
    string errorMessage = "Authentication failed. Please log in.";

    if (context.Exception is SecurityTokenExpiredException)
    {
        errorMessage = "Your session has expired. Please log in again.";
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }
    else if (context.Exception is SecurityTokenInvalidSignatureException ||
             context.Exception is SecurityTokenSignatureKeyNotFoundException)
    {
        errorMessage = "Invalid token signature. Please contact support.";
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }
    else if (context.Exception is SecurityTokenInvalidAudienceException)
    {
        errorMessage = "Invalid token audience. Please contact support.";
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }
    else if (context.Exception is SecurityTokenInvalidIssuerException)
    {
        errorMessage = "Invalid token issuer. Please contact support.";
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }
    else
    {
        // Otros errores no manejados específicamente
        errorMessage = "An error occurred during authentication. Please try again later.";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }

    // Registrar el error (opcional)
    Console.WriteLine($"Authentication failed: {context.Exception}");

    // Devolver el mensaje de error como JSON
    context.Response.ContentType = "application/json";
    return context.Response.WriteAsync(JsonConvert.SerializeObject(new { error = errorMessage }));
                }
                */
            };
        }
        );
            return services;
        }
    }
}
