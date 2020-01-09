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
        Task<NewsModel> AddNews(Guid contestId, string content);
        Task<bool> Exists(Guid contestId);
        Task<Participant> AddOrUpdateParticipant(Guid contestId, User user, string verification);
        Task<IReadOnlyList<Participant>> GetParticipants(Guid contestId);
        Task UpdateOptions(Guid contestId, ContestOptions newOptions);
        Task GenerateSeating(Guid contestId, Auditorium[] auditoriums);
        Task AddResultsDescription(Guid contestId, Dictionary<Class, string> tasksDescription);
        Task AddResults(Guid contestId, Dictionary<string, List<string>> results);
    }

    public class ContestManager : IContestManager
    {
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly IAsyncRepository<NewsModel> newsRepo;
        private readonly IAsyncRepository<Participant> participantsRepo;
        private readonly Context context;
        private readonly ISeatingGenerator seatingGenerator;

        public ContestManager(
            IAsyncRepository<Contest> contestsRepo,
            IAsyncRepository<NewsModel> newsRepo,
            IAsyncRepository<Participant> participantsRepo,
            Context context,
            ISeatingGenerator seatingGenerator)
        {
            this.contestsRepo = contestsRepo;
            this.newsRepo = newsRepo;
            this.participantsRepo = participantsRepo;
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

        public async Task<NewsModel> AddNews(Guid contestId, string content)
        {
            var news = new NewsModel
            {
                ContestId = contestId,
                CreationDate = DateTime.Now,
                Content = content,
            };

            return await newsRepo.AddAsync(news);
        }

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
            (await GetParticipantsInternal(contestId))
            .Select(p => p.WithoutLogin())
            .OrderBy(p => p.UserSnapshot.Class)
            .ThenBy(p => p.UserSnapshot.Name)
            .ToList();

        private async Task<IReadOnlyList<Participant>> GetParticipantsInternal(Guid contestId)
        {
            var contest = await contestsRepo.GetByIdAsync(contestId);
            if (contest.Options.HasFlag(ContestOptions.RegistrationOpen))
                return GetParticipantsFromUsers(contest);

            return await participantsRepo.WhereAsync(p => p.ContestId == contestId);
        }

        private IReadOnlyList<Participant> GetParticipantsFromUsers(Contest contest) =>
            context.Set<Participant>()
                .Where(p => p.ContestId == contest.Id)
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
