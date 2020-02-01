using System;
using System.Threading.Tasks;
using Core.Configs;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Users.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Front.React.Controllers
{
    public class InviteController : ControllerBase
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

            if (inviteLinkStatus == InviteLinkStatus.Ok)
                try
                {
                    await ActivateAccount(invite, authenticationAccount);
                }
                catch (Exception)
                {
                    return Json(InviteLinkStatus.Error);
                }

            return Json(inviteLinkStatus);
        }

        private async Task ActivateAccount(Invite invite, AuthenticationAccount account)
        {
            account.IsActive = true;
            await authenticationAccountRepo.UpdateAsync(account);

            invite.IsUsed = true;
            await invitesRepo.UpdateAsync(invite);

            var user = await userRepo.GetByIdAsync(account.UserId);
            await userCookieManager.SetLoginCookie(Response, user);
        }

        private async Task<(InviteLinkStatus, AuthenticationAccount)> CheckInvite(Invite invite)
        {
            if (invite == null)
                return (InviteLinkStatus.WrongLink, null);
            if (invite.IsUsed)
                return (InviteLinkStatus.AlreadyUsed, null);

            var account = await authenticationAccountRepo.GetByIdAsync(invite.AccountId);
            return (invite.PasswordRestore ? InviteLinkStatus.RestorePassword : InviteLinkStatus.Ok, account);
        }
    }

    public class InviteModel
    {
        public InviteLinkStatus Status { get; set; }
        public string Url { get; set; }
    }

    public enum InviteLinkStatus
    {
        WrongLink = 0,
        AlreadyUsed = 1,
        Ok = 2,
        Error = 3,
        RestorePassword = 4,
    }
}
