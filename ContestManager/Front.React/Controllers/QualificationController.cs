using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums;
using Core.Enums.DataBaseEnums;
using Front.React.Filters;
using Front.React.Models;
using Microsoft.AspNetCore.Mvc;

namespace Front.React.Controllers
{
    public class QualificationController : ControllerBase
    {
        private readonly IAsyncRepository<QualificationTask> tasksRepo;
        private readonly IAsyncRepository<Participant> participantRepo;
        private readonly IAsyncRepository<Contest> contestRepo;
        private readonly IAsyncRepository<QualificationParticipation> participationRepo;
        private static readonly TimeSpan RoundTime = TimeSpan.FromHours(3);

        public QualificationController(
            IAsyncRepository<QualificationTask> tasksRepo,
            IAsyncRepository<Participant> participantRepo,
            IAsyncRepository<Contest> contestRepo,
            IAsyncRepository<QualificationParticipation> participationRepo)
        {
            this.tasksRepo = tasksRepo;
            this.participantRepo = participantRepo;
            this.contestRepo = contestRepo;
            this.participationRepo = participationRepo;
        }

        [HttpPost("start")]
        [Authorized(UserRole.Participant)]
        public async Task<ActionResult> Start(Guid contestId, User user)
        {
            var participant = await participantRepo.FirstOrDefaultAsync(p => p.UserId == user.Id && p.ContestId == contestId);
            if (participant == null)
                return StatusCode(400, "No participant for contest");

            var tasks = await GetTasks(participant);

            var participation = new QualificationParticipation
            {
                Id = Guid.NewGuid(),
                ContestId = contestId,
                ParticipantId = participant.Id,
                EndTime = DateTimeOffset.UtcNow + RoundTime,
                Answers = tasks.OrderBy(t => t.Number).Select(_ => "").ToArray(),
            };

            var qualificationParticipation = await participationRepo.AddAsync(participation);

            return Json(qualificationParticipation);
        }

        [HttpGet("state")]
        [Authorized(UserRole.Participant)]
        public async Task<ActionResult> IsStarted(Guid contestId, User user)
        {
            var participant = await participantRepo.FirstOrDefaultAsync(p => p.UserId == user.Id && p.ContestId == contestId);
            if (participant == null)
                return StatusCode(400, "No participant for contest");

            var participation = await participationRepo.FirstOrDefaultAsync(p => p.ParticipantId == participant.Id);
            if (participation == null)
                return Json(QualificationSolveState.NotStarted);

            if (DateTimeOffset.UtcNow < participation.EndTime.ToUniversalTime())
                return Json(QualificationSolveState.InProgress);

            return Json(QualificationSolveState.Finished);
        }

        [HttpGet]
        [Authorized(UserRole.Participant)]
        public async Task<ActionResult> GetQualificationState(Guid contestId, User user)
        {
            var contest = await contestRepo.GetByIdAsync(contestId);
            if (!contest.Options.HasFlag(ContestOptions.QualificationOpen))
                return StatusCode(400, "contest closed");

            var participant = await participantRepo.FirstOrDefaultAsync(p => p.UserId == user.Id && p.ContestId == contestId);
            if (participant == null)
                return StatusCode(403, "No participant for contest");

            var participation = await participationRepo.FirstOrDefaultAsync(p => p.ParticipantId == participant.Id);
            if (participation == null)
                return StatusCode(400, "not started");

            var tasks = await GetTasks(participant);
            return Json(
                new QualificationState
                {
                    Title = contest.Title,
                    Answers = participation.Answers,
                    Tasks = tasks.OrderBy(t => t.Number).Select(t => t.Text).ToArray(),
                    TimeLeft = (int) (participation.EndTime.ToUniversalTime() - DateTimeOffset.UtcNow).TotalSeconds,
                });
        }

        [HttpPost("save")]
        [Authorized(UserRole.Participant)]
        public async Task<ActionResult> Save(Guid contestId, User user, [FromBody] string[] answers)
        {
            var contest = await contestRepo.GetByIdAsync(contestId);
            if (!contest.Options.HasFlag(ContestOptions.QualificationOpen))
                return StatusCode(403, "contest closed");

            var participant = await participantRepo.FirstOrDefaultAsync(p => p.UserId == user.Id && p.ContestId == contestId);
            if (participant == null)
                return StatusCode(403, "No participant for contest");

            var participation = await participationRepo.FirstOrDefaultAsync(p => p.ParticipantId == participant.Id);
            if (DateTimeOffset.UtcNow > participation.EndTime.ToUniversalTime())
                return BadRequest();

            participation.Answers = answers;
            await participationRepo.UpdateAsync(participation);

            return Ok();
        }

        private async Task<IReadOnlyList<QualificationTask>> GetTasks(Participant participant)
        {
            return await tasksRepo.WhereAsync(
                t => t.ContestId == participant.ContestId && t.Classes.Contains(participant.UserSnapshot.Class.Value));
        }
    }
}
