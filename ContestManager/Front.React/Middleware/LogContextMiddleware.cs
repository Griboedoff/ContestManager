using System.Threading.Tasks;
using Core.Users.Sessions;
using Microsoft.AspNetCore.Http;
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

        public async Task InvokeAsync(HttpContext context, ILogger<LogContextMiddleware> logger,
            IUserCookieManager cookieManager)
        {
            var requestId = context.TraceIdentifier;
            var sessionId = await GetSessionId(cookieManager, context);

            using (logger.BeginScope($"[{requestId}] [{sessionId}]"))
            {
                logger.LogInformation($"Start controller call {context.Request.Path}");
                await next(context);
                logger.LogInformation($"Finish controller call {context.Request.Path}. Response status code {context.Response.StatusCode}");
            }
        }

        private static async Task<string> GetSessionId(IUserCookieManager cookieManager, HttpContext context)
        {
            var (status, sid) = await cookieManager.GetSessionIdSafe(context.Request);

            return status == ValidateUserSessionStatus.Ok && sid.HasValue
                ? $"{sid.Value:D}".Substring(0, 8)
                : "No session";
        }
    }
}
