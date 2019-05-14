using Core.Enums.RequestStatuses;
using Core.Registration;
using Core.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Front.React.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserCookieManager userCookieManager;
        private readonly IAuthenticationManager authenticationManager;
        private readonly IUserManager userManager;

        public UsersController(
            IUserCookieManager userCookieManager,
            IAuthenticationManager authenticationManager,
            IUserManager userManager)
        {
            this.userCookieManager = userCookieManager;
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
                userCookieManager.SetLoginCookie(Response, user);

                return Json(user);
            }
            catch (AuthenticationFailedException)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("login/vk")]
        public ActionResult Login([FromBody] VkLoginInfo vkLoginInfo)
        {
            try
            {
                var user = authenticationManager.Authenticate(vkLoginInfo);
                userCookieManager.SetLoginCookie(Response, user);

                return Json(user);
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
        public RegistrationStatus ConfirmEmailRegistrationRequest(
            string name,
            string email,
            string password,
            string confirmationCode)
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