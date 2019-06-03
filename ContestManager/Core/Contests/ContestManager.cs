using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums;

namespace Core.Contests
{
    public interface IContestManager
    {
        Task<Contest> Create(CreateContestModel title, Guid ownerId);
        Task<IReadOnlyList<News>> GetNews(Guid contestId);
        Task<News> AddNews(Guid contestId, string content);
        Task<bool> Exists(Guid contestId);
        Task<Participant> AddParticipant(Guid contestId, Guid userId);
        Task<IReadOnlyList<Participant>> GetParticipants(Guid contestId);
        Task UpdateOptions(Guid contestId, ContestOptions newOptions);
        Task GenerateSeating(Guid contestId, Auditorium[] auditoriums);
        Task AddResultsDescription(Guid contestId, Dictionary<Class, string> tasksDescription);
    }

    public class ContestManager : IContestManager
    {
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly IAsyncRepository<News> newsRepo;
        private readonly IAsyncRepository<Participant> participantsRepo;
        private readonly Context context;
        private readonly ISeatingGenerator seatingGenerator;

        public ContestManager(
            IAsyncRepository<Contest> contestsRepo,
            IAsyncRepository<News> newsRepo,
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

        public async Task<IReadOnlyList<News>> GetNews(Guid contestId)
            => await newsRepo.WhereAsync(n => n.ContestId == contestId);

        public async Task<News> AddNews(Guid contestId, string content)
        {
            var news = new News
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

        public async Task<Participant> AddParticipant(Guid contestId, Guid userId)
        {
            var participant = new Participant
            {
                ContestId = contestId,
                UserId = userId,
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
                .Join(
                    context.Set<User>(),
                    participant => participant.UserId,
                    user => user.Id,
                    (participant, user) => new { user, participant })
                .Where(t => t.participant.ContestId == contest.Id)
                .AsEnumerable()
                .Select(t => t.participant.WithUser(t.user))
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