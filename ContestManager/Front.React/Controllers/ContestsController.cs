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
using Core.Users.Sessions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Front.React.Controllers
{
    public class ContestsController : ControllerBase
    {
        private readonly IUserCookieManager cookieManager;
        private readonly IContestManager contestManager;
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly ISheetsApiClient sheetsApiClient;

        public ContestsController(
            IUserCookieManager cookieManager,
            IContestManager contestManager,
            IAsyncRepository<Contest> contestsRepo,
            ISheetsApiClient sheetsApiClient)
        {
            this.cookieManager = cookieManager;
            this.contestManager = contestManager;
            this.contestsRepo = contestsRepo;
            this.sheetsApiClient = sheetsApiClient;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateContestModel contestModel)
        {
            var user = await cookieManager.GetUser(Request);
            if (user.Role != UserRole.Admin)
                return StatusCode(403);

            var contest = await contestManager.Create(contestModel, user.Id);

            return Json(contest);
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

        [HttpGet("{id}/news")]
        public async Task<JsonResult> GetNews(Guid id)
        {
            var news = await contestManager.GetNews(id);

            return Json(news.OrderByDescending(n => n.CreationDate));
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(Guid id)
        {
            var contest = await contestsRepo.GetByIdAsync(id);

            return Json(contest);
        }

        [HttpPost("{id}/participate")]
        public async Task<ObjectResult> Participate(Guid id)
        {
            var user = await cookieManager.GetUser(Request);
            if (user.Role != UserRole.Participant)
                return StatusCode(400, "Неверная роль");
            if (string.IsNullOrEmpty(user.School))
                return StatusCode(400, "Не заполнена школа");
            if (!user.Class.HasValue)
                return StatusCode(400, "Не указан класс");

            await contestManager.AddParticipant(id, user);
            return StatusCode(200, "Успешно");
        }

        [HttpPatch("{id}/options")]
        public async Task<ActionResult> UpdateOptions(Guid id, [FromBody] ContestOptions options)
        {
            var user = await cookieManager.GetUser(Request);
            if (user.Role != UserRole.Admin)
                return StatusCode(403);

            await contestManager.UpdateOptions(id, options);

            return StatusCode(200);
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

        [HttpGet("{id}/generateSeating")]
        public async Task<StatusCodeResult> GenerateSeating(Guid id, [FromBody] Auditorium[] auditoriums)
        {
            var user = await cookieManager.GetUser(Request);
            if (user.Role != UserRole.Admin)
                return StatusCode(403);

            await contestManager.GenerateSeating(id, auditoriums);

            return Ok();
        }

        [HttpPost("{id}/resultsTable")]
        public async Task<ActionResult> CreateResultsTable(
            Guid id,
            [FromBody] Dictionary<Class, string> tasksDescriptions)
        {
            var user = await cookieManager.GetUser(Request);
            if (user.Role != UserRole.Admin)
                return StatusCode(403);

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
        public async Task<ActionResult> CreateResultsTable(Guid id)
        {
            var user = await cookieManager.GetUser(Request);
            if (user.Role != UserRole.Admin)
                return StatusCode(403);

            var contest = await contestsRepo.GetByIdAsync(id);
            var results = await sheetsApiClient.GetResults(contest.ResultsTableLink);

            await contestManager.AddResults(id, results);
            return Json(200);
        }
    }
}
