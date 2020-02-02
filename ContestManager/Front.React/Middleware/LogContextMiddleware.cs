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
            var userId = await GetUserId(cookieManager, context);

            using (logger.BeginScope($"[{requestId}] [{userId}]"))
            {
                await next(context);
            }
        }

        private static async Task<string> GetUserId(IUserCookieManager cookieManager, HttpContext context)
        {
            var (status, userId) = await cookieManager.GetUserIdSafe(context.Request);

            return status == ValidateUserSessionStatus.Ok && userId.HasValue
                ? $"{userId.Value:D}".Substring(0, 8)
                : "Unknown";
        }
    }
}