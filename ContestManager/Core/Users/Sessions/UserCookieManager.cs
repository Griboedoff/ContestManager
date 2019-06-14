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
        void SetLoginCookie(HttpResponse response, User user);
        Task<User> GetUser(HttpRequest request);
        void Clear(HttpResponse response);
    }

    public class UserCookieManager : IUserCookieManager
    {
        private readonly IAsyncRepository<User> userRepo;
        private const string Sid = "sid";
        private const string UserInfo = "User";

        public UserCookieManager(IAsyncRepository<User> userRepo) => this.userRepo = userRepo;

        public void SetLoginCookie(HttpResponse response, User user)
        {
            var sid = SessionManager.CreateSession(user.Name);

            response.Cookies.Append(
                Sid,
                sid,
                new CookieOptions { Expires = DateTimeOffset.Now.Add(TimeSpan.FromDays(1)) });
            response.Cookies.Append(UserInfo, CreateUserInfo(user));
        }

        public async Task<User> GetUser(HttpRequest request)
        {
            if (!request.Cookies.TryGetValue(Sid, out var sid) ||
                !TryGetUser(request, out var user) ||
                !SessionManager.ValidateSession(user.Name, sid))
                throw new UnauthorizedAccessException();

            return await userRepo.GetByIdAsync(user.Id);
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