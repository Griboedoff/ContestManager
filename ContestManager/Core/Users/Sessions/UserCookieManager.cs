using System;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Microsoft.AspNetCore.Http;

namespace Core.Users.Sessions
{
    public interface IUserCookieManager
    {
        Task SetLoginCookie(HttpResponse response, User user);
        Task<(ValidateUserSessionStatus status, Guid? userId)> GetUserIdSafe(HttpRequest request);
        Task<(ValidateUserSessionStatus status, User user)> GetUserSafe(HttpRequest request);
        void Clear(HttpResponse response);
    }

    public class UserCookieManager : IUserCookieManager
    {
        private readonly IAsyncRepository<User> userRepo;
        private readonly ISessionManager sessionManager;
        private const string Sid = "sid";
        private const string UserId = "userId";

        public UserCookieManager(IAsyncRepository<User> userRepo, ISessionManager sessionManager)
        {
            this.userRepo = userRepo;
            this.sessionManager = sessionManager;
        }

        public async Task SetLoginCookie(HttpResponse response, User user)
        {
            var sid = await sessionManager.CreateSession(user);

            AddCookie(Sid, sid);
            AddCookie(UserId, user.Id);

            void AddCookie(string name, Guid value)
            {
                response.Cookies.Append(
                    name,
                    value.ToString(),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.Add(TimeSpan.FromDays(1)),
                        HttpOnly = true,
                        Secure = Constants.IsSecureCookie
                    });
            }
        }

        public async Task<(ValidateUserSessionStatus status, Guid? userId)> GetUserIdSafe(HttpRequest request)
        {
            if (!TryGetCookie(request, Sid, out var sid))
                return OnlyStatus(ValidateUserSessionStatus.BadSidCookie);

            if (!TryGetCookie(request, UserId, out var userId))
                return OnlyStatus(ValidateUserSessionStatus.BadUserCookie);

            if (!await sessionManager.ValidateSession(sid, userId))
                return OnlyStatus(ValidateUserSessionStatus.InvalidSession);

            return (ValidateUserSessionStatus.Ok, userId);

            (ValidateUserSessionStatus status, Guid? userId) OnlyStatus(ValidateUserSessionStatus status)
                => (status, null);
        }

        public async Task<(ValidateUserSessionStatus status, User user)> GetUserSafe(HttpRequest request)
        {
            var (sessionStatus, userId) = await GetUserIdSafe(request);

            if (sessionStatus != ValidateUserSessionStatus.Ok || !userId.HasValue)
                return (sessionStatus, null);

            return (ValidateUserSessionStatus.Ok, await userRepo.GetByIdAsync(userId.Value));
        }

        public void Clear(HttpResponse response)
        {
            response.Cookies.Delete(Sid);
            response.Cookies.Delete(UserId);
        }

        private static bool TryGetCookie(HttpRequest request, string cookieName, out Guid value)
        {
            value = default;
            return request.Cookies.TryGetValue(cookieName, out var valueStr) && Guid.TryParse(valueStr, out value);
        }
    }
}
