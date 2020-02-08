using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Core.Contests;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums;
using Core.Enums.DataBaseEnums;
using Core.SheetsApi;
using Front.React.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Front.React.Controllers
{
    [Authorized(UserRole.Admin)]
    public class ContestAdminController : ControllerBase
    {
        private readonly ILogger<ContestAdminController> logger;
        private readonly IContestManager contestManager;
        private readonly IContestAdminManager contestAdminManager;
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly ISheetsApiClient sheetsApiClient;

        public ContestAdminController(
            ILogger<ContestAdminController> logger,
            IContestManager contestManager,
            IContestAdminManager contestAdminManager,
            IAsyncRepository<Contest> contestsRepo,
            ISheetsApiClient sheetsApiClient)
        {
            this.logger = logger;
            this.contestManager = contestManager;
            this.contestAdminManager = contestAdminManager;
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

        [HttpPost("{id}/moveToMainPart")]
        public async Task<ActionResult> MoveToMainPart(Guid id, [FromBody] MoveToMainPartData data)
        {
            var moveStatus = await contestAdminManager.MoveParticipantsToMainPart(id, data.ToContest, data.Thresholds);
            return Json(moveStatus);
        }

        [HttpPost("{id}/verify")]
        public async Task<ActionResult> Verify(User user, Guid id, [FromBody] VerifyData data)
        {
            await contestAdminManager.VerifyParticipant(data.ParticipantId, data.Verification);
            logger.LogInformation($"{user.Name} подтвердил участника {data.ParticipantId}");

            return Ok();
        }

        [HttpPost("{id}/drawDiplomas")]
        public async Task<ActionResult> Verify(Guid id)
        {
            var pdf = await contestAdminManager.GenerateDiplomas(id);

            using (var ms = new MemoryStream())
            {
                pdf.Save(ms, false);

                return File(ms.ToArray(), MediaTypeNames.Application.Pdf, $"Diplomas.pdf");
            }
        }
    }

    public class VerifyData
    {
        public Guid ParticipantId { get; set; }
        public string Verification { get; set; }
    }

    public class MoveToMainPartData
    {
        public Guid ToContest { get; set; }
        public int[] Thresholds { get; set; }
    }
}
