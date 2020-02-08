using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums;
using NewsModel = Core.DataBaseEntities.News;

namespace Core.Contests
{
    public interface IContestManager
    {
        Task<Contest> Create(CreateContestModel title, Guid ownerId);
        Task<IReadOnlyList<NewsModel>> GetNews(Guid contestId);
        Task<bool> Exists(Guid contestId);
        Task<Participant> AddOrUpdateParticipant(Guid contestId, User user, string verification);
        Task<IReadOnlyList<Participant>> GetParticipants(Guid contestId);
        Task UpdateOptions(Guid contestId, ContestOptions newOptions);
        Task GenerateSeating(Guid contestId, Auditorium[] auditoriums);
        Task AddResultsDescription(Guid contestId, Dictionary<Class, string> tasksDescription);
        Task AddResults(Guid contestId, Dictionary<string, List<string>> results);
        Task CalculateQualificationResults(Guid contestId);
        Task<Dictionary<int, Result[]>> GetResults(Guid contestId, bool showPreResults);
    }

    public class ContestManager : IContestManager
    {
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly IAsyncRepository<NewsModel> newsRepo;
        private readonly IAsyncRepository<Participant> participantsRepo;
        private readonly IAsyncRepository<QualificationParticipation> qualificationParticipationRepo;
        private readonly IAsyncRepository<QualificationTask> qualificationTaskRepo;
        private readonly Context context;
        private readonly ISeatingGenerator seatingGenerator;

        public ContestManager(
            IAsyncRepository<Contest> contestsRepo,
            IAsyncRepository<NewsModel> newsRepo,
            IAsyncRepository<Participant> participantsRepo,
            IAsyncRepository<QualificationParticipation> qualificationParticipationRepo,
            IAsyncRepository<QualificationTask> qualificationTaskRepo,
            Context context,
            ISeatingGenerator seatingGenerator)
        {
            this.contestsRepo = contestsRepo;
            this.newsRepo = newsRepo;
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

        public async Task<IReadOnlyList<NewsModel>> GetNews(Guid contestId)
            => await newsRepo.WhereAsync(n => n.ContestId == contestId);

        public async Task<bool> Exists(Guid contestId)
        {
            return await contestsRepo.GetByIdAsync(contestId) != null;
        }

        public async Task<Participant> AddOrUpdateParticipant(Guid contestId, User user, string verification)
        {
            var participant =
                await participantsRepo.FirstOrDefaultAsync(p => p.ContestId == contestId && p.UserId == user.Id);

            if (participant != null)
            {
                participant.ContestId = contestId;
                participant.UserId = user.Id;
                participant.UserSnapshot = user;
                participant.Verification = verification;

                return await participantsRepo.UpdateAsync(participant);
            }

            participant = new Participant
            {
                ContestId = contestId,
                UserId = user.Id,
                UserSnapshot = user,
                Verification = verification,
            };

            return await participantsRepo.AddAsync(participant);
        }

        public async Task<IReadOnlyList<Participant>> GetParticipants(Guid contestId) =>
            (await participantsRepo.WhereAsync(p => p.ContestId == contestId))
            .Select(p => p.WithoutLogin())
            .OrderBy(p => p.UserSnapshot.Class)
            .ThenBy(p => p.UserSnapshot.Name)
            .ToList();

        public async Task UpdateOptions(Guid contestId, ContestOptions newOptions)
        {
            var contest = await contestsRepo.GetByIdAsync(contestId);
            if (contest.Options.HasFlag(ContestOptions.RegistrationOpen) &&
                !newOptions.HasFlag(ContestOptions.RegistrationOpen))
                await SealParticipants(contest);

            contest.Options = newOptions;

            await contestsRepo.UpdateAsync(contest);
        }

        public async Task GenerateSeating(Guid contestId, Auditorium[] auditoriums)
        {
            var participants = await GetParticipants(contestId);
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

        public async Task<Dictionary<int, Result[]>> GetResults(Guid contestId, bool showPreResults)
        {
            var contest = await contestsRepo.GetByIdAsync(contestId);
            var participants = await participantsRepo.WhereAsync(p => p.ContestId == contestId);
            var participantsByClass = participants
                .Where(p => contest.Type != ContestType.Common || p.Verified)
                .Where(p => p.Results.Length != 0 && p.UserSnapshot.Class.HasValue)
                .GroupBy(p => p.UserSnapshot.Class.Value)
                .ToDictionary(
                    g => (int)g.Key,
                    g => g.Select(
                            p =>
                            {
                                var results = p.Results.Select(int.Parse).ToArray();
                                var city = (!string.IsNullOrEmpty(p.UserSnapshot.City) ? $" ({p.UserSnapshot.City})" : "");
                                return new Result
                                {
                                    Id = p.Id,
                                    Name = showPreResults ? p.Login : p.UserSnapshot.Name,
                                    SchoolWithCity = showPreResults ? "" : $"{p.UserSnapshot.School}{city}",
                                    Results = results,
                                    Sum = results.Sum(),
                                    Place = showPreResults ? "" : p.Place,
                                };
                            })
                        .OrderByDescending(r => r.Sum)
                        .ThenBy(r => r.Name)
                        .ToArray());

            foreach (var c in Enum.GetValues(typeof(Class)).Cast<int>())
                if (!participantsByClass.ContainsKey(c))
                    participantsByClass[c] = new Result[0];

            return participantsByClass;
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

        private async Task SealParticipants(Contest contest)
        {
            foreach (var t in from participant in context.Set<Participant>()
                              join user in context.Set<User>() on participant.UserId equals user.Id
                              where participant.ContestId == contest.Id
                              select new { user, participant })
                t.participant.UserSnapshot = t.user;

            await context.SaveChangesAsync();
        }
    }
}
