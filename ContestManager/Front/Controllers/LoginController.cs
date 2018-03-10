using System.Web.Mvc;
using Core.Enums.RequestStatuses;
using Core.Exceptions;
using Core.Managers;

namespace Front.Controllers
{
    public class LoginController : Controller
    {
        private readonly ICookieManager cookieManager;
        private readonly IAuthenticationManager authenticationManager;

        public LoginController(ICookieManager cookieManager,
            IAuthenticationManager authenticationManager)
        {
            this.cookieManager = cookieManager;
            this.authenticationManager = authenticationManager;
        }

        [HttpGet]
        public ActionResult Index()
            => View();

        [HttpPost]
        public LoginStatus PasswordLogin(string userEmail, string userPassword)
        {
            try
            {
                var user = authenticationManager.Authenticate(userEmail, userPassword);
                cookieManager.SetLoginCookie(Response, user);

                return LoginStatus.Success;
            }
            catch (AuthenticationFailedException)
            {
                return LoginStatus.WrongEmailOrPassword;
            }
        }


        [HttpPost]
        public LoginStatus VkLogin(long expire, string mid, string secret, string sid, string sig)
        {
            try
            {
                var user = authenticationManager.Authenticate(expire, mid, secret, sid, sig);
                cookieManager.SetLoginCookie(Response, user);

                return LoginStatus.Success;
            }
            catch (AuthenticationFailedException)
            {
                return LoginStatus.WrongVkAuthData;
            }
        }
    }
}