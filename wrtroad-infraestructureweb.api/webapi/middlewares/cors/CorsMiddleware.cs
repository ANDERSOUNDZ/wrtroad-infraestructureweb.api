using System.Net;

namespace wrtroad_infraestructureweb.api.webapi.middlewares.cors
{
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;


        public CorsMiddleware(RequestDelegate next, IConfiguration configuration, IWebHostEnvironment env)
        {
            _next = next;
            _configuration = configuration;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var allowedOrigins = _configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
                if (_env.IsDevelopment())
                {
                    var localOrigins = new[] { "https://localhost:7035", "http://localhost:3000" };
                    allowedOrigins = allowedOrigins?.Concat(localOrigins).ToArray();
                }
                if (allowedOrigins != null && allowedOrigins.Contains(context.Request.Headers["Origin"].ToString()))
                {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Content-Type");
                    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    if (context.Request.Method == "OPTIONS")
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsync("Origin not allowed.");
                    return;
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CORS Middleware Error: {ex}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "An error occurred while processing the request." });
                return;
            }
        }
    }
}
