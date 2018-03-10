using System;
using System.Web;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Extensions;
using Core.Helpers;

namespace Core.Managers
{
    public interface ICookieManager
    {
        void SetLoginCookie(HttpResponseBase request, User user);
        User GetUser(HttpRequestBase request);
    }

    public class CookieManager : ICookieManager
    {
        private readonly ICryptoHelper cryptoHelper;

        public CookieManager(ICryptoHelper cryptoHelper)
        {
            this.cryptoHelper = cryptoHelper;
        }

        public void SetLoginCookie(HttpResponseBase request, User user)
        {
            var loginCookie = CreateLoginCookie(user);
            request.Cookies.Add(loginCookie);
        }

        public User GetUser(HttpRequestBase request)
        {
            var cookie = request.Cookies[Secret.LoginCookieName];

            if (cookie == null)
                throw new Exception();

            var bytes = $"Id={cookie["Id"]}&Name={cookie["Name"]}&Role={cookie["Role"]}".ToBytes();
            if (cryptoHelper.VerifyDetachedSign(bytes, cookie["Sign"].FromBase64()))
                return new User()
                {
                    Id = Guid.Parse(cookie["Id"]),
                    Name = cookie["Name"],
                    Role = (UserRole)Enum.Parse(typeof(UserRole), cookie["Role"]),
                };

            request.Cookies.Remove(Secret.LoginCookieName);
            throw new Exception();
        }

        private HttpCookie CreateLoginCookie(User user)
        {
            var bytes = $"Id={user.Id:D}&Name={user.Name}&Role={user.Role:G}".ToBytes();
            var sign = cryptoHelper.ComputeDetachedSign(bytes);

            return new HttpCookie(Secret.LoginCookieName)
            {
                ["Id"] = $"{user.Id:D}",
                ["Name"] = user.Name,
                ["Role"] = $"{user.Role:G}",
                ["Sign"] = sign.ToBase64()
            };
        }
    }
}