using System.Threading.Tasks;
using Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Front.React.Middleware
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate next;

        public LogContextMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<LogContextMiddleware>>();

            using (logger.AddHttpContext(context))
            {
                await next(context);
            }
        }
    }
}