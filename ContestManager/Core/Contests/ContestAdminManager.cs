using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Extensions;
using Microsoft.Extensions.Logging;

namespace Core.Contests
{
    public interface IContestAdminManager
    {
        Task<MoveParticipantsStatus> MoveParticipantsToMainPart(
            Guid fromContestId,
            Guid toContestId,
            int[] tasksThreshold);

        Task VerifyParticipant(Guid participantId, string verification);
    }

    public class ContestAdminManager : IContestAdminManager
    {
        private readonly ILogger<ContestAdminManager> logger;
        private readonly IContestManager contestManager;
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly IAsyncRepository<Participant> participantsRepo;
        private readonly IAsyncRepository<QualificationParticipation> qualificationParticipationRepo;
        private readonly IAsyncRepository<QualificationTask> qualificationTaskRepo;

        public ContestAdminManager(
            ILogger<ContestAdminManager> logger,
            IContestManager contestManager,
            IAsyncRepository<Contest> contestsRepo,
            IAsyncRepository<Participant> participantsRepo,
            IAsyncRepository<QualificationParticipation> qualificationParticipationRepo,
            IAsyncRepository<QualificationTask> qualificationTaskRepo)
        {
            this.logger = logger;
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
    }
}
