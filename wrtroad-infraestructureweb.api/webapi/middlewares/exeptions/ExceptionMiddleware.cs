using wrtroad_infraestructureweb.api.webapi.DTOs;

namespace wrtroad_infraestructureweb.api.webapi.middlewares.exeptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new ResponseBase
                {
                    Message = "An error occurred while processing your request.",
                    Code = ResponseCode.INTERNAL_SERVER_ERROR,
                    Data = null,
                    CodeText = ResponseCodeText.INTERNAL_SERVER_ERROR.ToString()
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
