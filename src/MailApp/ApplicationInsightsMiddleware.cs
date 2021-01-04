using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace MailApp
{
    public class ApplicationInsightsMiddleware
    {
        private readonly RequestDelegate _next;

        public ApplicationInsightsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestTelemetry = context.Features.Get<RequestTelemetry>();
            foreach (var (key, value) in context.Request.Headers)
            {
                requestTelemetry.Properties[$"Request header - {key}"] = value;
            }

            await _next(context);

            foreach (var (key, value) in context.Response.Headers)
            {
                requestTelemetry.Properties[$"Response header - {key}"] = value;
            }
        }
    }
}