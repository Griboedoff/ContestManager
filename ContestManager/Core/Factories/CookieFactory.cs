using System.Web;
using Core.DataBaseEntities;
using Core.Extensions;
using Core.Helpers;

namespace Core.Factories
{
    public interface ICookieFactory
    {
        HttpCookie CreateLoginCookie(User user);
    }

    public class CookieFactory : ICookieFactory
    {
        private readonly ICryptoHelper cryptoHelper;

        public CookieFactory(ICryptoHelper cryptoHelper)
        {
            this.cryptoHelper = cryptoHelper;
        }

        public HttpCookie CreateLoginCookie(User user)
        {
            var str = $"Id={user.Id:D}&Name={user.Name}&Role={user.Role:G}";
            var bytes = str.ToBytes();
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