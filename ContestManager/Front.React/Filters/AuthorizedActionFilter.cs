using System.Linq;
using System.Threading.Tasks;
using Core.Enums.DataBaseEnums;
using Core.Users.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Front.React.Filters
{
    public class AuthorizedActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<AuthorizedActionFilter> logger;
        private readonly IUserCookieManager cookieManager;
        private readonly UserRole[] roles;

        public AuthorizedActionFilter(
            ILogger<AuthorizedActionFilter> logger,
            IUserCookieManager cookieManager,
            UserRole[] roles)
        {
            this.logger = logger;
            this.cookieManager = cookieManager;
            this.roles = roles;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var (status, user) = await cookieManager.GetUserSafe(context.HttpContext.Request);
            if (user == null)
            {
                logger.LogWarning($"Неавторизованный вызов {context.HttpContext.Request.Path} {status:G}");
                cookieManager.Clear(context.HttpContext.Response);
                context.Result = new UnauthorizedResult();
                return;
            }

            if (roles.Any() && !roles.Contains(user.Role))
            {
                logger.LogWarning($"Недостаточно прав для {context.HttpContext.Request.Path} {user.Id}");
                context.Result = new ObjectResult("Неверная роль")
                {
                    StatusCode = 401,
                };
            }
            else
            {
                context.ActionArguments["user"] = user;

                await next();
            }
        }
    }
}
