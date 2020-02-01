using System;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Core.Users.Sessions
{
    public interface IUserCookieManager
    {
        Task SetLoginCookie(HttpResponse response, User user);
        Task<(ValidateUserSessionStatus status, User user)> GetUserSafe(HttpRequest request);
        void Clear(HttpResponse response);
    }

    public class UserCookieManager : IUserCookieManager
    {
        private readonly IAsyncRepository<User> userRepo;
        private readonly ISessionManager sessionManager;
        private const string Sid = "sid";
        private const string UserInfo = "User";

        public UserCookieManager(IAsyncRepository<User> userRepo, ISessionManager sessionManager)
        {
            this.userRepo = userRepo;
            this.sessionManager = sessionManager;
        }

        public async Task SetLoginCookie(HttpResponse response, User user)
        {
            var sid = await sessionManager.CreateSession(user);

            response.Cookies.Append(
                Sid,
                sid.ToString(),
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.Add(TimeSpan.FromDays(1)),
                    HttpOnly = true,
                    Secure = Constants.IsSecureCookie
                });
            response.Cookies.Append(
                UserInfo,
                CreateUserInfo(user),
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.Add(TimeSpan.FromDays(1)),
                    HttpOnly = true,
                    Secure = Constants.IsSecureCookie
                });
        }

        public async Task<(ValidateUserSessionStatus status, User user)> GetUserSafe(HttpRequest request)
        {
            if (!request.Cookies.TryGetValue(Sid, out var sidStr) || !Guid.TryParse(sidStr, out var sid))
                return OnlyStatus(ValidateUserSessionStatus.BadSidCookie);

            if (!TryGetUser(request, out var user))
                return OnlyStatus(ValidateUserSessionStatus.BadUserCookie);

            if (!await sessionManager.ValidateSession(sid, user.Id))
                return OnlyStatus(ValidateUserSessionStatus.InvalidSession);

            return (ValidateUserSessionStatus.Ok, await userRepo.GetByIdAsync(user.Id));

            (ValidateUserSessionStatus status, User user) OnlyStatus(ValidateUserSessionStatus status)
                => (status, null);
        }

        public void Clear(HttpResponse response)
        {
            response.Cookies.Delete(Sid);
            response.Cookies.Delete(UserInfo);
        }

        private static bool TryGetUser(HttpRequest request, out User user)
        {
            user = null;
            if (!request.Cookies.TryGetValue(UserInfo, out var userInfoJson))
                return false;

            var userInfo = JsonConvert.DeserializeObject<UserInfo>(userInfoJson);
            user = new User
            {
                Id = userInfo.Id,
                Name = userInfo.Name,
                Role = userInfo.Role,
            };
            return true;
        }

        private static string CreateUserInfo(User user) => JsonConvert.SerializeObject(
            new UserInfo
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role,
            });
    }

    internal class UserInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
    }
}
