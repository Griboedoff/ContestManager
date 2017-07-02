using System.Web.Mvc;
using Core.Enums.RequestStatuses;
using Core.Exceptions;
using Core.Factories;
using Core.Managers;

namespace Front.Controllers
{
    public class LoginController : Controller
    {
        private readonly ICookieFactory cookieFactory;
        private readonly IAuthenticationManager authenticationManager;

        public LoginController(ICookieFactory cookieFactory, IAuthenticationManager authenticationManager)
        {
            this.cookieFactory = cookieFactory;
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
                var loginCookie = cookieFactory.CreateLoginCookie(user);
                Request.Cookies.Add(loginCookie);

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
                var loginCookie = cookieFactory.CreateLoginCookie(user);
                Request.Cookies.Add(loginCookie);

                return LoginStatus.Success;
            }
            catch (AuthenticationFailedException)
            {
                return LoginStatus.WrongVkAuthData;
            }
        }
    }
}