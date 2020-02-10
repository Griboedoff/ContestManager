using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Users;
using Core.Users.Login;
using Core.Users.Registration;
using Core.Users.Sessions;
using Front.React.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Front.React.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> logger;
        private readonly IUserCookieManager userCookieManager;
        private readonly IAuthenticationManager authenticationManager;
        private readonly IUserManager userManager;
        private readonly IAsyncRepository<User> usersRepo;
        private readonly IAsyncRepository<AuthenticationAccount> authenticationAccountRepo;
        private readonly IAsyncRepository<Invite> invitesRepo;

        public UsersController(
            ILogger<UsersController> logger,
            IUserCookieManager userCookieManager,
            IAuthenticationManager authenticationManager,
            IUserManager userManager,
            IAsyncRepository<User> usersRepo,
            IAsyncRepository<AuthenticationAccount> authenticationAccountRepo,
            IAsyncRepository<Invite> invitesRepo
        )
        {
            this.logger = logger;
            this.userCookieManager = userCookieManager;
            this.authenticationManager = authenticationManager;
            this.userManager = userManager;
            this.usersRepo = usersRepo;
            this.authenticationAccountRepo = authenticationAccountRepo;
            this.invitesRepo = invitesRepo;
        }

        [HttpPost("login/email")]
        public async Task<ActionResult> Login([FromBody] EmailLoginInfo emailLoginInfo)
        {
            try
            {
                var user = await authenticationManager.Authenticate(emailLoginInfo);
                var sid = await userCookieManager.SetLoginCookie(Response, user);

                logger.LogInformation($"Успешный вход по логин-паролю {emailLoginInfo.Email}. sessionId {sid}");
                return Json(user);
            }
            catch (AuthenticationFailedException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("login/vk")]
        public async Task<ActionResult> Login([FromBody] VkLoginInfo vkLoginInfo)
        {
            try
            {
                var user = await authenticationManager.Authenticate(vkLoginInfo);
                var sid = await userCookieManager.SetLoginCookie(Response, user);

                logger.LogInformation($"Успешный вход по VK {vkLoginInfo.Mid}. sessionId {sid}");
                return Json(user);
            }
            catch (AuthenticationFailedException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("register/email")]
        public async Task<JsonResult> CreateEmailRegistrationRequest([FromBody] EmailRegisterInfo emailInfo)
        {
            var emailRegistrationRequest = await userManager.CreateEmailConfirmRequest(emailInfo);

            return Json(emailRegistrationRequest);
        }

        [HttpPost("register/vk")]
        public async Task<JsonResult> RegisterByVk([FromBody] VKRegisterInfo vkRegisterInfo)
        {
            var registerByVk = await userManager.RegisterByVk(vkRegisterInfo.Name, vkRegisterInfo.VkId);

            return Json(registerByVk);
        }

        [HttpPost("restore")]
        public async Task<ActionResult> RestorePassword([FromBody] string email)
        {
            var registrationRequestStatus = await userManager.CreatePasswordRestoreRequest(email);

            return Json(registrationRequestStatus);
        }

        [HttpGet("check")]
        [Authorized]
        public ActionResult Check(User user) => Json(user);

        [HttpPatch]
        [Authorized]
        public async Task<ActionResult> Patch([FromBody] User newUser, User user)
        {
            if (!RoleChangeValidator.Validate(user.Role, newUser.Role))
                return StatusCode(
                    403,
                    new { error = "Нельзя менять роль", from = user.Role, to = newUser.Role });

            user.Class = newUser.Class;
            user.School = newUser.School;
            user.Name = newUser.Name;
            user.Role = newUser.Role;
            user.Sex = newUser.Sex;
            user.City = newUser.City;
            user.Coach = newUser.Coach;
            await usersRepo.UpdateAsync(user);
            return Json(newUser);
        }

        [HttpPost("changePass/{code}")]
        public async Task<ActionResult> ChangePass([FromBody] string password, string code)
        {
            var invite = await invitesRepo.FirstOrDefaultAsync(r => r.ConfirmationCode == code);

            if (invite == null || invite.IsUsed || !invite.PasswordRestore)
                return StatusCode(400);

            var account = await authenticationAccountRepo.GetByIdAsync(invite.AccountId);

            invite.IsUsed = true;
            await invitesRepo.UpdateAsync(invite);

            var user = await usersRepo.GetByIdAsync(account.UserId);
            var isChanged = await userManager.ChangePassword(user, password);
            await userCookieManager.SetLoginCookie(Response, user);

            return isChanged ? Ok() : StatusCode(400);
        }

        [HttpPost("logout")]
        public void LogOut() => userCookieManager.Clear(Response);
    }
}
