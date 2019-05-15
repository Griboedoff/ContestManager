using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums;
using Core.Models;

namespace Core.Managers
{
    public interface IContestManager
    {
        Task<Contest> Create(string title, Guid ownerId, FieldDescription[] fields);
        Task Update(Guid contestId, Guid? ownerId, ContestOptions options, FieldDescription[] fields);
        Task<Contest> Get(Guid contestId);
        Task<IReadOnlyList<Contest>> GetAll();
        Task<IReadOnlyList<News>> GetNews(Guid contestId);
        Task<News> AddNews(Guid contestId, string content);
        Task<bool> Exists(Guid contestId);
        Task<Participant> AddParticipant(Guid contestId, Guid userId);
        Task<IReadOnlyList<Participant>> GetParticipants(Guid contestId);
    }

    public class ContestManager : IContestManager
    {
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly IAsyncRepository<News> newsRepo;
        private readonly IAsyncRepository<Participant> participantsRepo;

        public ContestManager(
            IAsyncRepository<Contest> contestsRepo,
            IAsyncRepository<News> newsRepo,
            IAsyncRepository<Participant> participantsRepo)
        {
            this.contestsRepo = contestsRepo;
            this.newsRepo = newsRepo;
            this.participantsRepo = participantsRepo;
        }

        public async Task<Contest> Create(string title, Guid ownerId, FieldDescription[] fields)
        {
            var contest = new Contest
            {
                Id = Guid.NewGuid(),
                Title = title,
                OwnerId = ownerId,
                Fields = fields,
                CreationDate = DateTime.Now,
                State = ContestState.RegistrationOpen,
            };
            return await contestsRepo.AddAsync(contest);
        }

        public async Task Update(Guid contestId, Guid? ownerId, ContestOptions options, FieldDescription[] fields)
        {
            var contest = await contestsRepo.GetByIdAsync(contestId);

            if (ownerId.HasValue)
                contest.OwnerId = ownerId.Value;
            contest.Options = options;
            contest.Fields = fields;

            await contestsRepo.UpdateAsync(contest);
        }

        public async Task<Contest> Get(Guid contestId) => await contestsRepo.GetByIdAsync(contestId);

        public async Task<IReadOnlyList<Contest>> GetAll() => await contestsRepo.ListAllAsync();

        public async Task<IReadOnlyList<News>> GetNews(Guid contestId)
            => await newsRepo.WhereAsync(n => n.ContestId == contestId);

        public async Task<News> AddNews(Guid contestId, string content)
        {
            var news = new News
            {
                ContestId = contestId,
                CreationDate = DateTime.Now,
                MdContent = content,
            };

            return await newsRepo.AddAsync(news);
        }

        public async Task<bool> Exists(Guid contestId) => await contestsRepo.GetByIdAsync(contestId) != null;

        public async Task<Participant> AddParticipant(Guid contestId, Guid userId)
        {
            var participant = new Participant
            {
                ContestId = contestId,
                UserId = userId,
            };

            return await participantsRepo.AddAsync(participant);
        }

        public async Task<IReadOnlyList<Participant>> GetParticipants(Guid contestId)
            => await participantsRepo.WhereAsync(p => p.ContestId == contestId);
    }
}