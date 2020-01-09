using System;
using System.Threading.Tasks;
using Core.Configs;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Users.Registration;
using Core.Users.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Front.React.Controllers
{
    [Route("[controller]")]
    [ResponseCache(NoStore = true, Duration = 0)]
    public class InviteController : Controller
    {
        private readonly IAsyncRepository<Invite> invitesRepo;
        private readonly IAsyncRepository<AuthenticationAccount> authenticationAccountRepo;
        private readonly IAsyncRepository<User> userRepo;
        private readonly IOptions<ConfigOptions> options;
        private readonly IUserCookieManager userCookieManager;

        public InviteController(
            IAsyncRepository<Invite> invitesRepo,
            IAsyncRepository<AuthenticationAccount> authenticationAccountRepo,
            IAsyncRepository<User> userRepo,
            IOptions<ConfigOptions> options,
            IAuthenticationAccountFactory authenticationAccountFactory,
            IUserCookieManager userCookieManager
        )
        {
            this.invitesRepo = invitesRepo;
            this.authenticationAccountRepo = authenticationAccountRepo;
            this.userRepo = userRepo;
            this.options = options;
            this.userCookieManager = userCookieManager;
        }

        [HttpGet("{code}")]
        public async Task<ActionResult> Accept(string code)
        {
            var invite = await invitesRepo.FirstOrDefaultAsync(
                r => r.Type == ConfirmationType.Registration && r.ConfirmationCode == code);

            var (inviteLinkStatus, authenticationAccount) = await CheckInvite(invite);

            if (inviteLinkStatus == InviteLinkStatus.Ok || inviteLinkStatus == InviteLinkStatus.RestorePassword)
            {
                var user = await userRepo.GetByIdAsync(authenticationAccount.UserId);
                userCookieManager.SetLoginCookie(Response, user);
            }

            return View(
                new InviteModel
                {
                    Status = inviteLinkStatus,
                    Url = options.Value.SiteAddress,
                });
        }

        private async Task<(InviteLinkStatus, AuthenticationAccount)> CheckInvite(Invite invite)
        {
            if (invite == null)
                return (InviteLinkStatus.WrongLink, null);
            if (invite.IsUsed)
                return (InviteLinkStatus.AlreadyUsed, null);

            try
            {
                var account = await authenticationAccountRepo.GetByIdAsync(invite.AccountId);
                account.IsActive = true;
                await authenticationAccountRepo.UpdateAsync(account);

                invite.IsUsed = true;
                await invitesRepo.UpdateAsync(invite);

                return (invite.PasswordRestore ? InviteLinkStatus.RestorePassword : InviteLinkStatus.Ok, account);
            }
            catch (Exception)
            {
                return (InviteLinkStatus.Error, null);
            }
        }
    }

    public class InviteModel
    {
        public InviteLinkStatus Status { get; set; }
        public string Url { get; set; }
    }

    public enum InviteLinkStatus
    {
        WrongLink,
        AlreadyUsed,
        Ok,
        Error,
        RestorePassword,
    }
}
