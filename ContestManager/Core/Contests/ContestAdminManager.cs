using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Diplomas;
using Core.Enums;
using Core.Extensions;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;

namespace Core.Contests
{
    public interface IContestAdminManager
    {
        Task<Contest> Create(CreateContestModel title, Guid ownerId);
        Task UpdateOptions(Guid contestId, ContestOptions newOptions);

        Task CalculateQualificationResults(Guid contestId);
        Task<MoveParticipantsStatus> MoveParticipantsToMainPart(
            Guid fromContestId,
            Guid toContestId,
            int[] tasksThreshold);

        Task VerifyParticipant(Guid participantId, string verification);
        Task GenerateSeating(Guid contestId, Auditorium[] auditoriums);
        Task AddResultsDescription(Guid contestId, Dictionary<Class, string> tasksDescription);
        Task AddResults(Guid contestId, Dictionary<string, List<string>> results);
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
        private readonly Context context;
        private readonly ISeatingGenerator seatingGenerator;

        public ContestAdminManager(
            ILogger<ContestAdminManager> logger,
            IDiplomaDrawer diplomaDrawer,
            IContestManager contestManager,
            IAsyncRepository<Contest> contestsRepo,
            IAsyncRepository<Participant> participantsRepo,
            IAsyncRepository<QualificationParticipation> qualificationParticipationRepo,
            IAsyncRepository<QualificationTask> qualificationTaskRepo,
            Context context,
            ISeatingGenerator seatingGenerator)
        {
            this.logger = logger;
            this.diplomaDrawer = diplomaDrawer;
            this.contestManager = contestManager;
            this.contestsRepo = contestsRepo;
            this.participantsRepo = participantsRepo;
            this.qualificationParticipationRepo = qualificationParticipationRepo;
            this.qualificationTaskRepo = qualificationTaskRepo;
            this.context = context;
            this.seatingGenerator = seatingGenerator;
        }

        public async Task<Contest> Create(CreateContestModel contestModel, Guid ownerId)
        {
            var contest = new Contest
            {
                Id = Guid.NewGuid(),
                Title = contestModel.Title,
                Type = contestModel.Type,
                OwnerId = ownerId,
                CreationDate = DateTime.Now,
                Options = ContestOptions.RegistrationOpen,
            };

            return await contestsRepo.AddAsync(contest);
        }

        public async Task UpdateOptions(Guid contestId, ContestOptions newOptions)
        {
            var contest = await contestsRepo.GetByIdAsync(contestId);

            //todo понять зачем это было написано :|
            // if (contest.Options.HasFlag(ContestOptions.RegistrationOpen) &&
                // !newOptions.HasFlag(ContestOptions.RegistrationOpen))
                // await SealParticipants(contest);

            contest.Options = newOptions;

            await contestsRepo.UpdateAsync(contest);
        }

        public async Task CalculateQualificationResults(Guid contestId)
        {
            var correctAnswers = await GetCorrectAnswers(contestId);
            var participations = await qualificationParticipationRepo.WhereAsync(p => p.ContestId == contestId);

            foreach (var participation in participations)
            {
                var participant = await participantsRepo.GetByIdAsync(participation.ParticipantId);

                participant.Results = correctAnswers[participant.UserSnapshot.Class.Value]
                    .Select((t, i) => t == participation.Answers[i] ? "1" : "0")
                    .ToArray();

                await participantsRepo.UpdateAsync(participant);
            }
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

        public async Task GenerateSeating(Guid contestId, Auditorium[] auditoriums)
        {
            var participants = await contestManager.GetParticipants(contestId);
            var readOnlyList = seatingGenerator.Generate(participants, auditoriums);

            foreach (var participant in readOnlyList)
                await participantsRepo.UpdateAsync(participant);
        }

        public async Task AddResultsDescription(Guid contestId, Dictionary<Class, string> tasksDescription)
        {
            var contest = await contestsRepo.GetByIdAsync(contestId);
            contest.TasksDescription = tasksDescription.ToDictionary(
                kv => kv.Key,
                kv => kv.Value.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());

            await contestsRepo.UpdateAsync(contest);
        }

        public async Task AddResults(Guid contestId, Dictionary<string, List<string>> results)
        {
            foreach (var (pass, result) in results)
            {
                var participant =
                    await participantsRepo.FirstOrDefaultAsync(p => p.ContestId == contestId && p.Pass == pass);
                if (participant == null)
                    throw new ArgumentException($"Wrong participant pass {pass}");

                participant.Results = result.ToArray();

                await participantsRepo.UpdateAsync(participant);
            }
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

        private async Task<Dictionary<Class, string[]>> GetCorrectAnswers(Guid contestId)
        {
            var tasks = await qualificationTaskRepo.WhereAsync(t => t.ContestId == contestId);
            var result = new Dictionary<Class, string[]>();
            foreach (var c in Enum.GetValues(typeof(Class)).Cast<Class>())
                result[c] = tasks.Where(t => t.Classes.Contains(c))
                    .OrderBy(t => t.Number)
                    .Select(t => t.Answer)
                    .ToArray();

            return result;
        }

        // private async Task SealParticipants(Contest contest)
        // {
        //     foreach (var t in from participant in context.Set<Participant>()
        //                       join user in context.Set<User>() on participant.UserId equals user.Id
        //                       where participant.ContestId == contest.Id
        //                       select new { user, participant })
        //         t.participant.UserSnapshot = t.user;
        //
        //     await context.SaveChangesAsync();
        // }
    }
}
