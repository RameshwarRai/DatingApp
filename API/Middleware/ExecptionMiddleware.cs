using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using API.Errors;
using System.Text.Json;

namespace API.Middleware
{
    public class ExecptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExecptionMiddleware> _logger;

        private IHostEnvironment _env;
        public ExecptionMiddleware(RequestDelegate next,
                                   ILogger<ExecptionMiddleware> logger,
                                   IHostEnvironment env)
        {
            _env=env;
            _logger=logger;
            _next=next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _env.IsDevelopment()
                ? new ApiExecption(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiExecption(context.Response.StatusCode, "Internal Server Error");
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json=JsonSerializer.Serialize(response,options);

                await context.Response.WriteAsync(json);
            }

            
        }

      
    }
}