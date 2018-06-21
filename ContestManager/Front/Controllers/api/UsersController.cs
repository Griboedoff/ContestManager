using System.Web.Mvc;
using Core.Enums.RequestStatuses;
using Core.Exceptions;
using Core.Managers;
using Front.Helpers;

namespace Front.Controllers.api
{
    [RoutePrefix("users")]
    public class UsersController : Controller
    {
        private readonly ICookieManager cookieManager;
        private readonly IAuthenticationManager authenticationManager;
        private readonly IUserManager userManager;

        public UsersController(ICookieManager cookieManager,
            IAuthenticationManager authenticationManager,
            IUserManager userManager)
        {
            this.cookieManager = cookieManager;
            this.authenticationManager = authenticationManager;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("login/email")]
        public ActionResult Login(string email, string password)
        {
            try
            {
                var user = authenticationManager.Authenticate(email, password);
                cookieManager.SetLoginCookie(Response, user);

                return new JsonNetResult
                {
                    Data = user,
                };
            }
            catch (AuthenticationFailedException)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("login/vk")]
        public ActionResult Login(long expire, string mid, string secret, string sid, string sig)
        {
            try
            {
                var user = authenticationManager.Authenticate(expire, mid, secret, sid, sig);
                cookieManager.SetLoginCookie(Response, user);

                return new JsonNetResult
                {
                    Data = user,
                };
            }
            catch (AuthenticationFailedException)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("register/email")]
        public RegistrationStatus CreateEmailRegistrationRequest(string email)
        {
            return userManager.CreateEmailRegistrationRequest(email);
        }

        [HttpPost]
        [Route("register/email/confirm")]
        public RegistrationStatus ConfirmEmailRegistrationRequest(string name, string email,
            string password, string confirmationCode)
        {
            return userManager.ConfirmEmailRegistrationRequest(name, email, password, confirmationCode);
        }

        [HttpPost]
        [Route("register/vk")]
        public RegistrationStatus RegisterByVk(string name, string vkId)
        {
            return userManager.RegisterByVk(name, vkId);
        }
    }
}