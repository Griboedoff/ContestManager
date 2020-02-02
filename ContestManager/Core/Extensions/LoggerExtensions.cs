using System;
using Core.Users.Sessions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core.Extensions
{
    public static class LoggerExtensions
    {
        public static IDisposable AddHttpContext(this ILogger logger, HttpContext httpContext)
        {
            var requestId = httpContext.TraceIdentifier;
            var userId = GetUserId(httpContext.Request);

            return logger.BeginScope($"[{requestId}] [{userId}]");
        }

        private static string GetUserId(HttpRequest request)
            => UserCookieManager.TryGetUser(request, out var user)
                ? $"{user.Id:D}".Substring(0, 8)
                : "Unknown";
    }
}