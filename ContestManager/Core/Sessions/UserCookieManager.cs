using System;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Core.Sessions
{
    public interface IUserCookieManager
    {
        void SetLoginCookie(HttpResponse response, User user);
        User GetUser(HttpRequest request);
    }

    public class UserCookieManager : IUserCookieManager
    {
        private readonly ICryptoHelper cryptoHelper;
        private const string Sid = "sid";
        private const string UserInfo = "User";

        public UserCookieManager(ICryptoHelper cryptoHelper)
        {
            this.cryptoHelper = cryptoHelper;
        }

        public void SetLoginCookie(HttpResponse response, User user)
        {
            var sid = SessionManager.CreateSession(user.Name);

            response.Cookies.Append(Sid, sid);
            response.Cookies.Append(UserInfo, CreateUserInfo(user));
        }

        public User GetUser(HttpRequest request)
        {
            if (!request.Cookies.TryGetValue(Sid, out var sid) ||
                !TryGetUser(request, out var user) ||
                !SessionManager.ValidateSession(user.Name, sid))
                throw new UnauthorizedAccessException();

            return user;
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