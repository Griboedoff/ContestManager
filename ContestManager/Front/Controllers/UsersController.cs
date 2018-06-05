using System.Web.Http.Cors;
using System.Web.Mvc;
using Core.Enums.RequestStatuses;
using Core.Exceptions;
using Core.Managers;
using Front.Helpers;

namespace Front.Controllers
{
    [RoutePrefix("users")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : Controller
    {
        private readonly ICookieManager cookieManager;
        private readonly IAuthenticationManager authenticationManager;
        private readonly IRegistrationManager registrationManager;

        public UsersController(ICookieManager cookieManager,
            IAuthenticationManager authenticationManager,
            IRegistrationManager registrationManager)
        {
            this.cookieManager = cookieManager;
            this.authenticationManager = authenticationManager;
            this.registrationManager = registrationManager;
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
        public RegistrationStatus CreateEmailRegistrationRequest(string userEmail)
        {
            return registrationManager.CreateEmailRegistrationRequest(userEmail);
        }

        [HttpPost]
        [Route("register/email/confirm")]
        public RegistrationStatus ConfirmEmailRegistrationRequest(string userName, string userEmail,
            string userPassword, string confirmationCode)
        {
            return registrationManager.ConfirmEmailRegistrationRequest(userName, userEmail, userPassword,
                confirmationCode);
        }

        [HttpPost]
        [Route("register/vk")]
        public RegistrationStatus RegisterByVk(string name, string vkId)
        {
            return registrationManager.RegisterByVk(name, vkId);
        }
    }
}