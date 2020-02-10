using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums;
using Core.Extensions;
using NewsModel = Core.DataBaseEntities.News;

namespace Core.Contests
{
    public interface IContestManager
    {
        Task<IReadOnlyList<NewsModel>> GetNews(Guid contestId);
        Task<bool> Exists(Guid contestId);
        Task<Participant> AddOrUpdateParticipant(Guid contestId, User user, string verification);
        Task<IReadOnlyList<Participant>> GetParticipants(Guid contestId);
        Task<Dictionary<int, Result[]>> GetResults(Guid contestId, bool showPreResults);
    }

    public class ContestManager : IContestManager
    {
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly IAsyncRepository<NewsModel> newsRepo;
        private readonly IAsyncRepository<Participant> participantsRepo;


        public ContestManager(
            IAsyncRepository<Contest> contestsRepo,
            IAsyncRepository<NewsModel> newsRepo,
            IAsyncRepository<Participant> participantsRepo)
        {
            this.contestsRepo = contestsRepo;
            this.newsRepo = newsRepo;
            this.participantsRepo = participantsRepo;
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

        public async Task<Dictionary<int, Result[]>> GetResults(Guid contestId, bool showPreResults)
        {
            var contest = await contestsRepo.GetByIdAsync(contestId);
            var participants = await participantsRepo.WhereAsync(p => p.ContestId == contestId);
            var participantsByClass = participants
                .Where(p => contest.Type != ContestType.Common || p.Verified)
                .Where(p => p.Results.Length != 0 && p.UserSnapshot.Class.HasValue)
                .GroupBy(p => p.UserSnapshot.Class.Value)
                .ToDictionary(
                    g => (int) g.Key,
                    g => g.Select(
                            p =>
                            {
                                var results = p.ResultsAsNumbers();
                                var city = !string.IsNullOrEmpty(p.UserSnapshot.City)
                                    ? $" ({p.UserSnapshot.City.Trim()})"
                                    : "";
                                var school = !string.IsNullOrEmpty(p.UserSnapshot.School)
                                    ? p.UserSnapshot.School.Trim()
                                    : "";
                                return new Result
                                {
                                    Id = p.Id,
                                    Name = showPreResults ? p.Login : p.UserSnapshot.Name,
                                    SchoolWithCity = showPreResults ? "" : $"{school}{city}",
                                    Results = results,
                                    Sum = results.Sum(),
                                    Place = showPreResults ? "" : (p.Place.HasValue ? p.Place.Value.ToString() : ""),
                                };
                            })
                        .OrderBy(r => int.TryParse(r.Place, out var result) ? result : int.MaxValue)
                        .ThenByDescending(r => r.Sum)
                        .ThenBy(r => r.Name)
                        .ToArray());

            foreach (var c in Enum.GetValues(typeof(Class)).Cast<int>())
                if (!participantsByClass.ContainsKey(c))
                    participantsByClass[c] = new Result[0];

            return participantsByClass;
        }


    }
}
