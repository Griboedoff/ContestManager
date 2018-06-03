using System.Web.Mvc;
using Core.Exceptions;
using Core.Managers;
using Front.Helpers;
using Newtonsoft.Json;

namespace Front.Controllers
{
    [RoutePrefix("users")]
    public class UsersController : Controller
    {
        private readonly ICookieManager cookieManager;
        private readonly IAuthenticationManager authenticationManager;

        public UsersController(ICookieManager cookieManager,
            IAuthenticationManager authenticationManager)
        {
            this.cookieManager = cookieManager;
            this.authenticationManager = authenticationManager;
        }

        // GET
        [Route("login")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("email")]
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            try
            {
                var user = authenticationManager.Authenticate(email, password);
                cookieManager.SetLoginCookie(Response, user);

                return new JsonNetResult {
                    Data = user,
                };
            }
            catch (AuthenticationFailedException)
            {
                return null;
            }
        }

        [Route("vk")]
        [HttpPost]
        public ActionResult Login(long expire, string mid, string secret, string sid, string sig)
        {
            try
            {
                var user = authenticationManager.Authenticate(expire, mid, secret, sid, sig);
                cookieManager.SetLoginCookie(Response, user);

                return new JsonNetResult {
                    Data = user,
                };
            }
            catch (AuthenticationFailedException)
            {
                return null;
            }
        }
    }
}