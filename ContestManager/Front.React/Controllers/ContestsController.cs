using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Contests;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums;
using Core.Enums.DataBaseEnums;
using Core.SheetsApi;
using Front.React.Filters;
using Front.React.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Front.React.Controllers
{
    public class ContestsController : ControllerBase
    {
        private readonly IContestManager contestManager;
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly ISheetsApiClient sheetsApiClient;

        public ContestsController(
            IContestManager contestManager,
            IAsyncRepository<Contest> contestsRepo,
            ISheetsApiClient sheetsApiClient)
        {
            this.contestManager = contestManager;
            this.contestsRepo = contestsRepo;
            this.sheetsApiClient = sheetsApiClient;
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
        public async Task<ActionResult> GetParticipants(Guid id)
        {
            var participants = await contestManager.GetParticipants(id);

            return Json(participants);
        }

        [HttpGet("{id}/results")]
        public string Results(Guid id)
        {
            var participants = contestManager.GetParticipants(id);

            return JsonConvert.SerializeObject(participants);
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

        [HttpPost]
        [Authorized(UserRole.Admin)]
        public async Task<ActionResult> Create([FromBody] CreateContestModel contestModel, User user)
        {
            var contest = await contestManager.Create(contestModel, user.Id);

            return Json(contest);
        }

        [HttpPatch("{id}/options")]
        [Authorized(UserRole.Admin)]
        public async Task<ActionResult> UpdateOptions(Guid id, [FromBody] ContestOptions options)
        {
            await contestManager.UpdateOptions(id, options);

            return StatusCode(200);
        }

        [HttpGet("{id}/generateSeating")]
        [Authorized(UserRole.Admin)]
        public async Task<StatusCodeResult> GenerateSeating(Guid id, [FromBody] Auditorium[] auditoriums)
        {
            await contestManager.GenerateSeating(id, auditoriums);

            return Ok();
        }

        [HttpPost("{id}/resultsTable")]
        [Authorized(UserRole.Admin)]
        public async Task<ActionResult> CreateResultsTable(
            Guid id,
            [FromBody] Dictionary<Class, string> tasksDescriptions)
        {
            await contestManager.AddResultsDescription(id, tasksDescriptions);

            var contest = await contestsRepo.GetByIdAsync(id);

            var tableId = await sheetsApiClient.CreateTable(contest.Title);
            contest.ResultsTableLink = tableId;
            await contestsRepo.UpdateAsync(contest);

            var participants = await contestManager.GetParticipants(id);

            await sheetsApiClient.FillParticipantsData(tableId, participants, contest.TasksDescription);

            return Json(tableId);
        }

        [HttpPost("{id}/fetchResults")]
        [Authorized(UserRole.Admin)]
        public async Task<ActionResult> CreateResultsTable(Guid id)
        {
            var contest = await contestsRepo.GetByIdAsync(id);
            var results = await sheetsApiClient.GetResults(contest.ResultsTableLink);

            await contestManager.AddResults(id, results);
            return Json(200);
        }
    }
}
