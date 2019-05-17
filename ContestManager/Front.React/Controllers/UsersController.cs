using System;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Login(string email, string password)
        {
            try
            {
                var user = await authenticationManager.Authenticate(email, password);
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
        public async Task<ActionResult> Login([FromBody] VkLoginInfo vkLoginInfo)
        {
            try
            {
                var user = await authenticationManager.Authenticate(vkLoginInfo);
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
        public async Task<JsonResult> CreateEmailRegistrationRequest([FromBody] EmailRegisterInfo emailInfo)
        {
            var emailRegistrationRequest = await userManager.CreateEmailRegistrationRequest(emailInfo);

            return Json(emailRegistrationRequest);
        }

        [HttpPost]
        [Route("register/vk")]
        public async Task<JsonResult> RegisterByVk([FromBody] VKRegisterInfo vkRegisterInfo)
        {
            var registerByVk = await userManager.RegisterByVk(vkRegisterInfo.Name, vkRegisterInfo.VkId);

            return Json(registerByVk);
        }

        [HttpGet]
        [Route("check")]
        public ActionResult Check()
        {
            try
            {
                return Json(userCookieManager.GetUser(Request));
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403);
            }
        }
    }
}