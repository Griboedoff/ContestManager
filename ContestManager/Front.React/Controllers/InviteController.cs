using System;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Front.React.Controllers
{
    [Route("[controller]")]
    public class InviteController : Controller
    {
        private readonly IAsyncRepository<EmailConfirmationRequest> confirmationRequestsRepo;
        private readonly IAsyncRepository<AuthenticationAccount> authenticationAccountRepo;
        private readonly IOptions<ConfigOptions> options;

        public InviteController(
            IAsyncRepository<EmailConfirmationRequest> confirmationRequestsRepo,
            IAsyncRepository<AuthenticationAccount> authenticationAccountRepo,
            IOptions<ConfigOptions> options
        )
        {
            this.confirmationRequestsRepo = confirmationRequestsRepo;
            this.authenticationAccountRepo = authenticationAccountRepo;
            this.options = options;
        }

        [HttpGet]
        [Route("{code}")]
        public async Task<ActionResult> Accept(string code)
        {
            var request = await confirmationRequestsRepo.FirstOrDefaultAsync(
                r => r.Type == ConfirmationType.Registration && r.ConfirmationCode == code);

            var inviteLinkStatus = await CheckRequest(request);
            return View(
                new InviteModel
                {
                    Status = inviteLinkStatus,
                    Url = options.Value.SiteAddress,
                });
        }

        private async Task<InviteLinkStatus> CheckRequest(EmailConfirmationRequest request)
        {
            if (request == null)
                return InviteLinkStatus.WrongLink;
            if (request.IsUsed)
                return InviteLinkStatus.AlreadyUsed;

            try
            {
                var account = await authenticationAccountRepo.GetByIdAsync(request.AccountId);
                account.IsActive = true;
                await authenticationAccountRepo.UpdateAsync(account);

                request.IsUsed = true;
                await confirmationRequestsRepo.UpdateAsync(request);
            }
            catch (Exception e)
            {
                return InviteLinkStatus.Error;
            }

            return InviteLinkStatus.Ok;
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
    }
}