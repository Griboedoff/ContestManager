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
using Microsoft.AspNetCore.Mvc;

namespace Front.React.Controllers
{
    [Authorized(UserRole.Admin)]
    public class ContestAdminController : ControllerBase
    {
        private readonly IContestManager contestManager;
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly ISheetsApiClient sheetsApiClient;

        public ContestAdminController(
            IContestManager contestManager,
            IAsyncRepository<Contest> contestsRepo,
            ISheetsApiClient sheetsApiClient)
        {
            this.contestManager = contestManager;
            this.contestsRepo = contestsRepo;
            this.sheetsApiClient = sheetsApiClient;
        }

        [HttpPost("{id}/calcQualificationResults")]
        public async Task<ActionResult> CalculateQualificationResults(Guid id)
        {
            await contestManager.CalculateQualificationResults(id);

            return Ok();
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CreateContestModel contestModel, User user)
        {
            var contest = await contestManager.Create(contestModel, user.Id);

            return Json(contest);
        }

        [HttpPatch("{id}/options")]
        public async Task<ActionResult> UpdateOptions(Guid id, [FromBody] ContestOptions options)
        {
            await contestManager.UpdateOptions(id, options);

            return StatusCode(200);
        }

        [HttpGet("{id}/generateSeating")]
        public async Task<StatusCodeResult> GenerateSeating(Guid id, [FromBody] Auditorium[] auditoriums)
        {
            await contestManager.GenerateSeating(id, auditoriums);

            return Ok();
        }

        [HttpPost("{id}/resultsTable")]
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
        public async Task<ActionResult> CreateResultsTable(Guid id)
        {
            var contest = await contestsRepo.GetByIdAsync(id);
            var results = await sheetsApiClient.GetResults(contest.ResultsTableLink);

            await contestManager.AddResults(id, results);
            return Json(200);
        }
    }
}
