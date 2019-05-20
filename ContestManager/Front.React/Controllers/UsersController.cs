using System;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Managers;
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
        private readonly IAsyncRepository<User> usersRepo;

        public UsersController(
            IUserCookieManager userCookieManager,
            IAuthenticationManager authenticationManager,
            IUserManager userManager,
            IAsyncRepository<User> usersRepo)
        {
            this.userCookieManager = userCookieManager;
            this.authenticationManager = authenticationManager;
            this.userManager = userManager;
            this.usersRepo = usersRepo;
        }

        [HttpPost("login/email")]
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

        [HttpPost("login/vk")]
        public async Task<ActionResult> Login([FromBody] VkLoginInfo vkLoginInfo)
        {
            try
            {
                var user = await authenticationManager.Authenticate(vkLoginInfo);
                userCookieManager.SetLoginCookie(Response, user);

                var actionResult = Json(user);
                return actionResult;
            }
            catch (AuthenticationFailedException)
            {
                return null;
            }
        }

        [HttpPost("register/email")]
        public async Task<JsonResult> CreateEmailRegistrationRequest([FromBody] EmailRegisterInfo emailInfo)
        {
            var emailRegistrationRequest = await userManager.CreateEmailRegistrationRequest(emailInfo);

            return Json(emailRegistrationRequest);
        }

        [HttpPost("register/vk")]
        public async Task<JsonResult> RegisterByVk([FromBody] VKRegisterInfo vkRegisterInfo)
        {
            var registerByVk = await userManager.RegisterByVk(vkRegisterInfo.Name, vkRegisterInfo.VkId);

            return Json(registerByVk);
        }

        [HttpGet("check")]
        public async Task<ActionResult> Check()
        {
            try
            {
                var userId = userCookieManager.GetUser(Request).Id;
                var user = await usersRepo.GetByIdAsync(userId);

                return Json(user);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403);
            }
        }

        [HttpPatch]
        public async Task<ActionResult> Patch([FromBody] User user)
        {
            try
            {
                var userFromDb = userCookieManager.GetUser(Request);

                if (!RoleChangeValidator.Validate(userFromDb.Role, user.Role))
                    return StatusCode(
                        403,
                        new { error = "Нельзя менять роль", from = userFromDb.Role, to = user.Role });

                await usersRepo.UpdateAsync(user);
                return Json(user);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403);
            }
        }

        [HttpPost("logout")]
        public void LogOut() => userCookieManager.Clear(Response);
    }
}