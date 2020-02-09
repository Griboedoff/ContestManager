using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Diplomas;
using Core.Extensions;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace Core.Contests
{
    public interface IContestAdminManager
    {
        Task<MoveParticipantsStatus> MoveParticipantsToMainPart(
            Guid fromContestId,
            Guid toContestId,
            int[] tasksThreshold);

        Task VerifyParticipant(Guid participantId, string verification);
        Task<PdfDocument> GenerateDiplomas(Guid contestId);
    }

    public class ContestAdminManager : IContestAdminManager
    {
        private readonly ILogger<ContestAdminManager> logger;
        private readonly IDiplomaDrawer diplomaDrawer;
        private readonly IContestManager contestManager;
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly IAsyncRepository<Participant> participantsRepo;
        private readonly IAsyncRepository<QualificationParticipation> qualificationParticipationRepo;
        private readonly IAsyncRepository<QualificationTask> qualificationTaskRepo;

        public ContestAdminManager(
            ILogger<ContestAdminManager> logger,
            IDiplomaDrawer diplomaDrawer,
            IContestManager contestManager,
            IAsyncRepository<Contest> contestsRepo,
            IAsyncRepository<Participant> participantsRepo,
            IAsyncRepository<QualificationParticipation> qualificationParticipationRepo,
            IAsyncRepository<QualificationTask> qualificationTaskRepo)
        {
            this.logger = logger;
            this.diplomaDrawer = diplomaDrawer;
            this.contestManager = contestManager;
            this.contestsRepo = contestsRepo;
            this.participantsRepo = participantsRepo;
            this.qualificationParticipationRepo = qualificationParticipationRepo;
            this.qualificationTaskRepo = qualificationTaskRepo;
        }

        public async Task<MoveParticipantsStatus> MoveParticipantsToMainPart(
            Guid fromContestId,
            Guid toContestId,
            int[] tasksThreshold)
        {
            var thresholdsByClass = Enum.GetValues(typeof(Class))
                .Cast<Class>()
                .ToDictionary(
                    c => c,
                    c => tasksThreshold[(int) c - 5]);

            logger.LogInformation(
                $"Перевожу участников из {fromContestId} в {toContestId}. Граница {thresholdsByClass.Select(kv => $"{kv.Key:G}: {kv.Value}").JoinToString(", ")}");

            var participants = (await participantsRepo.WhereAsync(p => p.ContestId == fromContestId))
                .Where(p => p.UserSnapshot.Class.HasValue)
                .Where(p => p.Results.Select(int.Parse).Sum() >= thresholdsByClass[p.UserSnapshot.Class.Value])
                .Select(
                    p => new Participant
                    {
                        Id = Guid.NewGuid(),
                        ContestId = toContestId,
                        UserId = p.UserId,
                        UserSnapshot = p.UserSnapshot,
                        Verification = "Отборочный тур",
                        Verified = true,
                    });

            foreach (var newParticipant in participants)
            {
                await participantsRepo.AddAsync(newParticipant);
                logger.LogInformation(
                    $"{newParticipant.UserId} {newParticipant.UserSnapshot.Name} переведен в {toContestId}");
            }

            return MoveParticipantsStatus.Ok;
        }

        public async Task VerifyParticipant(Guid participantId, string verification)
        {
            var participant = await participantsRepo.GetByIdAsync(participantId);
            participant.Verification = verification;
            participant.Verified = true;

            await participantsRepo.UpdateAsync(participant);
        }

        public async Task<PdfDocument> GenerateDiplomas(Guid contestId)
        {
            var participants = await participantsRepo.WhereAsync(p => p.ContestId == contestId && p.Verified);
            var diplomasData = participants
                .OrderBy(p => p.UserSnapshot.Class)
                .ThenByDescending(p => p.ResultsAsNumbers().Sum()).Select(
                p => new DiplomaData
                {
                    Name = p.UserSnapshot.Name,
                    Sex = p.UserSnapshot.Sex,
                    Class = p.UserSnapshot.Class.Value,
                    Coach = p.UserSnapshot.Coach,
                    Institution = p.UserSnapshot.School,
                    City = p.UserSnapshot.City,
                    Position = p.Place,
                });

            var positionDiplomas = new List<Diploma>();
            var coachDiplomas = new List<Diploma>();

            foreach (var dData in diplomasData)
            {
                if (dData.HasPositionDiploma())
                    positionDiplomas.Add(dData.GetPositionDiploma());

                if (dData.HasCoachDiploma())
                    coachDiplomas.AddRange(dData.GetCoachDiplomas());
            }

            positionDiplomas.AddRange(coachDiplomas.Distinct());

            return diplomaDrawer.DrawDiplomas(positionDiplomas);
        }
    }
}
