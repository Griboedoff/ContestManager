using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Contests;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums;
using Core.Enums.DataBaseEnums;
using Core.Users.Sessions;
using Front.React.Filters;
using Front.React.Models;
using Microsoft.AspNetCore.Mvc;
namespace Front.React.Controllers
{
    public class ContestsController : ControllerBase
    {
        private readonly IContestManager contestManager;
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly IUserCookieManager cookieManager;

        public ContestsController(IContestManager contestManager, IAsyncRepository<Contest> contestsRepo, IUserCookieManager cookieManager)
        {
            this.contestManager = contestManager;
            this.contestsRepo = contestsRepo;
            this.cookieManager = cookieManager;
        }

        [HttpGet]
        public async Task<JsonResult> List()
        {
            var contests = await contestsRepo.ListAllAsync();

            return Json(
                contests
                    .OrderByDescending(c => c.CreationDate)
                    .ToArray());
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(Guid id)
        {
            var contest = await contestsRepo.GetByIdAsync(id);

            return Json(contest);
        }

        [HttpGet("{id}/news")]
        public async Task<JsonResult> GetNews(Guid id)
        {
            var news = await contestManager.GetNews(id);

            return Json(news.OrderByDescending(n => n.CreationDate));
        }

        [HttpGet("{id}/participants")]
        public async Task<ActionResult> GetParticipants(Guid id, bool onlyNotVerified)
        {
            var participants = await contestManager.GetParticipants(id);

            if (onlyNotVerified)
                return Json(participants.Where(p => !p.Verified).ToArray());

            var contest = await contestsRepo.GetByIdAsync(id);

            return Json(participants.Where(p => !contest.Options.HasFlag(ContestOptions.FilterVerified) || p.Verified).ToArray());
        }

        [HttpGet("{id}/results")]
        public async Task<ActionResult> Results(Guid id)
        {
            var (_, user) = await cookieManager.GetUserSafe(Request);
            var contest = await contestsRepo.GetByIdAsync(id);
            var isAdmin = user?.Role == UserRole.Admin;
            if (isAdmin || contest.Options.HasFlag(ContestOptions.ResultsOpen))
                return Json(await contestManager.GetResults(id, false));
            if (contest.Options.HasFlag(ContestOptions.PreResultsOpen))
                return Json(await contestManager.GetResults(id, true));

            return NotFound();
        }

        [HttpPost("{id}/participate")]
        [Authorized(UserRole.Participant)]
        public async Task<ObjectResult> Participate(Guid id, User user, [FromBody] string verification = null)
        {
            if (string.IsNullOrEmpty(user.School))
                return StatusCode(400, "Не заполнена школа");
            if (!user.Class.HasValue)
                return StatusCode(400, "Не указан класс");

            var contest = await contestsRepo.GetByIdAsync(id);
            if (contest.Type == ContestType.Common && string.IsNullOrWhiteSpace(verification))
                return StatusCode(400, "Не заполнено подтверждение");

            await contestManager.AddOrUpdateParticipant(id, user, verification);

            return StatusCode(200, "Успешно");
        }

        [HttpPatch("{id}/updateParticipant")]
        [Authorized(UserRole.Participant, UserRole.Admin)]
        public async Task<ObjectResult> Update(Guid id, [FromBody] ParticipantData participantData, User user)
        {
            if (user.Role == UserRole.Participant && user.Id != participantData.User.Id)
                return StatusCode(400, "Нет прав");
            if (string.IsNullOrEmpty(participantData.User.School))
                return StatusCode(400, "Не заполнена школа");
            if (!participantData.User.Class.HasValue)
                return StatusCode(400, "Не указан класс");

            var contest = await contestsRepo.GetByIdAsync(id);
            if (contest.Type == ContestType.Common && string.IsNullOrWhiteSpace(participantData.Verification))
                return StatusCode(400, "Не заполнено подтверждение");

            await contestManager.AddOrUpdateParticipant(id, participantData.User, participantData.Verification);

            return StatusCode(200, "Успешно");
        }
    }
}
