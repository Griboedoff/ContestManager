using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Managers
{
    public interface IContestManager
    {
        Contest Create(string title, Guid ownerId, FieldDescription[] fields);
        Task Update(Guid contestId, Guid? ownerId, ContestOptions options, FieldDescription[] fields);
        Task<Contest> Get(Guid contestId);
        Task<Contest[]> GetAll();
        Task<News[]> GetNews(Guid contestId);
        Task<News> AddNews(Guid contestId, string content);
        Task<bool> Exists(Guid contestId);
        Task<Participant> AddParticipant(Guid contestId, Guid userId);
        Task<Participant[]> GetParticipants(Guid contestId);
    }

    public class ContestManager : IContestManager
    {
        public Contest Create(string title, Guid ownerId, FieldDescription[] fields)
        {
            using var db = new Context();

            var contest = new Contest
            {
                Id = Guid.NewGuid(),
                Title = title,
                OwnerId = ownerId,
                Fields = fields,
                CreationDate = DateTime.Now,
                State = ContestState.RegistrationOpen,
            };
            db.Contests.Add(contest);
            db.SaveChanges();

            return contest;
        }

        public async Task Update(Guid contestId, Guid? ownerId, ContestOptions options, FieldDescription[] fields)
        {
            using var db = new Context();

            var contest = await db.Contests.Read(contestId);

            if (ownerId.HasValue)
                contest.OwnerId = ownerId.Value;
            contest.Options = options;
            contest.Fields = fields;

            db.SaveChanges();
        }

        public async Task<Contest> Get(Guid contestId)
        {
            using var db = new Context();

            return await db.Contests.Read(contestId);
        }

        public async Task<Contest[]> GetAll()
        {
            using var db = new Context();

            return await db.Contests.ToArrayAsync();
        }

        public Task<News[]> GetNews(Guid contestId)
        {
            using var db = new Context();

            return db.News.Where(n => n.ContestId == contestId).ToArrayAsync();
        }

        public async Task<News> AddNews(Guid contestId, string content)
        {
            using var db = new Context();

            var news = new News
            {
                ContestId = contestId,
                CreationDate = DateTime.Now,
                MdContent = content,
            };

            db.News.Add(news);
            await db.SaveChangesAsync();

            return news;
        }

        public async Task<bool> Exists(Guid contestId)
        {
            using var db = new Context();

            return await db.Contests.FindAsync(contestId) != null;
        }

        public async Task<Participant> AddParticipant(Guid contestId, Guid userId)
        {
            using var db = new Context();

            var participant = new Participant
            {
                ContestId = contestId,
                UserId = userId,
            };

            db.Participants.Add(participant);
            await db.SaveChangesAsync();

            return participant;
        }

        public Task<Participant[]> GetParticipants(Guid contestId)
        {
            using var db = new Context();
            return db.Participants.Where(p => p.ContestId == contestId).ToArrayAsync();
        }
 
    }
}