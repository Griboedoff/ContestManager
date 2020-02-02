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
                await next(context);
            }
        }

        private static async Task<string> GetSessionId(IUserCookieManager cookieManager, HttpContext context)
        {
            var (status, sid) = await cookieManager.GetSessionIdSafe(context.Request);

            return status == ValidateUserSessionStatus.Ok && sid.HasValue
                ? $"{sid.Value:D}".Substring(0, 8)
                : "Unknown";
        }
    }
}