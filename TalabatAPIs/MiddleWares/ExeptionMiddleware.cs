using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using TalabatAPIs.Errors;
namespace TalabatAPIs.MiddleWares
{
    //By Convension
    public class ExeptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExeptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExeptionMiddleware(RequestDelegate next , ILogger<ExeptionMiddleware> logger , IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Take an Action With The Request
                await _next.Invoke(httpContext); // Go To the Next Middleware

                // Take an Action with The Response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message); // Development Env 
                //log Exception in (database | Files) // Production Env
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment() ?
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace) 
                    :
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response , options);
               await httpContext.Response.WriteAsync(json); 

                
            }
            
        }
    }
}
